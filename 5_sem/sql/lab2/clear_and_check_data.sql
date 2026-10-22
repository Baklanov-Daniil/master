---  1: RAW DATA CHECK
SELECT 'regions' AS tbl, COUNT(*) FROM raw_regions UNION ALL
SELECT 'categories', COUNT(*) FROM raw_categories UNION ALL
SELECT 'sellers', COUNT(*) FROM raw_sellers UNION ALL
SELECT 'customers', COUNT(*) FROM raw_customers UNION ALL
SELECT 'products', COUNT(*) FROM raw_products UNION ALL
SELECT 'orders', COUNT(*) FROM raw_orders UNION ALL
SELECT 'order_items', COUNT(*) FROM raw_order_items;

---  2: REGIONS (Очистка и Типизация)
CREATE TEMP VIEW v_regions_normalized AS
SELECT 
    region_id,
    TRIM(region_name) AS region_name
FROM raw_regions;

CREATE TEMP VIEW v_regions_typed AS
SELECT 
    region_id::INTEGER AS region_id,
    region_name
FROM v_regions_normalized;

-- Проверки Regions
SELECT 'Duplicated region_id' AS check_type, COUNT(*) AS bad_rows_count
FROM (
    SELECT region_id
    FROM v_regions_typed
    GROUP BY region_id
    HAVING COUNT(*) > 1
) duplicates;

SELECT 'Missing region_name' AS check_type, COUNT(*) AS null_names_count
FROM v_regions_typed
WHERE region_name IS NULL;

SELECT 'Total rows in v_regions_typed' AS info, COUNT(*) AS total_count
FROM v_regions_typed;

---  3: CATEGORIES (Очистка и Типизация)
CREATE TEMP VIEW v_categories_normalized AS
SELECT 
    category_id,
    TRIM(category_name) AS category_name
FROM raw_categories;

CREATE TEMP VIEW v_categories_typed AS
SELECT 
    category_id::INTEGER AS category_id,
    category_name
FROM v_categories_normalized;

-- Проверки Categories
SELECT 'Duplicated category_id' AS check_type, COUNT(*) AS bad_rows_count
FROM (
    SELECT category_id
    FROM v_categories_typed
    GROUP BY category_id
    HAVING COUNT(*) > 1
) duplicates;

SELECT 'Missing category_name' AS check_type, COUNT(*) AS null_names_count
FROM v_categories_typed
WHERE category_name IS NULL;

SELECT 'Total rows in v_categories_typed' AS info, COUNT(*) AS total_count
FROM v_categories_typed;

---  4: SELLERS (Очистка и Типизация)
CREATE TEMP VIEW v_sellers_normalized AS
SELECT 
    seller_id,
    TRIM(seller_name) AS seller_name,
    region_id
FROM raw_sellers;

CREATE TEMP VIEW v_sellers_typed AS
SELECT 
    seller_id::INTEGER AS seller_id,
    seller_name,
    CASE 
        WHEN region_id IS NULL OR TRIM(region_id) = '' THEN NULL
        ELSE region_id::INTEGER           
    END AS region_id
FROM v_sellers_normalized;

-- Проверки Sellers
SELECT 'Duplicated seller_id' AS check_type, COUNT(*) AS bad_rows_count
FROM (
    SELECT seller_id
    FROM v_sellers_typed
    GROUP BY seller_id
    HAVING COUNT(*) > 1
) duplicates;

SELECT 'Missing seller_name' AS check_type, COUNT(*) AS null_names_count
FROM v_sellers_typed
WHERE seller_name IS NULL OR TRIM(seller_name) = '';

SELECT 'Invalid region_ref' AS check_type, COUNT(*) AS orphaned_records
FROM v_sellers_typed s
LEFT JOIN v_regions_typed r ON s.region_id = r.region_id
WHERE s.region_id IS NOT NULL AND r.region_id IS NULL;

