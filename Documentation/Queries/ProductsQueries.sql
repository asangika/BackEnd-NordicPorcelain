--Insert product
CREATE OR REPLACE FUNCTION create_product(
    p_category_id UUID,
    p_description VARCHAR(100),
    p_price DECIMAL(10,2),
    p_stock INT,
    p_product_line VARCHAR(100),
    p_image_URL VARCHAR(200),
    p_isPrimary BOOLEAN,
    p_image_text VARCHAR(200),
    p_size_value ENUM, 
    p_stock_quantity INT,
    p_color_name VARCHAR(100)
) RETURNS BOOLEAN AS $$
DECLARE
    new_product_id UUID;
BEGIN
	IF NOT EXISTS (SELECT 1 FROM categories WHERE category_id = p_category_id) THEN
        RAISE EXCEPTION 'Category ID % does not exist', p_category_id;
    END IF;

	IF p_price < 0 THEN
        RAISE EXCEPTION 'Price cannot be negative';
    END IF;

	IF p_stock < 0 THEN
        RAISE EXCEPTION 'Stock cannot be negative';
    END IF;
	
    INSERT INTO products (product_id, category_id, description, price, stock, product_line)
    VALUES (p_product_id, p_category_id, p_description, p_price, p_stock, p_product_line);

    IF p_image_URL IS NOT NULL THEN
        INSERT INTO product_images (product_id, image_URL, isPrimary, image_text)
        VALUES (new_product_id, p_image_URL, p_isPrimary, p_image_text);
    END IF;

	IF p_size_value IS NOT NULL AND p_stock_quantity IS NOT NULL THEN
        INSERT INTO product_sizes (product_id, size_value, stock_quantity)
        VALUES (new_product_id, p_size_value, p_stock_quantity);
    END IF;
	
	IF p_color_name IS NOT NULL THEN
        INSERT INTO product_colors (product_id, color_name)
        VALUES (new_product_id, p_color_name);
    END IF;
	
    RETURN TRUE;
EXCEPTION
    WHEN OTHERS THEN
        RETURN FALSE;
END;
$$ LANGUAGE plpgsql;


--Retrieve Products
CREATE OR REPLACE FUNCTION get_all_products()
RETURNS TABLE (
    product_id UUID,
    category_id UUID,
    description CHAR(100),
    price DECIMAL(10,2),
    stock INT,
    product_line CHAR(100),
    image_URL CHAR(200),
    isPrimary BOOL,
    image_text CHAR(200),
    size_value CHAR(100),
    stock_quantity INT,
    color_name CHAR(100)
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.product_id,
        p.category_id,
        p.description,
        p.price,
        p.stock,
        p.product_line,
        pi.image_URL,
        pi.isPrimary,
        pi.image_text,
        ps.size_value,
        ps.stock_quantity,
        pc.color_name
    FROM 
        products p
    LEFT JOIN 
        product_images pi ON p.product_id = pi.product_id
    LEFT JOIN 
        product_sizes ps ON p.product_id = ps.product_id
    LEFT JOIN 
        product_colors pc ON p.product_id = pc.product_id
	WHERE 
        p.product_id IS NOT NULL 
        AND p.category_id IS NOT NULL 
        AND p.price >= 0 
        AND p.stock >= 0;
EXCEPTION
    WHEN OTHERS THEN
        RAISE WARNING 'An error occurred while retrieving products.';
        RETURN;
END;
$$ LANGUAGE plpgsql;



--get product by id
CREATE OR REPLACE FUNCTION get_product_byId(
    p_product_id UUID
)
RETURNS TABLE (
    product_id UUID,
    category_id UUID,
    description CHAR(100),
    price DECIMAL(10,2),
    stock INT,
    product_line CHAR(100),
    image_URL CHAR(200),
    isPrimary BOOL,
    image_text CHAR(200),
    size_value CHAR(100),
    stock_quantity INT,
    color_name CHAR(100)
) AS $$
BEGIN
	IF p_product_id IS NULL THEN
        RAISE EXCEPTION 'Product ID cannot be NULL';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM products WHERE product_id = p_product_id) THEN
        RAISE EXCEPTION 'Product with ID % does not exist', p_product_id;
    END IF;
	
    RETURN QUERY
    SELECT 
        p.product_id,
        p.category_id,
        p.description,
        p.price,
        p.stock,
        p.product_line,
        pi.image_URL,
        pi.isPrimary,
        pi.image_text,
        ps.size_value,
        ps.stock_quantity,
        pc.color_name
    FROM 
        products p
    LEFT JOIN 
        product_images pi ON p.product_id = pi.product_id
    LEFT JOIN 
        product_sizes ps ON p.product_id = ps.product_id
    LEFT JOIN 
        product_colors pc ON p.product_id = pc.product_id
    WHERE 
        p.product_id = p_product_id;
