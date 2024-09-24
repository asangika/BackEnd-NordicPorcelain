-- Create new user
CREATE OR REPLACE FUNCTION create_user_func(
    p_user_name VARCHAR, 
    p_email VARCHAR, 
    p_password VARCHAR, 
    p_phone_number VARCHAR, 
    p_role role_type,
    p_address address_type,
    p_is_default BOOLEAN
) RETURNS user_type AS 
$$
DECLARE
    new_user_id UUID;
    new_address_id UUID;
    user_summary user_type;
    salt_value VARCHAR;
BEGIN
    -- Check if user exists
    PERFORM 1 FROM users WHERE email = p_email;
    IF FOUND THEN
        RAISE EXCEPTION 'User already exist';
    END IF;

    -- Generate a unique salt
    salt_value := gen_salt('bf');

    INSERT INTO users (user_name, email, password, phone_number, role)
        VALUES (p_user_name, p_email, crypt(p_password, salt_value), p_phone_number, p_role)
        RETURNING user_id INTO new_user_id;

    INSERT INTO address(unit_number, street_number, address_line1, address_line2, city,
                postal_code, country) 
        VALUES(p_address.unit_number, p_address.street_number, p_address.address_line1, p_address.address_line2,
            p_address.city, p_address.postal_code, p_address.country)
        RETURNING address_id INTO new_address_id;
    
    INSERT INTO user_address(user_id, address_id, is_default)
    VALUES(new_user_id, new_address_id, p_is_default);

    -- Construct the user_type
    SELECT u.user_id, u.email, u.phone_number, u.role,
           a.unit_number AS address_unit_number,
           a.street_number AS address_street_number,
           a.address_line1 AS address_line1,
           a.address_line2 AS address_line2,
           a.city AS address_city,
           a.postal_code AS address_postal_code,
           a.country AS address_country,
    INTO user_summary
    FROM users u
    JOIN user_address ua ON ua.user_id = u.user_id
    JOIN address a ON a.address_id = ua.address_id
    WHERE u.user_id = new_user_id;
    RETURN user_summary;
END;
$$ LANGUAGE plpgsql;

-- Get all Users
CREATE OR REPLACE FUNCTION get_all_users_func(
    p_page INTEGER DEFAULT 1,
    p_limit INTEGER DEFAULT 10
)
RETURNS TABLE (
    user_id UUID, 
    user_name VARCHAR, 
    email VARCHAR, 
    phone_number VARCHAR, 
    role role_type,
    address_id UUID,
    unit_number INTEGER,
    street_number INTEGER,
    address_line1 VARCHAR,
    address_line2 VARCHAR,
    city VARCHAR,
    postal_code VARCHAR,
    country VARCHAR,
    is_default BOOLEAN
) AS 
$$
BEGIN
    RETURN QUERY 
    SELECT u.user_id, u.user_name, u.email, u.phone_number, u.role,
        a.address_id,
        a.unit_number,
        a.street_number,
        a.address_line1,
        a.address_line2,
        a.city,
        a.postal_code,
        a.country,
        ua.is_default
    FROM users u
    JOIN user_address ua ON ua.user_id = u.user_id
    JOIN address a ON a.address_id = ua.address_id
    OFFSET (p_page - 1) * p_limit
    LIMIT p_limit;
END;
$$ LANGUAGE plpgsql;

-- Get user by id
CREATE OR REPLACE FUNCTION get_user_by_id_func(p_user_id UUID) 
RETURNS user_type AS 
$$
DECLARE
    user_summary user_type;
BEGIN
    SELECT u.user_id, u.user_name, u.email, u.phone_number, u.role 
        a.address_id,
        a.unit_number,
        a.street_number,
        a.address_line1,
        a.address_line2,
        a.city,
        a.postal_code,
        a.country,
        ua.is_default
    INTO user_summary
    FROM users u 
    JOIN user_address ua ON ua.user_id = u.user_id
    JOIN address a ON a.address_id = ua.address_id
    WHERE u.user_id = p_user_id;

    RETURN user_summary
END;
$$ LANGUAGE plpgsql;