SELECT 'Total rows in v_sellers_typed' AS info, COUNT(*) AS total_count
FROM v_sellers_typed;

---  5: CUSTOMERS (Очистка, Типизация + Разделение даты)
CREATE TEMP VIEW v_customers_normalized AS
SELECT 
    customer_id,
    TRIM(customer_name) AS customer_name,
    region_id,
    registered_at
FROM raw_customers;

CREATE TEMP VIEW v_customers_typed AS
SELECT 
    customer_id::INTEGER AS customer_id,
    TRIM(customer_name) AS customer_name,
    CASE 
        WHEN region_id IS NULL OR TRIM(region_id) = '' THEN NULL
        ELSE region_id::INTEGER           
    END AS region_id,
    registered_at::TIMESTAMP AS registered_at_ts,
    registered_at::DATE AS registration_date,  
    registered_at::TIME AS registration_time 
FROM v_customers_normalized;

-- Проверки Customers
SELECT 'Duplicated customer_id' AS check_type, COUNT(*) AS bad_rows_count
FROM (
    SELECT customer_id
    FROM v_customers_typed
    GROUP BY customer_id
    HAVING COUNT(*) > 1
) duplicates;

SELECT 'Missing customer_name' AS check_type, COUNT(*) AS null_names_count
FROM v_customers_typed
WHERE customer_name IS NULL OR TRIM(customer_name) = '';

SELECT 'Invalid region_ref' AS check_type, COUNT(*) AS orphaned_records
FROM v_customers_typed c
LEFT JOIN v_regions_typed r ON c.region_id = r.region_id
WHERE c.region_id IS NOT NULL AND r.region_id IS NULL;

SELECT 'Min registered_at' AS check_type, MIN(registered_at_ts)::TEXT AS value FROM v_customers_typed
UNION ALL
SELECT 'Max registered_at', MAX(registered_at_ts)::TEXT FROM v_customers_typed
UNION ALL
SELECT 'Count with NULL date', COUNT(*)::TEXT FROM v_customers_typed WHERE registered_at_ts IS NULL;

SELECT 'Total rows in v_customers_typed' AS info, COUNT(*) AS total_count
FROM v_customers_typed;

---  6: PRODUCTS (Очистка и Типизация)
CREATE TEMP VIEW v_products_normalized AS
SELECT 
    product_id,
    seller_id,
    category_id,
    TRIM(product_name) AS product_name,
    current_price
FROM raw_products;

CREATE TEMP VIEW v_products_typed AS
SELECT 
    product_id::INTEGER AS product_id,
    seller_id::INTEGER AS seller_id,
    category_id::INTEGER AS category_id,
    TRIM(product_name) AS product_name,
    current_price::NUMERIC(10, 2) AS price
FROM v_products_normalized;

-- Проверки Products
SELECT 'Duplicated product_id' AS check_type, COUNT(*) AS bad_rows_count
FROM (
    SELECT product_id
    FROM v_products_typed
    GROUP BY product_id
    HAVING COUNT(*) > 1
) duplicates;

SELECT 'Missing product_name' AS check_type, COUNT(*) AS null_names_count
FROM v_products_typed
WHERE product_name IS NULL OR TRIM(product_name) = '';

SELECT 'Invalid seller_ref' AS check_type, COUNT(*) AS orphaned_records
FROM v_products_typed p
LEFT JOIN v_sellers_typed s ON p.seller_id = s.seller_id
WHERE p.seller_id IS NOT NULL AND s.seller_id IS NULL;

SELECT 'Invalid category_ref' AS check_type, COUNT(*) AS orphaned_records
FROM v_products_typed p
LEFT JOIN v_categories_typed c ON p.category_id = c.category_id
WHERE p.category_id IS NOT NULL AND c.category_id IS NULL;

