COPY raw_regions
FROM 'C:\Users\sex_mashina\Desktop\git\master\5_sem\sql\lab2\regions.txt'
WITH (FORMAT csv, DELIMITER E'\t', HEADER true, NULL '', ENCODING 'UTF8');

COPY raw_categories
FROM 'C:\Users\sex_mashina\Desktop\git\master\5_sem\sql\lab2\categories.txt'
WITH (FORMAT csv, DELIMITER E'\t', HEADER true, NULL '', ENCODING 'UTF8');

COPY raw_sellers
FROM 'C:\Users\sex_mashina\Desktop\git\master\5_sem\sql\lab2\sellers.txt'
WITH (FORMAT csv, DELIMITER E'\t', HEADER true, NULL '', ENCODING 'UTF8');

COPY raw_customers
FROM 'C:\Users\sex_mashina\Desktop\git\master\5_sem\sql\lab2\customers.txt'
WITH (FORMAT csv, DELIMITER E'\t', HEADER true, NULL '', ENCODING 'UTF8');

COPY raw_products
FROM 'C:\Users\sex_mashina\Desktop\git\master\5_sem\sql\lab2\products.txt'
WITH (FORMAT csv, DELIMITER E'\t', HEADER true, NULL '', ENCODING 'UTF8');

COPY raw_orders
FROM 'C:\Users\sex_mashina\Desktop\git\master\5_sem\sql\lab2\orders.txt'
WITH (FORMAT csv, DELIMITER E'\t', HEADER true, NULL '', ENCODING 'UTF8');

COPY raw_order_items
FROM 'C:\Users\sex_mashina\Desktop\git\master\5_sem\sql\lab2\order_items.txt'
WITH (FORMAT csv, DELIMITER E'\t', HEADER true, NULL '', ENCODING 'UTF8');