EXCEPTION
    WHEN OTHERS THEN
        RAISE WARNING 'An error occurred while retrieving product with ID %: %', p_product_id, SQLERRM;
        RETURN;
END;
$$ LANGUAGE plpgsql;


--update product
CREATE OR REPLACE FUNCTION update_product(
    p_product_id UUID,
    p_category_id UUID DEFAULT NULL,
    p_description CHAR(100)DEFAULT NULL,
    p_price DECIMAL(10,2)DEFAULT NULL,
    p_stock INT DEFAULT NULL,
    p_product_line CHAR(100) DEFAULT NULL,
    p_image_URL CHAR(200) DEFAULT NULL,
    p_isPrimary BOOL DEFAULT NULL,
    p_image_text CHAR(200) DEFAULT NULL,
    p_size_value CHAR(100) DEFAULT NULL,
    p_stock_quantity INT DEFAULT NULL,
    p_color_name CHAR(100) DEFAULT NULL
) RETURNS BOOLEAN AS $$
DECLARE
    v_sql TEXT;
	v_fields_updated BOOLEAN := FALSE;
BEGIN
	IF p_product_id IS NULL THEN
        RAISE EXCEPTION 'Product ID cannot be NULL';
    END IF;

	IF NOT EXISTS (SELECT 1 FROM products WHERE product_id = p_product_id) THEN
        RAISE EXCEPTION 'Product with ID % does not exist', p_product_id;
    END IF;
	
    v_sql := 'UPDATE products SET';
    IF p_category_id IS NOT NULL THEN v_sql := v_sql || ' category_id = ' || quote_literal(p_category_id) || ','; v_fields_updated := TRUE; END IF;
    IF p_description IS NOT NULL THEN v_sql := v_sql || ' description = ' || quote_literal(p_description) || ','; v_fields_updated := TRUE; END IF;
    IF p_price IS NOT NULL THEN v_sql := v_sql || ' price = ' || p_price || ','; v_fields_updated := TRUE; END IF;
    IF p_stock IS NOT NULL THEN v_sql := v_sql || ' stock = ' || p_stock || ','; v_fields_updated := TRUE; END IF;
    IF p_product_line IS NOT NULL THEN v_sql := v_sql || ' product_line = ' || quote_literal(p_product_line) || ','; v_fields_updated := TRUE; END IF;

	IF NOT v_fields_updated THEN
        RAISE EXCEPTION 'No fields provided for update';
    END IF;
	 
    v_sql := rtrim(v_sql, ',') || ' WHERE product_id = ' || quote_literal(p_product_id) || ';';

    EXECUTE v_sql;

    PERFORM update_product_images(p_product_id, p_image_URL, p_isPrimary, p_image_text);
    PERFORM update_product_sizes(p_product_id, p_size_value, p_stock_quantity);
    PERFORM update_product_colors(p_product_id, p_color_name);

    IF SQL%ROWCOUNT > 0 THEN
        RETURN TRUE;
    ELSE
        RETURN FALSE;
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        RAISE WARNING 'An error occurred while updating product with ID %: %', p_product_id, SQLERRM;
        RETURN FALSE;
END;
$$ LANGUAGE plpgsql;


-- delete product
CREATE OR REPLACE FUNCTION delete_product(
    p_product_id UUID
) 
RETURNS BOOLEAN AS $$
DECLARE
    v_product_exists BOOLEAN;
