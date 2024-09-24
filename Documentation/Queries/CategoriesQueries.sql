-- Create new category
CREATE OR REPLACE FUNCTION fun_create_category(
    _category_name VARCHAR
) RETURNS UUID AS $$
DECLARE
    new_category_id UUID;
BEGIN
    INSERT INTO categories (category_name)
    VALUES (_category_name)
    RETURNING category_id INTO new_category_id;

    RETURN new_category_id;
END;
$$ LANGUAGE plpgsql;

-- Get all categories
CREATE OR REPLACE FUNCTION fun_get_all_categories() 
RETURNS TABLE (
    category_id UUID, 
    category_name VARCHAR
) AS $$
BEGIN
    RETURN QUERY SELECT * FROM categories;
END;
$$ LANGUAGE plpgsql;

-- Get one category
CREATE OR REPLACE FUNCTION fun_get_category(_category_id UUID) 
RETURNS TABLE (
    category_id UUID, 
    category_name VARCHAR
) AS $$
BEGIN
    RETURN QUERY SELECT * FROM categories WHERE category_id = _category_id;
END;
$$ LANGUAGE plpgsql;

-- Update category
CREATE OR REPLACE FUNCTION fun_update_category(
    _category_id UUID, 
    _category_name VARCHAR
) RETURNS VOID AS $$
BEGIN
    UPDATE categories
    SET category_name = _category_name
    WHERE category_id = _category_id;
END;
$$ LANGUAGE plpgsql;

-- Delete category
CREATE OR REPLACE FUNCTION fun_delete_category(
  _category_id UUID
) RETURNS VOID AS 
$$
BEGIN
    DELETE FROM categories WHERE category_id = _category_id;
END;
$$ LANGUAGE plpgsql;
