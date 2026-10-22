-- 2. Постоянные таблицы из очищенных данных

CREATE TABLE regions AS
SELECT * FROM v_regions_typed;

CREATE TABLE categories AS
SELECT * FROM v_categories_typed;

CREATE TABLE sellers AS
SELECT * FROM v_sellers_typed;

CREATE TABLE customers AS
SELECT * FROM v_customers_typed;

CREATE TABLE products AS
SELECT * FROM v_products_typed;

CREATE TABLE orders AS
SELECT * FROM v_orders_typed;

CREATE TABLE order_items AS
SELECT * FROM v_order_items_typed;


-- 3. Установка PRIMARY KEYS
ALTER TABLE regions ADD CONSTRAINT pk_regions PRIMARY KEY (region_id);
ALTER TABLE categories ADD CONSTRAINT pk_categories PRIMARY KEY (category_id);
ALTER TABLE sellers ADD CONSTRAINT pk_sellers PRIMARY KEY (seller_id);
ALTER TABLE customers ADD CONSTRAINT pk_customers PRIMARY KEY (customer_id);
ALTER TABLE products ADD CONSTRAINT pk_products PRIMARY KEY (product_id);
ALTER TABLE orders ADD CONSTRAINT pk_orders PRIMARY KEY (order_id);
ALTER TABLE order_items ADD CONSTRAINT pk_order_items PRIMARY KEY (order_item_id);


-- 4. Установка FOREIGN KEYS
-- Sellers -> Regions
ALTER TABLE sellers
ADD CONSTRAINT fk_sellers_region
FOREIGN KEY (region_id) REFERENCES regions(region_id)
ON DELETE SET NULL;

-- Customers -> Regions
ALTER TABLE customers
ADD CONSTRAINT fk_customers_region
FOREIGN KEY (region_id) REFERENCES regions(region_id)
ON DELETE SET NULL;

-- Products -> Sellers
ALTER TABLE products
ADD CONSTRAINT fk_products_seller
FOREIGN KEY (seller_id) REFERENCES sellers(seller_id)
ON DELETE CASCADE; 

-- Products -> Categories
ALTER TABLE products
ADD CONSTRAINT fk_products_category
FOREIGN KEY (category_id) REFERENCES categories(category_id)
ON DELETE SET NULL;

-- Orders -> Customers
ALTER TABLE orders
ADD CONSTRAINT fk_orders_customer
FOREIGN KEY (customer_id) REFERENCES customers(customer_id)
ON DELETE SET NULL; 

-- Order Items -> Orders
ALTER TABLE order_items
ADD CONSTRAINT fk_order_items_order
FOREIGN KEY (order_id) REFERENCES orders(order_id)
ON DELETE CASCADE; -- Позиции удаляются вместе с заказом

-- Order Items -> Products
ALTER TABLE order_items
ADD CONSTRAINT fk_order_items_product
FOREIGN KEY (product_id) REFERENCES products(product_id)
ON DELETE RESTRICT; 


-- 5. Проверка
SELECT 'regions' AS tbl, COUNT(*) AS row_count FROM regions UNION ALL
SELECT 'categories', COUNT(*) FROM categories UNION ALL
SELECT 'sellers', COUNT(*) FROM sellers UNION ALL
SELECT 'customers', COUNT(*) FROM customers UNION ALL
SELECT 'products', COUNT(*) FROM products UNION ALL
SELECT 'orders', COUNT(*) FROM orders UNION ALL
SELECT 'order_items', COUNT(*) FROM order_items;

SELECT column_name, data_type 
FROM information_schema.columns 
WHERE table_name = 'orders'
ORDER BY ordinal_position;

SELECT 'Orphaned sellers' AS check_type, COUNT(*) 
FROM sellers s LEFT JOIN regions r ON s.region_id = r.region_id WHERE s.region_id IS NOT NULL AND r.region_id IS NULL
UNION ALL
SELECT 'Orphaned customers', COUNT(*) 
FROM customers c LEFT JOIN regions r ON c.region_id = r.region_id WHERE c.region_id IS NOT NULL AND r.region_id IS NULL
UNION ALL
SELECT 'Orphaned products (seller)', COUNT(*) 
FROM products p LEFT JOIN sellers s ON p.seller_id = s.seller_id WHERE p.seller_id IS NOT NULL AND s.seller_id IS NULL
UNION ALL
SELECT 'Orphaned products (cat)', COUNT(*) 
FROM products p LEFT JOIN categories c ON p.category_id = c.category_id WHERE p.category_id IS NOT NULL AND c.category_id IS NULL
UNION ALL
SELECT 'Orphaned orders', COUNT(*) 
FROM orders o LEFT JOIN customers c ON o.customer_id = c.customer_id WHERE o.customer_id IS NOT NULL AND c.customer_id IS NULL
UNION ALL
SELECT 'Orphaned items (order)', COUNT(*) 
FROM order_items oi LEFT JOIN orders o ON oi.order_id = o.order_id WHERE oi.order_id IS NOT NULL AND o.order_id IS NULL
UNION ALL
SELECT 'Orphaned items (prod)', COUNT(*) 
FROM order_items oi LEFT JOIN products p ON oi.product_id = p.product_id WHERE oi.product_id IS NOT NULL AND p.product_id IS NULL;