SELECT 'Min Price' AS info, MIN(price)::TEXT FROM v_products_typed
UNION ALL
SELECT 'Max Price', MAX(price)::TEXT FROM v_products_typed
UNION ALL
SELECT 'Total rows in v_products_typed', COUNT(*)::TEXT FROM v_products_typed;

---  7: ORDERS 
CREATE TEMP VIEW v_orders_normalized AS
SELECT 
    order_id,
    CASE 
        WHEN customer_id IS NULL OR TRIM(customer_id) = '' THEN NULL
        ELSE customer_id                     
    END AS customer_id,
    ordered_at,
    LOWER(TRIM(status)) AS status,           
    LOWER(TRIM(sales_channel)) AS sales_channel 
FROM raw_orders;

CREATE TEMP VIEW v_orders_typed AS
SELECT 
    order_id::INTEGER AS order_id,
    CASE 
        WHEN customer_id IS NULL OR TRIM(customer_id) = '' THEN NULL
        ELSE customer_id::INTEGER           
    END AS customer_id,
    ordered_at::TIMESTAMP AS ordered_at_ts, 
    ordered_at::DATE AS order_date,         
    ordered_at::TIME AS order_time,         
    status,                                 
    sales_channel                           
FROM v_orders_normalized;

-- Проверки Orders
SELECT 'Duplicated order_id' AS check_type, COUNT(*) AS bad_rows_count
FROM (
    SELECT order_id
    FROM v_orders_typed
    GROUP BY order_id
    HAVING COUNT(*) > 1
) duplicates;

SELECT 'Missing ordered_at' AS check_type, COUNT(*) AS null_dates_count
FROM v_orders_typed
WHERE ordered_at_ts IS NULL;

SELECT 'Invalid customer_ref' AS check_type, COUNT(*) AS orphaned_records
FROM v_orders_typed o
LEFT JOIN v_customers_typed c ON o.customer_id = c.customer_id
WHERE o.customer_id IS NOT NULL AND c.customer_id IS NULL;

-- Анализ статусов 
SELECT status, COUNT(*) as cnt
FROM v_orders_typed
GROUP BY status
ORDER BY cnt DESC;

SELECT 'Total rows in v_orders_typed' AS info, COUNT(*) AS total_count
FROM v_orders_typed;

---  8: ORDER ITEMS 
CREATE TEMP VIEW v_order_items_normalized AS
SELECT 
    order_item_id,
    order_id,
    product_id,
    quantity,
    unit_price
FROM raw_order_items;

CREATE TEMP VIEW v_order_items_typed AS
SELECT 
    order_item_id::INTEGER AS order_item_id,
    order_id::INTEGER AS order_id,
    product_id::INTEGER AS product_id,
    quantity::INTEGER AS quantity,
    unit_price::NUMERIC(10, 2) AS unit_price,
    (quantity::INTEGER * unit_price::NUMERIC(10, 2)) AS line_total
FROM v_order_items_normalized;

-- Проверки Order Items
SELECT 'Duplicated order_item_id' AS check_type, COUNT(*) AS bad_rows_count
FROM (
    SELECT order_item_id
    FROM v_order_items_typed
    GROUP BY order_item_id
    HAVING COUNT(*) > 1
) duplicates;

SELECT 'Invalid order_ref' AS check_type, COUNT(*) AS orphaned_records
FROM v_order_items_typed oi
LEFT JOIN v_orders_typed o ON oi.order_id = o.order_id
WHERE oi.order_id IS NOT NULL AND o.order_id IS NULL;

SELECT 'Invalid product_ref' AS check_type, COUNT(*) AS orphaned_records
FROM v_order_items_typed oi
LEFT JOIN v_products_typed p ON oi.product_id = p.product_id
WHERE oi.product_id IS NOT NULL AND p.product_id IS NULL;

SELECT 'Suspicious records (qty<=0 or price<0)' AS check_type, COUNT(*) AS suspicious_count
FROM v_order_items_typed
WHERE quantity <= 0 OR unit_price < 0;