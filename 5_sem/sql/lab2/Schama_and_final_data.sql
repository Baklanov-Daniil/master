DROP TABLE IF EXISTS order_items CASCADE;
DROP TABLE IF EXISTS orders CASCADE;
DROP TABLE IF EXISTS products CASCADE;
DROP TABLE IF EXISTS customers CASCADE;
DROP TABLE IF EXISTS sellers CASCADE;
DROP TABLE IF EXISTS categories CASCADE;
DROP TABLE IF EXISTS regions CASCADE;

CREATE TABLE regions AS
SELECT * FROM v_regions_typed;

CREATE TABLE categories AS
SELECT * FROM v_categories_typed;

CREATE TABLE sellers AS
SELECT * FROM v_sellers_typed;

CREATE TABLE customers AS
SELECT * FROM v_customers_typed

CREATE TABLE products AS
SELECT * FROM v_products_typed

CREATE TABLE orders AS
SELECT * FROM v_orders_typed

CREATE TABLE order_items AS
SELECT * FROM v_order_items_typed

SELECT 'regions' AS tbl, COUNT(*) FROM regions UNION ALL
SELECT 'categories', COUNT(*) FROM categories UNION ALL
SELECT 'sellers', COUNT(*) FROM sellers UNION ALL
SELECT 'customers', COUNT(*) FROM customers UNION ALL
SELECT 'products', COUNT(*) FROM products UNION ALL
SELECT 'orders', COUNT(*) FROM orders UNION ALL
SELECT 'order_items', COUNT(*) FROM order_items;

SELECT column_name, data_type 
FROM information_schema.columns 
WHERE table_name = 'orders';

ALTER TABLE regions 
ADD CONSTRAINT pk_regions PRIMARY KEY (region_id);

ALTER TABLE categories 
ADD CONSTRAINT pk_categories PRIMARY KEY (category_id);

ALTER TABLE sellers 
ADD CONSTRAINT pk_sellers PRIMARY KEY (seller_id);

ALTER TABLE customers 
ADD CONSTRAINT pk_customers PRIMARY KEY (customer_id);

ALTER TABLE products 
ADD CONSTRAINT pk_products PRIMARY KEY (product_id);

ALTER TABLE orders 
ADD CONSTRAINT pk_orders PRIMARY KEY (order_id);

ALTER TABLE order_items 
ADD CONSTRAINT pk_order_items PRIMARY KEY (order_item_id);


ALTER TABLE sellers
ADD CONSTRAINT fk_sellers_region
FOREIGN KEY (region_id) REFERENCES regions(region_id)
ON DELETE SET NULL;

ALTER TABLE customers
ADD CONSTRAINT fk_customers_region
FOREIGN KEY (region_id) REFERENCES regions(region_id)
ON DELETE SET NULL;

ALTER TABLE products
ADD CONSTRAINT fk_products_seller
FOREIGN KEY (seller_id) REFERENCES sellers(seller_id)
ON DELETE CASCADE;

ALTER TABLE products
ADD CONSTRAINT fk_products_category
FOREIGN KEY (category_id) REFERENCES categories(category_id)

ALTER TABLE orders
ADD CONSTRAINT fk_orders_customer
FOREIGN KEY (customer_id) REFERENCES customers(customer_id)
ON DELETE SET NULL;

ALTER TABLE order_items
ADD CONSTRAINT fk_order_items_order
FOREIGN KEY (order_id) REFERENCES orders(order_id)
ON DELETE CASCADE;

ALTER TABLE order_items
ADD CONSTRAINT fk_order_items_product
FOREIGN KEY (product_id) REFERENCES products(product_id)
ON DELETE RESTRICT;