-- Update user information
CREATE OR REPLACE FUNCTION update_user_func(
    p_user_id UUID,
    p_user_name VARCHAR DEFAULT NULL,
    p_email VARCHAR DEFAULT NULL,
    p_phone_number VARCHAR DEFAULT NULL,
    p_password VARCHAR DEFAULT NULL,
    p_unit_number VARCHAR DEFAULT NULL,
    p_street_number INTEGER DEFAULT NULL,
    p_address_line1 VARCHAR DEFAULT NULL,
    p_address_line2 VARCHAR DEFAULT NULL,
    p_city VARCHAR DEFAULT NULL,
    p_postal_code VARCHAR DEFAULT NULL,
    p_country VARCHAR DEFAULT NULL,
    p_is_default BOOLEAN DEFAULT NULL
)
RETURNS TABLE (
    user_id UUID,
    user_name VARCHAR,
    email VARCHAR,
    phone_number VARCHAR,
    role role_type,
    unit_number VARCHAR,
    street_number INTEGER,
    address_line1 VARCHAR,
    address_line2 VARCHAR,
    city VARCHAR,
    postal_code VARCHAR,
    country VARCHAR,
    is_default BOOLEAN
) AS
$$
BEGIN
    -- Update user details only for provided fields
    UPDATE users
    SET 
        user_name = COALESCE(p_user_name, user_name),
        email = COALESCE(p_email, email),
        phone_number = COALESCE(p_phone_number, phone_number),
        password = COALESCE(p_password, password)
    WHERE user_id = p_user_id;

    -- Update user address only if any address field is provided
    IF p_unit_number IS NOT NULL OR p_street_number IS NOT NULL OR 
       p_address_line1 IS NOT NULL OR p_address_line2 IS NOT NULL OR 
       p_city IS NOT NULL OR p_postal_code IS NOT NULL OR p_country IS NOT NULL THEN
       
       UPDATE address
       SET 
           unit_number = COALESCE(p_unit_number, unit_number),
           street_number = COALESCE(p_street_number, street_number),
           address_line1 = COALESCE(p_address_line1, address_line1),
           address_line2 = COALESCE(p_address_line2, address_line2),
           city = COALESCE(p_city, city),
           postal_code = COALESCE(p_postal_code, postal_code),
           country = COALESCE(p_country, country)
       WHERE address_id = (SELECT address_id FROM user_address WHERE user_id = p_user_id AND is_default = TRUE);

       -- Update the is_default flag only if provided
       IF p_is_default IS NOT NULL THEN
           UPDATE user_address
           SET is_default = p_is_default
           WHERE user_id = p_user_id AND address_id = (SELECT address_id FROM user_address WHERE user_id = p_user_id AND is_default = TRUE);
       END IF;
    END IF;

    -- Return the updated user info
    RETURN QUERY
    SELECT 
        u.user_id, u.user_name, u.email, u.phone_number, u.role,
        a.unit_number, a.street_number, a.address_line1, a.address_line2, a.city, a.postal_code, a.country,
        ua.is_default
    FROM users u
    LEFT JOIN user_address ua ON ua.user_id = u.user_id AND ua.is_default = TRUE
    LEFT JOIN address a ON a.address_id = ua.address_id
    WHERE u.user_id = p_user_id;
END;
$$ LANGUAGE plpgsql;


-- Delete user
CREATE OR REPLACE FUNCTION delete_user_func(p_user_id UUID) 
RETURNS VOID AS 
$$
BEGIN
    -- Check if user exists
    PERFORM 1 FROM users WHERE user_id = p_user_id;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'User does not exist';
    END IF;

    -- Delete user addresses
    DELETE FROM user_address WHERE user_id = p_user_id;

    -- Delete addresses only if they are not shared with other users
    DELETE FROM address WHERE address_id NOT IN (SELECT address_id FROM user_address);

    -- anonymize orders instead of deleting them
    UPDATE orders
    SET user_id = NULL
    WHERE user_id = p_user_id;

    UPDATE payments
    SET user_id = NULL
    WHERE user_id = p_user_id;
    
    -- delete the user
    DELETE FROM users WHERE user_id = p_user_id;
END;
$$ LANGUAGE plpgsql;



