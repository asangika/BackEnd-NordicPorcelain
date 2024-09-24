CREATE TYPE order_status AS ENUM ('pending','confirmed', 'cancelled');
CREATE TYPE shipment_status AS ENUM ('pending','confirmed', 'cancelled');
CREATE TYPE payment_status AS ENUM ('pending', 'payed', 'cnacelled');
CREATE TYPE role_type AS ENUM ('admin', 'customer');
CREATE TYPE order_item_type AS (product_id UUID, quantity INTEGER, price NUMERiC);
CREATE TYPE order_summary_type AS (
    order_id UUID,
    user_id UUID,
    order_date TIMESTAMP,
    total_price NUMERIC,
    order_status VARCHAR,
    shipment_id UUID,
    shipment_date TIMESTAMP,
);


CREATE TYPE shipment_type AS (
    shipmet_date date,
    shipment_status shipment_status,
    shipment_address address[]
);

CREATE TYPE user_address_type AS(
    user_id UUID,
    address_id UUID,
    is_default boolean
)

CREATE TYPE user_type AS(
    user_name VARCHAR,
    email VARCHAR,
    phone_number VARCHAR,
    role role_type,
    address address_type,
)

CREATE TYPE address_type AS (
    address_id UUID,
    unit_number VARCHAR,
    street_number INTEGER,
    address_line1 VARCHAR,
    address_line2 VARCHAR,
    city VARCHAR,
    postal_code VARCHAR,
    country VARCHAR
);

CREATE TYPE review_type AS (
    review_id UUID,
    user_id UUID,
    product_id UUID,
    review_date TIMESTAMP,
    review_text VARCHAR,
    rating INTEGER
);

-- Creating address table
CREATE TABLE IF NOT EXISTS public.address
(
    address_id UUID NOT NULL DEFAULT gen_random_uuid(),
    unit_number integer NOT NULL,
    street_number character varying(300) NOT NULL,
    address_line1 character varying(300) NOT NULL,
    address_line2 character varying(250),
    city character varying(250) NOT NULL,
    postal_code character varying(20) NOT NULL,  -- character varying for flexibility
    country character varying(250) NOT NULL,

    CONSTRAINT address_pkey PRIMARY KEY (address_id)
)
TABLESPACE pg_default;
ALTER TABLE IF EXISTS public.address OWNER TO postgres;


-- Creating user_address table

CREATE TABLE IF NOT EXISTS public.user_address
(
    user_id UUID,
    address_id UUID,
    is_default boolean,

    CONSTRAINT user_address_pkey PRIMARY KEY (user_id, address_id),
    CONSTRAINT user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id) ON DELETE CASCADE,
    CONSTRAINT address_id_fkey FOREIGN KEY (address_id) REFERENCES public.address(address_id) ON DELETE CASCADE,
    CONSTRAINT user_default_address_unique UNIQUE (user_id, is_default)

)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.user_address OWNER TO postgres;

-- Creating users table

