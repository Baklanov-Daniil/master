-- R1: Последний заказ каждого зарегистрированного покупателя
WITH ranked_orders AS (
    SELECT 
        *,
        ROW_NUMBER() OVER (
            PARTITION BY o.customer_id 
            ORDER BY o.ordered_at_ts DESC, o.order_id DESC
        ) as rn
    FROM orders o
    WHERE o.customer_id IS NOT NULL
)
SELECT order_id, customer_id, ordered_at_ts, status
FROM ranked_orders
WHERE rn = 1;

-- R2: Рейтинг продавцов по выручке внутри региона (RANK)
WITH seller_revenue AS (
    SELECT 
        s.seller_id,
        s.seller_name,
        s.region_id,
        COALESCE(SUM(oi.line_total), 0) AS total_revenue
    FROM sellers s
    JOIN products p ON s.seller_id = p.seller_id
    JOIN order_items oi ON p.product_id = oi.product_id
    JOIN orders o ON oi.order_id = o.order_id
    WHERE o.status = 'completed'
    GROUP BY s.seller_id, s.seller_name, s.region_id
)
SELECT 
    sr.region_id,
    r.region_name,
    sr.seller_id,
    sr.seller_name,
    sr.total_revenue,
    RANK() OVER (
        PARTITION BY sr.region_id 
        ORDER BY sr.total_revenue DESC
    ) AS revenue_rank
FROM seller_revenue sr
JOIN regions r ON sr.region_id = r.region_id
ORDER BY sr.region_id, revenue_rank;

-- R3: Рейтинг товаров внутри категории (DENSE_RANK)
WITH product_sales AS (
    SELECT 
        p.product_id,
        p.product_name,
        p.category_id,
        c.category_name,
        SUM(oi.quantity) AS units_sold 
    FROM products p  
    JOIN categories c ON p.category_id = c.category_id
    LEFT JOIN order_items oi ON p.product_id = oi.product_id
    GROUP BY p.product_id, p.product_name, p.category_id, c.category_name
)
SELECT 
    ps.category_name,
    ps.product_id,
    ps.product_name,
    ps.units_sold,
    DENSE_RANK() OVER (
        PARTITION BY ps.category_id 
        ORDER BY ps.units_sold DESC
    ) AS sales_dense_rank
FROM product_sales ps
ORDER BY ps.category_name, sales_dense_rank;

-- R4: Относительное положение выручки продавца (PERCENT_RANK) с округлением
WITH seller_revenue AS (
    SELECT 
        s.seller_id,
        s.seller_name,
        s.region_id,
        SUM(oi.line_total) AS total_revenue
    FROM sellers s
    JOIN products p ON s.seller_id = p.seller_id
    JOIN order_items oi ON p.product_id = oi.product_id
    JOIN orders o ON oi.order_id = o.order_id
    WHERE o.status = 'completed'
    GROUP BY s.seller_id, s.seller_name, s.region_id
)
SELECT 
    sr.region_id,
    r.region_name,
    sr.seller_id,
    sr.seller_name,
    sr.total_revenue,
    ROUND(PERCENT_RANK() OVER (
        PARTITION BY sr.region_id 
        ORDER BY sr.total_revenue ASC
    )::NUMERIC, 4) AS revenue_percentile
    
FROM seller_revenue sr
JOIN regions r ON sr.region_id = r.region_id
ORDER BY sr.region_id, revenue_percentile DESC;

-- R5: Доля товаров категории с ценой не выше текущей (CUME_DIST)
SELECT 
    p.product_id,
    p.product_name,
    p.category_id,
    c.category_name,
    p.price,
    CUME_DIST() OVER (
        PARTITION BY p.category_id      
        ORDER BY p.price ASC            
    ) AS price_cumulative_share        
FROM products p
JOIN categories c ON p.category_id = c.category_id
ORDER BY p.category_id, p.price;

-- R6: Квартили покупателей по сумме покупок (NTILE)
WITH customer_totals AS (
    SELECT 
        o.customer_id,
        SUM(oi.line_total) AS total_spent
    FROM orders o
    JOIN order_items oi ON o.order_id = oi.order_id
    WHERE o.status = 'completed' 
      AND o.customer_id IS NOT NULL
    GROUP BY o.customer_id
)
SELECT 
    ct.customer_id,
    c.customer_name,
    ct.total_spent,
    NTILE(4) OVER (
        ORDER BY ct.total_spent DESC
    ) AS spending_quartile
