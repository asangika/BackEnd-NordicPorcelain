-- Create new order
CREATE OR REPLACE FUNCTION create_order_func(
    p_user_id UUID,
    p_order_items order_item_type[],
    p_order_date TIMESTAMP,
    p_total_price NUMERIC,
    p_shipment shipment_type,
    p_order_status VARCHAR
)
RETURNS order_summary_type AS
$$
DECLARE
    new_order_id UUID;
    new_shipment_id UUID;
    new_address_id UUID;
    item order_item_type;
    item_price NUMERIC;
    order_summary order_summary_type;
BEGIN
    -- Check if user exists
    PERFORM 1 FROM users WHERE user_id = p_user_id;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'User does not exist';
    END IF;

    -- Insert Address
    INSERT INTO address(
        unit_number, street_number, address_line1, address_line2, city, postal_code, country
    ) VALUES (
        p_shipment.address.unit_number, 
        p_shipment.address.street_number, 
        p_shipment.address.address_line1, 
        p_shipment.address.address_line2, 
        p_shipment.address.city, 
        p_shipment.address.postal_code, 
        p_shipment.address.country
    ) RETURNING address_id INTO new_address_id;

    -- Insert new order
    INSERT INTO orders(user_id, order_date, total_price, shipping_address_id, order_status)
    VALUES (p_user_id, p_order_date, p_total_price, new_address_id, p_order_status)
    RETURNING order_id INTO new_order_id;

    -- Insert Shipment
    INSERT INTO shipments(order_id, shipment_date, address_id)
    VALUES (new_order_id, p_shipment.shipment_date, new_address_id)
    RETURNING shipment_id INTO new_shipment_id;

    -- Process each order item
    FOREACH item IN ARRAY p_order_items
    LOOP
        -- Check if product exists
        PERFORM 1 FROM products WHERE product_id = item.product_id;
        IF NOT FOUND THEN
            RAISE EXCEPTION 'Product with id % does not exist', item.product_id;
        END IF;

        -- Check if there is enough stock
        IF (SELECT stock FROM products WHERE product_id = item.product_id) < item.quantity THEN
            RAISE EXCEPTION 'Not enough inventory for product_id %', item.product_id;
        END IF;

        -- Get the price of the product
        SELECT price INTO item_price FROM products WHERE product_id = item.product_id;

        -- Insert into order_items
        INSERT INTO order_items(order_id, product_id, quantity, price)
        VALUES (new_order_id, item.product_id, item.quantity, item_price);

        -- Update product stock
        UPDATE products
        SET stock = stock - item.quantity
        WHERE product_id = item.product_id;
    END LOOP;

    -- Update the total price if necessary
    UPDATE orders
    SET total_price = (
        SELECT SUM(price * quantity)
        FROM order_items
        WHERE order_id = new_order_id
    )
    WHERE order_id = new_order_id;

    -- Construct the order summary
    SELECT o.order_id, o.user_id, o.order_date, o.total_price, o.order_status,
           s.shipment_id, s.shipment_date,
           a.unit_number AS address_unit_number,
           a.street_number AS address_street_number,
           a.address_line1 AS address_line1,
           a.address_line2 AS address_line2,
           a.city AS address_city,
           a.postal_code AS address_postal_code,
           a.country AS address_country,
           ARRAY(
               SELECT ROW(oi.product_id, oi.quantity, oi.price)::order_item_type
               FROM order_items oi
               WHERE oi.order_id = o.order_id
           ) AS order_items
    INTO order_summary
    FROM orders o
    JOIN shipments s ON s.order_id = o.order_id
    JOIN address a ON a.address_id = o.shipping_address_id
    WHERE o.order_id = new_order_id;

    -- Return the order summary
    RETURN order_summary;

END;
$$ LANGUAGE plpgsql;


-- Get all orders
CREATE OR REPLACE FUNCTION get_all_orders_func(
    p_user_id UUID DEFAULT NULL,
    p_page INTEGER DEFAULT 1,
    p_limit INTEGER DEFAULT 10
)
RETURNS TABLE (
    user_id UUID,
    order_items order_item_type[],
    order_date TIMESTAMP,
    total_price NUMERIC,
    shipment_id UUID,
    shipment_date TIMESTAMP,
    address_id UUID,
    unit_number VARCHAR,
    street_number INTEGER,
    address_line1 VARCHAR,
    address_line2 VARCHAR,
    city VARCHAR,
    postal_code VARCHAR,
    country VARCHAR,
    order_status VARCHAR
) AS
$$
BEGIN
    RETURN QUERY
    SELECT 
        o.user_id,
        ARRAY(
            SELECT ROW(oi.product_id, oi.quantity, oi.price)::order_item_type
            FROM order_items oi
            WHERE oi.order_id = o.order_id
        ) AS order_items,
        o.order_date,
        o.total_price,
        s.shipment_id,
        s.shipment_date,
        a.address_id,
        a.unit_number,
        a.street_number,
        a.address_line1,
        a.address_line2,
        a.city,
        a.postal_code,
        a.country,
        o.order_status
    FROM orders o
    LEFT JOIN shipments s ON o.order_id = s.order_id
    LEFT JOIN address a ON s.address_id = a.address_id
    WHERE (p_user_id IS NULL OR o.user_id = p_user_id)
    ORDER BY o.order_date DESC
    OFFSET (p_page - 1) * p_limit
    LIMIT p_limit;
