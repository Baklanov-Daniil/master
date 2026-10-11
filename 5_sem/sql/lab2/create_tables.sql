CREATE TEMP TABLE raw_regions (
    region_id TEXT,
    region_name TEXT
);

CREATE TEMP TABLE raw_categories (
    category_id TEXT,
    category_name TEXT
);

CREATE TEMP TABLE raw_sellers (
    seller_id TEXT,
    seller_name TEXT,
    region_id TEXT
);

CREATE TEMP TABLE raw_customers (
    customer_id TEXT,
    customer_name TEXT,
    region_id TEXT,
    registered_at TEXT
);

CREATE TEMP TABLE raw_products (
    product_id TEXT,
    seller_id TEXT,
    category_id TEXT,
    product_name TEXT,
    current_price TEXT
);

CREATE TEMP TABLE raw_orders (
    order_id TEXT,
    customer_id TEXT,
    ordered_at TEXT,
    status TEXT,
    sales_channel TEXT
);

CREATE TEMP TABLE raw_order_items (
    order_item_id TEXT,
    order_id TEXT,
    product_id TEXT,
    quantity TEXT,
    unit_price TEXT
);