FROM customer_totals ct
JOIN customers c ON ct.customer_id = c.customer_id
ORDER BY spending_quartile, ct.total_spent DESC;

-- N1: Дельта между текущим и предыдущим заказом
WITH order_totals AS (
    SELECT 
        o.order_id,
        o.customer_id,
        o.ordered_at_ts,
        SUM(oi.line_total) AS total_order_amount
    FROM orders o
    JOIN order_items oi ON o.order_id = oi.order_id
    WHERE o.status = 'completed' 
      AND o.customer_id IS NOT NULL
    GROUP BY o.order_id, o.customer_id, o.ordered_at_ts
)
SELECT 
    ot.customer_id,
    c.customer_name,
    ot.order_id,
    ot.ordered_at_ts,
    ot.total_order_amount,
    LAG(ot.total_order_amount) OVER (
        PARTITION BY ot.customer_id 
        ORDER BY ot.ordered_at_ts ASC, ot.order_id ASC
    ) AS prev_order_amount,

    (ot.total_order_amount - LAG(ot.total_order_amount) OVER (
        PARTITION BY ot.customer_id 
        ORDER BY ot.ordered_at_ts ASC, ot.order_id ASC
    )) AS delta_vs_prev
    
FROM order_totals ot
JOIN customers c ON ot.customer_id = c.customer_id
ORDER BY ot.customer_id, ot.ordered_at_ts;

-- N2: Покажите дату следующего заказа и интервал в днях до него.
SELECT 
    o.customer_id,
    c.customer_name,
    o.order_id AS current_order_id,
    o.ordered_at_ts AS current_order_time,
    
    LEAD(o.ordered_at_ts) OVER (
        PARTITION BY o.customer_id 
        ORDER BY o.ordered_at_ts ASC, o.order_id ASC
    ) AS next_order_date,
    
    CASE 
        WHEN LEAD(o.ordered_at_ts) OVER (
            PARTITION BY o.customer_id 
            ORDER BY o.ordered_at_ts ASC, o.order_id ASC
        ) IS NOT NULL THEN
            EXTRACT(DAY FROM (
                LEAD(o.ordered_at_ts) OVER (
                    PARTITION BY o.customer_id 
                    ORDER BY o.ordered_at_ts ASC, o.order_id ASC
                ) - o.ordered_at_ts
            ))
        ELSE NULL
    END AS days_to_next_order
    
FROM orders o
JOIN customers c ON o.customer_id = c.customer_id
WHERE o.status = 'completed' 
  AND o.customer_id IS NOT NULL  
ORDER BY o.customer_id, o.ordered_at_ts;

-- N3: Канал первого заказа
SELECT 
    o.order_id,
    o.customer_id,
    c.customer_name,
    o.ordered_at_ts,
    o.sales_channel AS current_order_channel,
    FIRST_VALUE(o.sales_channel) OVER (
        PARTITION BY o.customer_id 
        ORDER BY o.ordered_at_ts ASC, o.order_id ASC
    ) AS first_order_channel
FROM orders o
JOIN customers c ON o.customer_id = c.customer_id
WHERE o.customer_id IS NOT NULL
ORDER BY o.customer_id, o.ordered_at_ts;

-- N4: Статус последнего заказа
SELECT 
    o.order_id,
    o.customer_id,
    c.customer_name,
    o.ordered_at_ts,
    o.status AS current_order_status,
    LAST_VALUE(o.status) OVER (
        PARTITION BY o.customer_id 
        ORDER BY o.ordered_at_ts ASC, o.order_id ASC
        ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING
    ) AS last_global_order_status
FROM orders o
JOIN customers c ON o.customer_id = c.customer_id
WHERE o.customer_id IS NOT NULL
ORDER BY o.customer_id, o.ordered_at_ts;