CREATE TABLE IF NOT EXISTS public.users
(
    user_id UUID NOT NULL DEFAULT gen_random_uuid(),
    user_name varchar(100) NOT NULL,
    email character varying(250) NOT NULL UNIQUE,
    password varchar(255) NOT NULL,
    salt VARCHAR NOT NULL,
    phone_number varchar(15), -- it may contain non-numeric characters like '+'
    role role_type,
    CONSTRAINT users_pkey PRIMARY KEY (user_id)
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.users OWNER TO postgres;


-- Creating products table

CREATE TABLE IF NOT EXISTS public.products
(
	product_id UUID NOT NULL DEFAULT gen_random_uuid(),
	title character varying(255) COLLATE pg_catalog."default" NOT NULL,
	category_id UUID NOT NULL,
	description varchar(250),
	price numeric(10,2) NOT NULL,
	stock integer NOT NULL,
	product_line varchar(250),
	CONSTRAINT products_pkey PRIMARY KEY (product_id),
	CONSTRAINT products_price_check CHECK (price > 0::numeric) NOT VALID,
	CONSTRAINT products_stock_check CHECK (stock > 0) NOT VALID

)

TABLESPACE pg_default;
ALTER TABLE IF EXISTS public.products OWNER TO postgres;

-- Creating product_images table

CREATE TABLE IF NOT EXISTS public.product_images
(
    image_id UUID NOT NULL DEFAULT gen_random_uuid(),
    product_id UUID,
    image_url character varying(500) NOT NULL,
    is_primary boolean NOT NULL,
    image_text character varying(255) NOT NULL,

    CONSTRAINT product_images_pkey PRIMARY KEY (image_id),
    CONSTRAINT product_id_fkey FOREIGN KEY (product_id) REFERENCES public.products(product_id) ON DELETE SET NULL,
    CONSTRAINT unique_primary_image_per_product UNIQUE (product_id, is_primary) WHERE is_primary = true

)
TABLESPACE pg_default;
ALTER TABLE IF EXISTS public.product_images OWNER TO postgres;

-- Creating product_colors table
CREATE TABLE IF NOT EXISTS public.product_colors
(
    color_id UUID NOT NULL DEFAULT gen_random_uuid(),
    product_id UUID,
    color_name character varying(100) NOT NULL UNIQUE,

    CONSTRAINT color_pkey PRIMARY KEY (color_id),
    CONSTRAINT product_id_fkey FOREIGN KEY (product_id) REFERENCES public.products(product_id) ON DELETE SET NULL,
    CONSTRAINT primary_image_per_product UNIQUE (product_id, is_primary) WHERE is_primary = true --only one primary image per product.

)
TABLESPACE pg_default;
ALTER TABLE IF EXISTS public.product_colors OWNER TO postgres;

-- Creating product_sizes table
CREATE TABLE IF NOT EXISTS public.product_sizes
(
    size_id UUID NOT NULL DEFAULT gen_random_uuid(),
    product_id UUID,
    size_value character varying(100) NOT NULL UNIQUE,

    CONSTRAINT size_pkey PRIMARY KEY (size_id),
    CONSTRAINT product_id_fkey FOREIGN KEY (product_id) REFERENCES public.products(product_id) ON DELETE SET NULL
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.product_sizes OWNER TO postgres;

-- Creating categories table

CREATE TABLE IF NOT EXISTS public.categories
(
    category_id UUID NOT NULL DEFAULT gen_random_uuid(),
    category_name character varying(255),
    parent_category_id UUID,

    CONSTRAINT category_pkey PRIMARY KEY (category_id),
    CONSTRAINT parent_category_id_fkey FOREIGN KEY (parent_category_id) REFERENCES public.categories(category_id) ON DELETE SET NULL
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.categories OWNER TO postgres;

-- Creating orders table

CREATE TABLE IF NOT EXISTS PUBLIC.orders
(
	order_id UUID NOT NULL DEFAULT gen_random_uuid(),
	user_id UUID,
	order_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
	total_price numeric(10,2) NOT NULL,
	shipping_address_id UUID,
	order_status order_status DEFAULT 'pending',
	CONSTRAINT orders_pkey PRIMARY KEY (order_id),
	CONSTRAINT user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id) MATCH SIMPLE
		ON UPDATE NO ACTION
		ON DELETE NO ACTION,
    CONSTRAINT shipping_address_fkey FOREIGN KEY (shipping_address_id) REFERENCES public.address(address_id) 
        ON DELETE SET NULL

)

TABLESPACE pg_default;
ALTER TABLE IF EXISTS public.orders OWNER TO postgres;

-- Creating order_items table

CREATE TABLE IF NOT EXISTS public.order_items
(
    order_item_id UUID NOT NULL DEFAULT gen_random_uuid(),
    order_id UUID,
    quantity INTEGER NOT NULL CHECK (quantity > 0),
    price numeric(10,2) NOT NULL, 
    product_id UUID,  

    CONSTRAINT order_item_pkey PRIMARY KEY (order_item_id),
    CONSTRAINT order_id_fkey FOREIGN KEY (order_id) REFERENCES public.orders(order_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT product_id_fkey FOREIGN KEY (product_id) REFERENCES public.products(product_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.order_items OWNER TO postgres;

-- Creating shipments table
CREATE TABLE IF NOT EXISTS public.shipments
(
    shipment_id UUID NOT NULL DEFAULT gen_random_uuid(),
    order_id UUID,
    shipment_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    address_id UUID,

    CONSTRAINT shipments_pkey PRIMARY KEY (shipment_id),
    CONSTRAINT order_id_fkey FOREIGN KEY (order_id) REFERENCES public.orders(order_id) ON DELETE CASCADE,
    CONSTRAINT address_id_fkey FOREIGN KEY (address_id) REFERENCES public.address(address_id) ON DELETE CASCADE
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.shipments OWNER TO postgres;

-- Creating payments table
CREATE TABLE IF NOT EXISTS public.payments
(
    payment_id UUID NOT NULL DEFAULT gen_random_uuid(),
    user_id UUID,
    order_id UUID NOT NULL,
    payment_date timestamp DEFAULT CURRENT_TIMESTAMP,
    payment_method_id UUID, 
    amount numeric(10,2) NOT NULL CHECK (amount >= 0),


    CONSTRAINT payments_pkey PRIMARY KEY (payment_id),
    CONSTRAINT user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT order_id_fkey FOREIGN KEY (order_id) REFERENCES public.orders(order_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT payment_method_id_fkey FOREIGN KEY (payment_method_id) REFERENCES public.payment_methods(payment_method_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.payments OWNER TO postgres;


-- Creating payment_methods table

CREATE TABLE IF NOT EXISTS public.d
(
    payment_method_id UUID NOT NULL DEFAULT gen_random_uuid(),
    user_id UUID,
    payment_type character varying(255) NOT NULL,
    provider character varying(255) NOT NULL,
    card_number character varying(20) NOT NULL,
    expiry_date date NOT NULL,
    is_default boolean DEFAULT FALSE,

    CONSTRAINT payment_methods_pkey PRIMARY KEY (payment_method_id),
    CONSTRAINT user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.payment_methods OWNER TO postgres;

-- Creating reviews table

CREATE TABLE IF NOT EXISTS public.reviews
(
    review_id UUID NOT NULL DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL,
    product_id UUID NOT NULL,
    review_date timestamp DEFAULT CURRENT_TIMESTAMP,
    review_text character varying(500),
    rating integer NOT NULL CHECK(rating > 0 AND rating < 6),  -- Fixed typo in CHECK constraint

    CONSTRAINT reviews_pkey PRIMARY KEY (review_id),
    CONSTRAINT user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT product_id_fkey FOREIGN KEY (product_id) REFERENCES public.products(product_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.reviews OWNER TO postgres;
