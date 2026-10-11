# 01 Marketplace

Seed: 20260931
Format: UTF-8 TSV with a header; empty cell means SQL NULL unless the field is descriptive text.
Load into PostgreSQL with COPY ... WITH (FORMAT csv, DELIMITER E'\t', HEADER true, NULL '').
Identifiers and foreign keys are valid. Noise affects descriptions, statuses, optional values and occasional measurements.
For chronological window calculations use the timestamp and then the row ID as tie-breaker.

## Tables

- `regions.txt`: 12 rows; PK `region_id`; columns: region_id, region_name
- `categories.txt`: 20 rows; PK `category_id`; columns: category_id, category_name
- `sellers.txt`: 150 rows; PK `seller_id`; columns: seller_id, seller_name, region_id
- `customers.txt`: 180 rows; PK `customer_id`; columns: customer_id, customer_name, region_id, registered_at
- `products.txt`: 180 rows; PK `product_id`; columns: product_id, seller_id, category_id, product_name, current_price
- `orders.txt`: 10,000 rows; PK `order_id`; columns: order_id, customer_id, ordered_at, status, sales_channel
- `order_items.txt`: 13,998 rows; PK `order_item_id`; columns: order_item_id, order_id, product_id, quantity, unit_price

## Relationships

- `sellers.region_id` -> `regions`
- `customers.region_id` -> `regions`
- `products.seller_id` -> `sellers`
- `products.category_id` -> `categories`
- `orders.customer_id` -> `customers` (nullable)
- `order_items.order_id` -> `orders`
- `order_items.product_id` -> `products`

## Intentional imperfections

- About 1–3% of names have casing or whitespace variations.
- Some statuses have uppercase or surrounding spaces; normalize with LOWER(TRIM(...)).
- Optional fields may be empty; selected numerical measures contain rare outliers.
- Equal timestamps occur in some event streams; include IDs in ORDER BY.
- Do not delete repeated-looking business events without a documented deduplication rule.
