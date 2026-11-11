-- Вариант 01. Маркетплейс. PostgreSQL.
SET search_path TO variant_01_marketplace, public;

-- Запрос 01
SELECT order_id, customer_id, ordered_at FROM orders WHERE ordered_at IS NOT NULL
ORDER BY ordered_at DESC NULLS LAST, order_id DESC LIMIT 20;

-- Исправлено
CREATE INDEX IF NOT EXISTS idx_orders_ordered_at_desc 
ON orders (ordered_at DESC NULLS LAST, order_id DESC);

EXPLAIN (ANALYZE, BUFFERS, FORMAT TEXT)
SELECT order_id, customer_id, ordered_at FROM orders WHERE ordered_at IS NOT NULL
ORDER BY ordered_at DESC NULLS LAST, order_id DESC LIMIT 20;


-- Запрос 02
SELECT d.customer_id,
  (SELECT COUNT(*) FROM orders f WHERE f.customer_id = d.customer_id
    AND f.ordered_at >= TIMESTAMP '2026-07-01' AND f.ordered_at < TIMESTAMP '2026-08-01') AS month_1,
  (SELECT COUNT(*) FROM orders f WHERE f.customer_id = d.customer_id
    AND f.ordered_at >= TIMESTAMP '2026-08-01' AND f.ordered_at < TIMESTAMP '2026-09-01') AS month_2
FROM customers d;

-- Исправлено
CREATE INDEX IF NOT EXISTS idx_orders_customer_date 
ON orders (customer_id, ordered_at);

EXPLAIN (ANALYZE, BUFFERS, FORMAT TEXT)
SELECT d.customer_id,
  COUNT(*) FILTER (WHERE o.ordered_at >= TIMESTAMP '2026-07-01' AND o.ordered_at < TIMESTAMP '2026-08-01') AS month_1,
  COUNT(*) FILTER (WHERE o.ordered_at >= TIMESTAMP '2026-08-01' AND o.ordered_at < TIMESTAMP '2026-09-01') AS month_2
FROM customers d
LEFT JOIN orders o ON o.customer_id = d.customer_id
GROUP BY d.customer_id;

-- Запрос 03
SELECT customer_id, COUNT(*) AS n FROM orders
GROUP BY customer_id ORDER BY n DESC, customer_id ASC NULLS LAST LIMIT 10;

-- Запрос 04
SELECT DISTINCT order_id FROM orders WHERE ordered_at >= TIMESTAMP '2026-07-01' AND ordered_at < TIMESTAMP '2026-08-01';

-- Запрос 05
SELECT order_id, customer_id, ordered_at FROM orders WHERE ordered_at < TIMESTAMP '2026-08-01'
UNION
SELECT order_id, customer_id, ordered_at FROM orders WHERE ordered_at >= TIMESTAMP '2026-08-01';

-- Запрос 06
SELECT order_id, ordered_at FROM orders
WHERE lower(btrim(status)) = 'returned'
 AND ordered_at >= TIMESTAMP '2026-07-01' AND ordered_at < TIMESTAMP '2026-08-01';

-- Запрос 07
SELECT customer_id, customer_name FROM customers;

-- Запрос 08
SELECT order_id, customer_id, ordered_at FROM orders WHERE order_id = 5001;

-- Запрос 09
SELECT d.customer_id,
  (SELECT COUNT(*) FROM orders f WHERE f.customer_id = d.customer_id) AS n,
  (SELECT MIN(ordered_at) FROM orders f WHERE f.customer_id = d.customer_id) AS first_at,
  (SELECT MAX(ordered_at) FROM orders f WHERE f.customer_id = d.customer_id) AS last_at
FROM customers d;

-- Запрос 10
WITH all_facts AS MATERIALIZED (SELECT order_id, customer_id, ordered_at FROM orders)
SELECT order_id, customer_id, ordered_at FROM all_facts WHERE customer_id = 135;

-- Запрос 11
SELECT o.order_id, (SELECT SUM(i.quantity::numeric * i.unit_price)
 FROM order_items i WHERE i.order_id = o.order_id) AS amount FROM orders o;

-- Запрос 12
SELECT order_id, customer_id, ordered_at FROM orders WHERE order_id::text = '5001';

-- Запрос 13
SELECT COUNT(*) AS total FROM orders;

-- Запрос 14
SELECT order_id, customer_id, ordered_at FROM orders
WHERE customer_id = 135 AND ordered_at >= TIMESTAMP '2026-07-01' AND ordered_at < TIMESTAMP '2026-08-01'
ORDER BY ordered_at, order_id;

-- Запрос 15
SELECT f.order_id, f.ordered_at,
  (SELECT COUNT(*) FROM orders x WHERE x.customer_id = f.customer_id
   AND (x.ordered_at, x.order_id) <= (f.ordered_at, f.order_id)) AS seq
FROM orders f WHERE f.customer_id = 135 AND f.ordered_at IS NOT NULL;

-- Запрос 16
SELECT DISTINCT d.customer_id, d.customer_name
FROM customers d JOIN orders f ON f.customer_id = d.customer_id
WHERE f.ordered_at >= TIMESTAMP '2026-07-01' AND f.ordered_at < TIMESTAMP '2026-08-01';

-- Запрос 17
SELECT customer_id, COUNT(*) AS n FROM orders GROUP BY customer_id;

-- Запрос 18
SELECT order_id, customer_id, ordered_at FROM orders
WHERE date_trunc('month', ordered_at) = TIMESTAMP '2026-07-01';

-- Запрос 19
WITH ranked AS (
 SELECT customer_id, order_id, ordered_at, ROW_NUMBER() OVER (
  PARTITION BY customer_id ORDER BY ordered_at DESC NULLS LAST, order_id DESC) AS rn
 FROM orders
) SELECT customer_id, order_id, ordered_at FROM ranked WHERE rn=1 ORDER BY customer_id;

-- Запрос 20
SELECT d.customer_id,
  (SELECT COUNT(*) FROM orders f WHERE f.customer_id = d.customer_id) AS fact_count
FROM customers d;

