-- Create new review
CREATE OR REPLACE FUNCTION create_review_func(
    p_user_id UUID, 
    p_product_id UUID,
    p_review_text VARCHAR, 
    p_rating INTEGER
) 
RETURNS review_type AS $$
DECLARE
    review_summary review_type;
BEGIN

    -- Check if user exists
    PERFORM 1 FROM users WHERE user_id = p_user_id;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'User does not exist';
    END IF;

    -- Check if product exists
    PERFORM 1 FROM products WHERE product_id = p_product_id;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Product does not exist';
    END IF;


    INSERT INTO reviews (user_id, product_id, review_text, rating)
    VALUES (p_user_id, p_product_id, p_reveiw_text, p_rating)
    RETURNING review_id INTO new_review_id;

    -- Construct the review_summary
    SELECT r.review_id, r.user_id, r.product_id, r.review_date, r.review_text, r.rating
    FROM reviews r
    WHERE r.review_id = new_review_id

    -- returing review
    RETURN review_summary;
END;
$$ LANGUAGE plpgsql;

-- Get all reviews
CREATE OR REPLACE FUNCTION get_all_reviews_func(
    p_page INTEGER DEFAULT 1,
    p_limit INTEGER DEFAULT 10
) 
RETURNS TABLE (
    review_id UUID, 
    user_id UUID, 
    product_id UUID, 
    review_date TIMESTAMP,
    review_text VARCHAR, 
    rating INTEGER

) AS 
$$
BEGIN
    RETURN QUERY 
    SELECT r.review_id, r.user_id, r.product_id, r.review_date, r.review_text, r.rating 
    FROM reviews r
    OFFSET (p_page - 1) * p_limit
    LIMIT p_limit;
END;
$$ LANGUAGE plpgsql;

-- Get all review of the product
CREATE OR REPLACE FUNCTION get_all_reviews_by_product_id_func(p_product_id UUID)
RETURNS TABLE (
    review_id UUID, 
    user_id UUID, 
    product_id UUID,
    review_date TIMESTAMP,
    review_text VARCHAR, 
    rating INTEGER
) AS $$
BEGIN
    -- Check if product exists
    PERFORM 1 FROM products WHERE product_id = p_product_id;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Product does not exist';
    END IF;

    RETURN QUERY 
    SELECT r.review_id, r.user_id, r.product_id, r.review_date, r.review_text, r.rating 
    FROM reviews r WHERE r.product_id = p_product_id;
END;
$$ LANGUAGE plpgsql;

-- Get all user reviews
CREATE OR REPLACE FUNCTION get_all_reviews_by_user_id_func(p_user_id UUID)
RETURNS TABLE (
    review_id UUID, 
    user_id UUID, 
    product_id UUID,
    review_date TIMESTAMP,
    review_text VARCHAR, 
    rating INTEGER
) AS $$
BEGIN
    -- Check if user exists
    PERFORM 1 FROM users WHERE user_id = p_user_id;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'User does not exist';
    END IF;

    RETURN QUERY 
    SELECT r.review_id, r.user_id, r.product_id, r.review_date, r.review_text, r.rating 
    FROM reviews r
    WHERE r.user_id = p_user_id;
END;
$$ LANGUAGE plpgsql;

-- Get a single review
CREATE OR REPLACE FUNCTION get_review_by_id_func(p_review_id UUID)
RETURNS review_type AS 
$$
BEGIN
    -- Check if review exists
    PERFORM 1 FROM reviews WHERE review_id = p_review_id;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Review does not exist';
    END IF;

    RETURN QUERY
    SELECT r.review_id, r.user_id, r.product_id, r.review_date, r.review_text, r.rating 
    FROM reviews r
    WHERE r.review_id = p_review_id;
END;
$$ LANGUAGE plpgsql;

-- Update One Review
CREATE OR REPLACE FUNCTION update_review_func(
    p_review_id UUID,
    p_rating INTEGER,
    p_review_text VARCHAR
)
RETURNS TABLE (
    review_id UUID,
    product_id UUID,
    user_id UUID,
    review_date TIMESTAMP,
    rating INTEGER,
    review_text VARCHAR
) AS
$$
BEGIN
    -- Check if the review exists
    PERFORM 1 FROM reviews WHERE review_id = p_review_id;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Review not found';
    END IF;

    -- Update the review
    UPDATE reviews
    SET 
        rating = p_rating,
        review_text = p_review_text,
        review_date = NOW()  -- Update review date to current timestamp
    WHERE review_id = p_review_id;

    -- Return the updated review
    RETURN QUERY
    SELECT 
        r.review_id, 
        r.product_id, 
        r.user_id, 
        r.review_date, 
        r.rating, 
        r.review_text
    FROM reviews r
    WHERE r.review_id = p_review_id;
END;
$$ LANGUAGE plpgsql;

-- Delete one review
CREATE OR REPLACE FUNCTION delete_review_func(p_review_id UUID) 
RETURNS VOID AS $$
BEGIN
    -- Check if review exists
    PERFORM 1 FROM reviews WHERE review_id = p_review_id;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Review does not exist';
    END IF;

    DELETE FROM reviews WHERE review_id = p_review_id;
END;
$$ LANGUAGE plpgsql;