-- N5: Сумма второго заказа
WITH order_totals AS (
    SELECT 
        o.order_id,
        o.customer_id,
        o.ordered_at_ts,
        SUM(oi.line_total) AS total_order_amount
    FROM orders o
    JOIN order_items oi ON o.order_id = oi.order_id
    WHERE o.status = 'completed' 
      AND o.customer_id IS NOT NULL
    GROUP BY o.order_id, o.customer_id, o.ordered_at_ts
),
calculated_second_orders AS (
    SELECT 
        ot.customer_id,
        c.customer_name,
        NTH_VALUE(ot.total_order_amount, 2) OVER (
            PARTITION BY ot.customer_id 
            ORDER BY ot.ordered_at_ts ASC, ot.order_id ASC
            ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING
        ) AS second_order_sum
    FROM order_totals ot
    JOIN customers c ON ot.customer_id = c.customer_id
)
SELECT DISTINCT
    customer_id,
    customer_name,
    second_order_sum
FROM calculated_second_orders
ORDER BY customer_id;

-- A1: Накопительная выручка продавца
SELECT 
    s.seller_id,
    s.seller_name,
    o.order_id,
    o.ordered_at_ts,
    oi.line_total AS current_item_revenue,
    SUM(oi.line_total) OVER (
        PARTITION BY s.seller_id          
        ORDER BY o.ordered_at_ts ASC, o.order_id ASC, oi.order_item_id ASC
    ) AS cumulative_seller_revenue
FROM order_items oi
JOIN orders o ON oi.order_id = o.order_id
JOIN products p ON oi.product_id = p.product_id
JOIN sellers s ON p.seller_id = s.seller_id
WHERE o.status = 'completed'
ORDER BY s.seller_id, o.ordered_at_ts;

-- A2: Число заказов покупателя
SELECT 
    o.customer_id,
    o.order_id,
    COUNT(*) OVER (
        PARTITION BY o.customer_id          
        ORDER BY o.ordered_at_ts ASC, o.order_id ASC
    ) AS order_sequence_number
FROM orders o
WHERE o.customer_id IS NOT NULL
ORDER BY o.customer_id, o.order_id;

-- A3: Средняя стоимость последних пяти заказов
WITH order_totals AS (
    SELECT 
        o.order_id,
        o.customer_id,
        o.ordered_at_ts,
        SUM(oi.line_total) AS total_order_amount
    FROM orders o
    JOIN order_items oi ON o.order_id = oi.order_id
    WHERE o.status = 'completed' 
      AND o.customer_id IS NOT NULL
    GROUP BY o.order_id, o.customer_id, o.ordered_at_ts
),
moving_averages AS (
    SELECT 
        ot.customer_id,
        c.customer_name,
        ot.ordered_at_ts,
        AVG(ot.total_order_amount) OVER (
            PARTITION BY ot.customer_id         
            ORDER BY ot.ordered_at_ts ASC, ot.order_id ASC 
            ROWS BETWEEN 4 PRECEDING AND CURRENT ROW
        ) AS avg_last_5_orders
    FROM order_totals ot
    JOIN customers c ON ot.customer_id = c.customer_id
)
SELECT DISTINCT ON (customer_id)
    customer_id,
    customer_name,
    ROUND(avg_last_5_orders::NUMERIC, 2) AS avg_last_5
FROM moving_averages
ORDER BY customer_id, ordered_at_ts DESC;

-- A4: Мин/Макс стоимость заказа покупателя
WITH order_totals AS (
    SELECT 
        o.order_id,
        o.customer_id,
        SUM(oi.line_total) AS total_order_amount
    FROM orders o
    JOIN order_items oi ON o.order_id = oi.order_id
    WHERE o.status = 'completed' 
      AND o.customer_id IS NOT NULL
    GROUP BY o.order_id, o.customer_id
)
SELECT 
    ot.order_id,
    ot.customer_id,
    c.customer_name,
    ot.total_order_amount AS current_order_sum,
    MIN(ot.total_order_amount) OVER (
        PARTITION BY ot.customer_id
    ) AS global_min_order_sum,
    MAX(ot.total_order_amount) OVER (
        PARTITION BY ot.customer_id
    ) AS global_max_order_sum
FROM order_totals ot
JOIN customers c ON ot.customer_id = c.customer_id
ORDER BY ot.customer_id, ot.order_id;

-- A5: Массив товаров этого заказа
SELECT 
    oi.order_item_id,
    oi.order_id,
    p.product_id,
    p.product_name AS current_product_name,
    ARRAY_AGG(p.product_name) OVER (
        PARTITION BY oi.order_id 
    ) AS order_products_array
FROM order_items oi
JOIN products p ON oi.product_id = p.product_id
ORDER BY oi.order_id, oi.order_item_id;