END;
$$ LANGUAGE plpgsql;

-- Get one order
CREATE OR REPLACE FUNCTION get_order_by_id_func(p_order_id UUID)
RETURNS TABLE (
    user_id UUID,
    order_items order_summary_type[],
    order_date TIMESTAMP,
    total_price NUMERIC,
    shipment_id UUID,
    shipment_date TIMESTAMP,
    address address_type,
    order_status VARCHAR
)
LANGUAGE plpgsql AS
$$
BEGIN
    RETURN QUERY
    SELECT 
        o.user_id,
        ARRAY(
            SELECT ROW(oi.order_item_id, oi.quantity, oi.price)::order_summary_type
            FROM order_items oi
            WHERE oi.order_id = o.order_id
        ) AS order_items,
        o.order_date,
        o.total_price,
        s.shipment_id,
        s.shipment_date,
        ROW(a.address_id, a.unit_number, a.street_number, a.address_line1, 
            a.address_line2, a.city, a.postal_code, a.country)::address_type AS address,
        o.order_status
    FROM orders o
    LEFT JOIN shipments s ON o.order_id = s.order_id
    LEFT JOIN address a ON s.address_id = a.address_id
    WHERE o.order_id = p_order_id;
    
    -- If no rows are found, raise an exception
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Order not found';
    END IF;
END;
$$;

-- update order status
CREATE OR REPLACE FUNCTION update_order_by_id_func(
    p_order_id UUID,
    p_order_items order_summary_type[]
)
RETURNS TABLE (
    user_id UUID,
    order_items order_summary_type[],
    order_date TIMESTAMP,
    total_price NUMERIC,
    shipment_id UUID,
    shipment_date TIMESTAMP,
    address address_type,
    order_status VARCHAR
)
LANGUAGE plpgsql AS
$$
DECLARE
    new_total_price NUMERIC := 0;
    item order_summary_type;
BEGIN
    -- Check if the order exists
    IF NOT EXISTS (SELECT 1 FROM orders WHERE order_id = p_order_id) THEN
        RAISE EXCEPTION 'Order not found';
    END IF;

    -- Loop through the provided order items and update them
    FOREACH item IN ARRAY p_order_items
    LOOP
        -- Update the order item in the database
        UPDATE order_items
        SET quantity = item.quantity,
            price = item.price
        WHERE order_item_id = item.order_item_id
          AND order_id = p_order_id;
          
        -- Recalculate the total price
        new_total_price := new_total_price + (item.quantity * item.price);
    END LOOP;

    -- Update the total price in the orders table
    UPDATE orders
    SET total_price = new_total_price
    WHERE order_id = p_order_id;

    -- Return the updated order summary
    RETURN QUERY
    SELECT 
        o.user_id,
        ARRAY(
            SELECT ROW(oi.order_item_id, oi.quantity, oi.price)::order_summary_type
            FROM order_items oi
            WHERE oi.order_id = o.order_id
        ) AS order_items,
        o.order_date,
        o.total_price,
        s.shipment_id,
        s.shipment_date,
        ROW(a.address_id, a.unit_number, a.street_number, a.address_line1, 
            a.address_line2, a.city, a.postal_code, a.country)::address_type AS address,
        o.order_status
    FROM orders o
    LEFT JOIN shipments s ON o.order_id = s.order_id
    LEFT JOIN address a ON s.address_id = a.address_id
    WHERE o.order_id = p_order_id;
END;
$$;


-- Delete an order
CREATE OR REPLACE FUNCTION fun_delete_order_func(_order_id UUID) 
RETURNS VOID AS 
$$
BEGIN
    -- Check if the order exists
    IF NOT EXISTS (SELECT 1 FROM orders WHERE order_id = p_order_id) THEN
        RAISE EXCEPTION 'Order not found';
    END IF;

    -- Delete order items
    DELETE FROM order_items WHERE order_id = p_order_id;
    
    -- Delete the order from the orders table
    DELETE FROM orders WHERE order_id = p_order_id;

    -- Delete shipment data if found
    DELETE FROM shipments WHERE order_id = p_order_id;
END;
$$ LANGUAGE plpgsql;