BEGIN
 	IF p_product_id IS NULL THEN
        RAISE EXCEPTION 'Product ID cannot be NULL';
    END IF;

    SELECT EXISTS(SELECT 1 FROM products WHERE product_id = p_product_id) INTO v_product_exists;
    IF NOT v_product_exists THEN
        RAISE EXCEPTION 'Product with ID % does not exist', p_product_id;
    END IF;

    DELETE FROM product_images
    WHERE product_id = p_product_id;
    
    DELETE FROM product_sizes
    WHERE product_id = p_product_id;

    DELETE FROM product_colors
    WHERE product_id = p_product_id;

    DELETE FROM products
    WHERE product_id = p_product_id;
  
    IF SQL%ROWCOUNT > 0 THEN
        RETURN TRUE;
    ELSE
        RETURN FALSE;
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        RAISE WARNING 'An error occurred while deleting product with ID %: %', p_product_id, SQLERRM;
        RETURN FALSE;
END;
$$ LANGUAGE plpgsql;

--get most purchased products
CREATE OR REPLACE FUNCTION get_most_purchased_products(
	p_limit INT
)
RETURNS TABLE (
    product_id UUID,
    product_name CHAR(100),
    total_quantity_purchased INT
) AS $$
BEGIN
	IF p_limit IS NULL OR p_limit <= 0 THEN
        RAISE EXCEPTION 'The limit must be a positive integer greater than 0';
    END IF;
	
    RETURN QUERY
    SELECT
        p.product_id,
        p.description AS product_name,
        COALESCE(SUM(oi.quantity), 0) AS total_quantity_purchased
    FROM
        products p
    JOIN
        order_items oi ON p.product_id = oi.product_id
    JOIN
        orders o ON o.order_id = oi.order_id
    WHERE
        o.order_status = 'completed'
    GROUP BY
        p.product_id, p.description
    ORDER BY
        total_quantity_purchased DESC
	LIMIT p_limit;

EXCEPTION
    WHEN OTHERS THEN
        RAISE WARNING 'An error occurred while retrieving most purchased products: %', SQLERRM;
        RETURN;
END;
$$ LANGUAGE plpgsql;


--update product images
CREATE OR REPLACE FUNCTION update_product_images(
    p_product_id UUID,
    p_image_URL CHAR(200) DEFAULT NULL,
    p_isPrimary BOOL DEFAULT NULL,
    p_image_text CHAR(200) DEFAULT NULL
) RETURNS VOID AS $$
DECLARE
    v_sql TEXT;
BEGIN
    v_sql := 'UPDATE product_images SET';
    IF p_image_URL IS NOT NULL THEN v_sql := v_sql || ' image_URL = ' || quote_literal(p_image_URL) || ','; END IF;
    IF p_isPrimary IS NOT NULL THEN v_sql := v_sql || ' isPrimary = ' || p_isPrimary || ','; END IF;
    IF p_image_text IS NOT NULL THEN v_sql := v_sql || ' image_text = ' || quote_literal(p_image_text) || ','; END IF;

    v_sql := rtrim(v_sql, ',') || ' WHERE product_id = ' || quote_literal(p_product_id) || ';';

    EXECUTE v_sql;
END;
$$ LANGUAGE plpgsql;

--update product size
CREATE OR REPLACE FUNCTION update_product_sizes(
    p_product_id UUID,
    p_size_value CHAR(100) DEFAULT NULL,
    p_stock_quantity INT DEFAULT NULL
) RETURNS VOID AS $$
DECLARE
    v_sql TEXT;
BEGIN
    v_sql := 'UPDATE product_sizes SET';
    IF p_size_value IS NOT NULL THEN v_sql := v_sql || ' size_value = ' || quote_literal(p_size_value) || ','; END IF;
    IF p_stock_quantity IS NOT NULL THEN v_sql := v_sql || ' stock_quantity = ' || p_stock_quantity || ','; END IF;

    v_sql := rtrim(v_sql, ',') || ' WHERE product_id = ' || quote_literal(p_product_id) || ';';

    EXECUTE v_sql;
END;
$$ LANGUAGE plpgsql;

--update product colors
CREATE OR REPLACE FUNCTION update_product_colors(
    p_product_id UUID,
    p_color_name CHAR(100) DEFAULT NULL
) RETURNS VOID AS $$
DECLARE
    v_sql TEXT;
BEGIN
    v_sql := 'UPDATE product_colors SET';
    IF p_color_name IS NOT NULL THEN v_sql := v_sql || ' color_name = ' || quote_literal(p_color_name) || ','; END IF;

    v_sql := rtrim(v_sql, ',') || ' WHERE product_id = ' || quote_literal(p_product_id) || ';';

    EXECUTE v_sql;
END;
$$ LANGUAGE plpgsql;
