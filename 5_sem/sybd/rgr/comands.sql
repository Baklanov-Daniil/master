-- ---------- Очистка (для повторного запуска) ----------
DROP MATERIALIZED VIEW IF EXISTS mv_forecast_accuracy;
DROP MATERIALIZED VIEW IF EXISTS mv_monthly_demand;
DROP VIEW IF EXISTS v_order_total;
DROP PROCEDURE IF EXISTS pr_refresh_demand_mvs();
DROP FUNCTION IF EXISTS fn_forecast_mape(integer, integer);
DROP TABLE IF EXISTS demand_forecast, order_item, orders, product, customer, region, product_category CASCADE;

-- ============================================================
-- 1. DDL: таблицы без внешних ключей
-- ============================================================

CREATE TABLE product_category (
    category_id  INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name         VARCHAR(100) NOT NULL UNIQUE,
    description  TEXT
);

CREATE TABLE region (
    region_id         INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name              VARCHAR(100) NOT NULL UNIQUE,
    federal_district  VARCHAR(50)  NOT NULL,
    population        INTEGER      NOT NULL CHECK (population >= 0)
);

CREATE TABLE customer (
    customer_id        INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    region_id          INTEGER      NOT NULL,
    customer_type      VARCHAR(3)   NOT NULL CHECK (customer_type IN ('B2B','B2C')),
    name               VARCHAR(150) NOT NULL,
    registration_date  DATE         NOT NULL DEFAULT CURRENT_DATE
);

CREATE TABLE product (
    product_id  INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    category_id INTEGER       NOT NULL,
    sku         VARCHAR(30)   NOT NULL UNIQUE,
    name        VARCHAR(150)  NOT NULL,
    price       NUMERIC(12,2) NOT NULL CHECK (price >= 0),
    stock_qty   INTEGER       NOT NULL DEFAULT 0 CHECK (stock_qty >= 0),
    unit        VARCHAR(10)   NOT NULL DEFAULT 'шт'
                              CHECK (unit IN ('шт','кг','л','т','м'))
);

CREATE TABLE orders (
    order_id     INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    customer_id  INTEGER     NOT NULL,
    order_date   DATE        NOT NULL CHECK (order_date <= CURRENT_DATE),
    status       VARCHAR(12) NOT NULL DEFAULT 'completed'
                             CHECK (status IN ('new','completed','cancelled'))
);

CREATE TABLE order_item (
    order_id    INTEGER       NOT NULL,
    item_no     SMALLINT      NOT NULL CHECK (item_no > 0),
    product_id  INTEGER       NOT NULL,
    quantity    INTEGER       NOT NULL CHECK (quantity > 0),
    unit_price  NUMERIC(12,2) NOT NULL CHECK (unit_price >= 0),
    line_total  NUMERIC(14,2) GENERATED ALWAYS AS
                    (ROUND(quantity * unit_price, 2)) STORED,
    CONSTRAINT pk_order_item PRIMARY KEY (order_id, item_no)
);

CREATE TABLE demand_forecast (
    forecast_id     INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    product_id      INTEGER     NOT NULL,
    region_id       INTEGER     NOT NULL,
    forecast_month  DATE        NOT NULL
                        CHECK (forecast_month = date_trunc('month', forecast_month)::date),
    forecast_qty    INTEGER     NOT NULL CHECK (forecast_qty >= 0),
    method          VARCHAR(10) NOT NULL
                        CHECK (method IN ('ARIMA','ML','EXP','MA','MANUAL')),
    created_at      TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT ak1_forecast UNIQUE (product_id, region_id, forecast_month, method)
);

-- ============================================================
-- 2. DDL: блок связей (внешние ключи)
-- ============================================================

ALTER TABLE customer
    ADD CONSTRAINT fk_customer_region
    FOREIGN KEY (region_id) REFERENCES region (region_id)
    ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE product
    ADD CONSTRAINT fk_product_category
    FOREIGN KEY (category_id) REFERENCES product_category (category_id)
    ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE orders
    ADD CONSTRAINT fk_orders_customer
    FOREIGN KEY (customer_id) REFERENCES customer (customer_id)
    ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE order_item
    ADD CONSTRAINT fk_order_item_orders
    FOREIGN KEY (order_id) REFERENCES orders (order_id)
    ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE order_item
    ADD CONSTRAINT fk_order_item_product
    FOREIGN KEY (product_id) REFERENCES product (product_id)
    ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE demand_forecast
    ADD CONSTRAINT fk_forecast_product
    FOREIGN KEY (product_id) REFERENCES product (product_id)
    ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE demand_forecast
    ADD CONSTRAINT fk_forecast_region
    FOREIGN KEY (region_id) REFERENCES region (region_id)
    ON UPDATE CASCADE ON DELETE CASCADE;

-- ============================================================
-- БЛОК 3. Индексы (PostgreSQL)
-- ============================================================

-- Внешние ключи: ускорение JOIN и каскадных операций
CREATE INDEX ix_customer_region       ON customer (region_id);
CREATE INDEX ix_product_category      ON product (category_id);
CREATE INDEX ix_orders_customer       ON orders (customer_id);
CREATE INDEX ix_order_item_product    ON order_item (product_id);

-- Частые условия отбора и сортировки
CREATE INDEX ix_customer_type         ON customer (customer_type);
CREATE INDEX ix_product_price         ON product (price);
CREATE INDEX ix_orders_date           ON orders (order_date);

-- Составной индекс под агрегацию спроса по статусу и периоду
CREATE INDEX ix_orders_status_date    ON orders (status, order_date);

-- Индексы для аналитики прогнозов
CREATE INDEX ix_forecast_month        ON demand_forecast (forecast_month);
CREATE INDEX ix_forecast_region_month ON demand_forecast (region_id, forecast_month);

-- ============================================================
-- Блок 4. Вставка данных
-- ============================================================

-- 1. Виды продукции (ID 1–15)
INSERT INTO product_category (category_id, name, description) VALUES
(1,'Молочная продукция','Молоко, кефир, йогурты'),
(2,'Хлебобулочные изделия','Хлеб, батоны, выпечка'),
(3,'Бакалея','Крупы, макароны, мука'),
(4,'Напитки','Соки, воды, лимонады'),
(5,'Кондитерские изделия','Шоколад, печенье, конфеты'),
(6,'Мясная продукция','Колбасы, полуфабрикаты'),
(7,'Овощи и фрукты','Свежая плодоовощная продукция'),
(8,'Замороженная продукция','Заморозка овощная и ягодная'),
(9,'Бытовая химия','Моющие и чистящие средства'),
(10,'Снеки','Чипсы, орехи, сухарики'),
(11,'Чай и кофе','Чай листовой, кофе зерновой'),
(12,'Консервы','Рыбные и овощные консервы'),
(13,'Соусы и приправы','Соусы, специи, майонез'),
(14,'Детское питание','Смеси, пюре, каши'),
(15,'Яйцо','Куриное яйцо столовое');

-- 2. Регионы (ID 1–16)
INSERT INTO region (region_id, name, federal_district, population) VALUES
(1,'Москва','ЦФО',13100000),
(2,'Санкт-Петербург','СЗФО',5600000),
(3,'Новосибирск','СФО',1630000),
(4,'Екатеринбург','УФО',1540000),
(5,'Казань','ПФО',1310000),
(6,'Нижний Новгород','ПФО',1210000),
(7,'Челябинск','УФО',1180000),
(8,'Самара','ПФО',1160000),
(9,'Омск','СФО',1110000),
(10,'Ростов-на-Дону','ЮФО',1140000),
(11,'Уфа','ПФО',1150000),
(12,'Красноярск','СФО',1190000),
(13,'Воронеж','ЦФО',1050000),
(14,'Пермь','ПФО',1030000),
(15,'Волгоград','ЮФО',1020000),
(16,'Краснодар','ЮФО',1120000);

-- 3. Заказчики (ID 1–20)
INSERT INTO customer (customer_id, region_id, customer_type, name, registration_date) VALUES
(1,1,'B2C','Клиент №001','2023-01-21'),
(2,2,'B2C','Клиент №002','2023-02-01'),
(3,3,'B2B','ООО «Торг-3»','2023-02-12'),
(4,4,'B2C','Клиент №004','2023-02-23'),
(5,5,'B2C','Клиент №005','2023-03-06'),
(6,6,'B2B','ООО «Торг-6»','2023-03-17'),
(7,7,'B2C','Клиент №007','2023-03-28'),
(8,8,'B2C','Клиент №008','2023-04-08'),
(9,9,'B2B','ООО «Торг-9»','2023-04-19'),
(10,10,'B2C','Клиент №010','2023-04-30'),
(11,11,'B2C','Клиент №011','2023-05-11'),
(12,12,'B2B','ООО «Торг-12»','2023-05-22'),
(13,13,'B2C','Клиент №013','2023-06-02'),
(14,14,'B2C','Клиент №014','2023-06-13'),
(15,15,'B2B','ООО «Торг-15»','2023-06-24'),
(16,16,'B2C','Клиент №016','2023-07-05'),
(17,1,'B2C','Клиент №017','2023-07-16'),
(18,2,'B2B','ООО «Торг-18»','2023-07-27'),
(19,3,'B2C','Клиент №019','2023-08-07'),
(20,4,'B2C','Клиент №020','2023-08-18');

-- 4. Продукты (ID 1–20)
INSERT INTO product (product_id, category_id, sku, name, price, stock_qty, unit) VALUES
(1,1,'SKU-0001','Товар 01',97.50,100000,'шт'),
(2,2,'SKU-0002','Товар 02',115.00,100000,'кг'),
(3,3,'SKU-0003','Товар 03',132.50,100000,'л'),
(4,4,'SKU-0004','Товар 04',150.00,100000,'шт'),
(5,5,'SKU-0005','Товар 05',167.50,100000,'кг'),
(6,6,'SKU-0006','Товар 06',185.00,100000,'шт'),
(7,7,'SKU-0007','Товар 07',202.50,100000,'кг'),
(8,8,'SKU-0008','Товар 08',220.00,100000,'л'),
(9,9,'SKU-0009','Товар 09',237.50,100000,'шт'),
(10,10,'SKU-0010','Товар 10',255.00,100000,'кг'),
(11,11,'SKU-0011','Товар 11',272.50,100000,'шт'),
(12,12,'SKU-0012','Товар 12',290.00,100000,'кг'),
(13,13,'SKU-0013','Товар 13',307.50,100000,'л'),
(14,14,'SKU-0014','Товар 14',325.00,100000,'шт'),
(15,15,'SKU-0015','Товар 15',342.50,100000,'кг'),
(16,1,'SKU-0016','Товар 16',360.00,100000,'шт'),
(17,2,'SKU-0017','Товар 17',377.50,100000,'кг'),
(18,3,'SKU-0018','Товар 18',395.00,100000,'л'),
(19,4,'SKU-0019','Товар 19',412.50,100000,'шт'),
(20,5,'SKU-0020','Товар 20',430.00,100000,'кг');

-- 5. Заказы (ID 1–96, включая отменённые 11, 22, 33)
INSERT INTO orders (order_id, customer_id, order_date, status) VALUES
(1,1,'2024-01-03','completed'),(2,2,'2024-01-09','completed'),(3,3,'2024-01-15','completed'),(4,4,'2024-01-21','completed'),
(5,5,'2024-02-03','completed'),(6,6,'2024-02-09','completed'),(7,7,'2024-02-15','completed'),(8,8,'2024-02-21','completed'),
(9,9,'2024-03-03','completed'),(10,10,'2024-03-09','completed'),(11,11,'2024-03-15','cancelled'),(12,12,'2024-03-21','completed'),
(13,13,'2024-04-03','completed'),(14,14,'2024-04-09','completed'),(15,15,'2024-04-15','completed'),(16,16,'2024-04-21','completed'),
(17,17,'2024-05-03','completed'),(18,18,'2024-05-09','completed'),(19,19,'2024-05-15','completed'),(20,20,'2024-05-21','completed'),
(21,1,'2024-06-03','completed'),(22,2,'2024-06-09','cancelled'),(23,3,'2024-06-15','completed'),(24,4,'2024-06-21','completed'),
(25,5,'2024-07-03','completed'),(26,6,'2024-07-09','completed'),(27,7,'2024-07-15','completed'),(28,8,'2024-07-21','completed'),
(29,9,'2024-08-03','completed'),(30,10,'2024-08-09','completed'),(31,11,'2024-08-15','completed'),(32,12,'2024-08-21','completed'),
(33,13,'2024-09-03','completed'),(34,14,'2024-09-09','completed'),(35,15,'2024-09-15','completed'),(36,16,'2024-09-21','cancelled'),
(37,17,'2024-10-03','completed'),(38,18,'2024-10-09','completed'),(39,19,'2024-10-15','completed'),(40,20,'2024-10-21','completed'),
(41,1,'2024-11-03','completed'),(42,2,'2024-11-09','completed'),(43,3,'2024-11-15','completed'),(44,4,'2024-11-21','completed'),
(45,5,'2024-12-03','completed'),(46,6,'2024-12-09','completed'),(47,7,'2024-12-15','completed'),(48,8,'2024-12-21','completed'),
(49,9,'2025-01-03','completed'),(50,10,'2025-01-09','completed'),(51,11,'2025-01-15','completed'),(52,12,'2025-01-21','completed'),
(53,13,'2025-02-03','completed'),(54,14,'2025-02-09','completed'),(55,15,'2025-02-15','completed'),(56,16,'2025-02-21','completed'),
(57,17,'2025-03-03','completed'),(58,18,'2025-03-09','completed'),(59,19,'2025-03-15','completed'),(60,20,'2025-03-21','completed'),
(61,1,'2025-04-03','completed'),(62,2,'2025-04-09','completed'),(63,3,'2025-04-15','completed'),(64,4,'2025-04-21','completed'),
(65,5,'2025-05-03','completed'),(66,6,'2025-05-09','completed'),(67,7,'2025-05-15','completed'),(68,8,'2025-05-21','completed'),
(69,9,'2025-06-03','completed'),(70,10,'2025-06-09','completed'),(71,11,'2025-06-15','completed'),(72,12,'2025-06-21','completed'),
(73,13,'2025-07-03','completed'),(74,14,'2025-07-09','completed'),(75,15,'2025-07-15','completed'),(76,16,'2025-07-21','completed'),
(77,17,'2025-08-03','completed'),(78,18,'2025-08-09','completed'),(79,19,'2025-08-15','completed'),(80,20,'2025-08-21','completed'),
(81,1,'2025-09-03','completed'),(82,2,'2025-09-09','completed'),(83,3,'2025-09-15','completed'),(84,4,'2025-09-21','completed'),
(85,5,'2025-10-03','completed'),(86,6,'2025-10-09','completed'),(87,7,'2025-10-15','completed'),(88,8,'2025-10-21','completed'),
(89,9,'2025-11-03','completed'),(90,10,'2025-11-09','completed'),(91,11,'2025-11-15','completed'),(92,12,'2025-11-21','completed'),
(93,13,'2025-12-03','completed'),(94,14,'2025-12-09','completed'),(95,15,'2025-12-15','completed'),(96,16,'2025-12-21','completed');

-- 6. Позиции в заказе (по 3 строки на каждый НЕотменённый заказ)
-- Отменённые заказы: 11, 22, 33, 36 — для них позиции НЕ создаём
INSERT INTO order_item (order_id, item_no, product_id, quantity, unit_price) VALUES
(1,1,1,5,97.50),(1,2,6,8,185.00),(1,3,11,3,272.50),
(2,1,2,7,115.00),(2,2,7,4,202.50),(2,3,12,9,290.00),
(3,1,3,6,132.50),(3,2,8,10,220.00),(3,3,13,2,307.50),
(4,1,4,4,150.00),(4,2,9,7,237.50),(4,3,14,5,325.00),
(5,1,5,9,167.50),(5,2,10,3,255.00),(5,3,15,8,342.50),
(6,1,6,5,185.00),(6,2,11,10,272.50),(6,3,16,4,360.00),
(7,1,7,7,202.50),(7,2,12,2,290.00),(7,3,17,9,377.50),
(8,1,8,4,220.00),(8,2,13,8,307.50),(8,3,18,6,395.00),
(9,1,9,10,237.50),(9,2,14,5,325.00),(9,3,19,3,412.50),
(10,1,10,6,255.00),(10,2,15,9,342.50),(10,3,20,7,430.00),
(12,1,1,4,97.50),(12,2,6,10,185.00),(12,3,11,5,272.50),
(13,1,2,8,115.00),(13,2,7,3,202.50),(13,3,12,7,290.00),
(14,1,3,5,132.50),(14,2,8,9,220.00),(14,3,13,2,307.50),
(15,1,4,10,150.00),(15,2,9,6,237.50),(15,3,14,8,325.00),
(16,1,5,3,167.50),(16,2,10,7,255.00),(16,3,15,4,342.50),
(17,1,6,9,185.00),(17,2,11,2,272.50),(17,3,16,10,360.00),
(18,1,7,5,202.50),(18,2,12,8,290.00),(18,3,17,3,377.50),
(19,1,8,7,220.00),(19,2,13,4,307.50),(19,3,18,9,395.00),
(20,1,9,2,237.50),(20,2,14,10,325.00),(20,3,19,6,412.50),
(21,1,10,8,255.00),(21,2,15,3,342.50),(21,3,20,5,430.00),
(23,1,1,6,97.50),(23,2,6,9,185.00),(23,3,11,4,272.50),
(24,1,2,10,115.00),(24,2,7,5,202.50),(24,3,12,8,290.00),
(25,1,3,3,132.50),(25,2,8,7,220.00),(25,3,13,9,307.50),
(26,1,4,8,150.00),(26,2,9,2,237.50),(26,3,14,6,325.00),
(27,1,5,4,167.50),(27,2,10,10,255.00),(27,3,15,3,342.50),
(28,1,6,7,185.00),(28,2,11,5,272.50),(28,3,16,8,360.00),
(29,1,7,9,202.50),(29,2,12,3,290.00),(29,3,17,10,377.50),
(30,1,8,2,220.00),(30,2,13,8,307.50),(30,3,18,4,395.00),
(31,1,9,6,237.50),(31,2,14,9,325.00),(31,3,19,7,412.50),
(32,1,10,5,255.00),(32,2,15,4,342.50),(32,3,20,8,430.00),
(34,1,1,10,97.50),(34,2,6,7,185.00),(34,3,11,2,272.50),
(35,1,2,4,115.00),(35,2,7,9,202.50),(35,3,12,5,290.00),
(37,1,4,6,150.00),(37,2,9,8,237.50),(37,3,14,4,325.00),
(38,1,5,9,167.50),(38,2,10,5,255.00),(38,3,15,7,342.50),
(39,1,6,3,185.00),(39,2,11,10,272.50),(39,3,16,6,360.00),
(40,1,7,8,202.50),(40,2,12,4,290.00),(40,3,17,9,377.50),
(41,1,8,5,220.00),(41,2,13,7,307.50),(41,3,18,3,395.00),
(42,1,9,10,237.50),(42,2,14,2,325.00),(42,3,19,8,412.50),
(43,1,10,7,255.00),(43,2,15,6,342.50),(43,3,20,4,430.00),
(44,1,1,9,97.50),(44,2,6,3,185.00),(44,3,11,10,272.50),
(45,1,2,5,115.00),(45,2,7,8,202.50),(45,3,12,2,290.00),
(46,1,3,7,132.50),(46,2,8,4,220.00),(46,3,13,9,307.50),
(47,1,4,2,150.00),(47,2,9,10,237.50),(47,3,14,5,325.00),
(48,1,5,8,167.50),(48,2,10,6,255.00),(48,3,15,3,342.50),
(49,1,6,4,185.00),(49,2,11,9,272.50),(49,3,16,7,360.00),
(50,1,7,10,202.50),(50,2,12,5,290.00),(50,3,17,2,377.50),
(51,1,8,3,220.00),(51,2,13,8,307.50),(51,3,18,6,395.00),
(52,1,9,9,237.50),(52,2,14,4,325.00),(52,3,19,10,412.50),
(53,1,10,6,255.00),(53,2,15,7,342.50),(53,3,20,3,430.00),
(54,1,1,2,97.50),(54,2,6,10,185.00),(54,3,11,8,272.50),
(55,1,2,7,115.00),(55,2,7,5,202.50),(55,3,12,9,290.00),
(56,1,3,4,132.50),(56,2,8,3,220.00),(56,3,13,10,307.50),
(57,1,4,8,150.00),(57,2,9,6,237.50),(57,3,14,2,325.00),
(58,1,5,10,167.50),(58,2,10,4,255.00),(58,3,15,7,342.50),
(59,1,6,5,185.00),(59,2,11,8,272.50),(59,3,16,3,360.00),
(60,1,7,9,202.50),(60,2,12,2,290.00),(60,3,17,6,377.50),
(61,1,8,3,220.00),(61,2,13,9,307.50),(61,3,18,5,395.00),
(62,1,9,7,237.50),(62,2,14,4,325.00),(62,3,19,8,412.50),
(63,1,10,10,255.00),(63,2,15,2,342.50),(63,3,20,6,430.00),
(64,1,1,8,97.50),(64,2,6,5,185.00),(64,3,11,9,272.50),
(65,1,2,3,115.00),(65,2,7,10,202.50),(65,3,12,4,290.00),
(66,1,3,6,132.50),(66,2,8,7,220.00),(66,3,13,2,307.50),
(67,1,4,9,150.00),(67,2,9,3,237.50),(67,3,14,8,325.00),
(68,1,5,2,167.50),(68,2,10,9,255.00),(68,3,15,5,342.50),
(69,1,6,7,185.00),(69,2,11,4,272.50),(69,3,16,10,360.00),
(70,1,7,5,202.50),(70,2,12,8,290.00),(70,3,17,3,377.50),
(71,1,8,10,220.00),(71,2,13,6,307.50),(71,3,18,9,395.00),
(72,1,9,4,237.50),(72,2,14,2,325.00),(72,3,19,7,412.50),
(73,1,10,8,255.00),(73,2,15,5,342.50),(73,3,20,3,430.00),
(74,1,1,6,97.50),(74,2,6,9,185.00),(74,3,11,4,272.50),
(75,1,2,2,115.00),(75,2,7,7,202.50),(75,3,12,10,290.00),
(76,1,3,9,132.50),(76,2,8,3,220.00),(76,3,13,6,307.50),
(77,1,4,5,150.00),(77,2,9,8,237.50),(77,3,14,2,325.00),
(78,1,5,10,167.50),(78,2,10,4,255.00),(78,3,15,7,342.50),
(79,1,6,3,185.00),(79,2,11,9,272.50),(79,3,16,5,360.00),
(80,1,7,8,202.50),(80,2,12,2,290.00),(80,3,17,10,377.50),
(81,1,8,4,220.00),(81,2,13,7,307.50),(81,3,18,3,395.00),
(82,1,9,6,237.50),(82,2,14,10,325.00),(82,3,19,8,412.50),
(83,1,10,2,255.00),(83,2,15,5,342.50),(83,3,20,9,430.00),
(84,1,1,7,97.50),(84,2,6,4,185.00),(84,3,11,10,272.50),
(85,1,2,9,115.00),(85,2,7,6,202.50),(85,3,12,3,290.00),
(86,1,3,5,132.50),(86,2,8,8,220.00),(86,3,13,2,307.50),
(87,1,4,10,150.00),(87,2,9,3,237.50),(87,3,14,7,325.00),
(88,1,5,4,167.50),(88,2,10,9,255.00),(88,3,15,6,342.50),
(89,1,6,8,185.00),(89,2,11,2,272.50),(89,3,16,5,360.00),
(90,1,7,3,202.50),(90,2,12,10,290.00),(90,3,17,4,377.50),
(91,1,8,6,220.00),(91,2,13,5,307.50),(91,3,18,9,395.00),
(92,1,9,2,237.50),(92,2,14,8,325.00),(92,3,19,7,412.50),
(93,1,10,5,255.00),(93,2,15,3,342.50),(93,3,20,10,430.00),
(94,1,1,9,97.50),(94,2,6,7,185.00),(94,3,11,4,272.50),
(95,1,2,3,115.00),(95,2,7,10,202.50),(95,3,12,6,290.00),
(96,1,3,8,132.50),(96,2,8,2,220.00),(96,3,13,9,307.50);

-- 7. Прогноз спроса (24 записи: 12 месяцев 2025 × 2 метода)
INSERT INTO demand_forecast (forecast_id, product_id, region_id, forecast_month, forecast_qty, method) VALUES
(1,1,1,'2025-01-01',43,'MA'),
(2,2,2,'2025-02-01',46,'ARIMA'),
(3,3,3,'2025-03-01',49,'MA'),
(4,4,4,'2025-04-01',52,'ARIMA'),
(5,5,5,'2025-05-01',55,'MA'),
(6,6,6,'2025-06-01',58,'ARIMA'),
(7,7,7,'2025-07-01',61,'MA'),
(8,8,8,'2025-08-01',64,'ARIMA'),
(9,9,9,'2025-09-01',67,'MA'),
(10,10,10,'2025-10-01',70,'ARIMA'),
(11,11,11,'2025-11-01',73,'MA'),
(12,12,12,'2025-12-01',76,'ARIMA'),
(13,13,13,'2025-01-01',79,'MA'),
(14,14,14,'2025-02-01',82,'ARIMA'),
(15,15,15,'2025-03-01',85,'MA'),
(16,16,16,'2025-04-01',88,'ARIMA'),
(17,1,1,'2025-05-01',91,'MA'),
(18,2,2,'2025-06-01',94,'ARIMA'),
(19,3,3,'2025-07-01',97,'MA'),
(20,4,4,'2025-08-01',100,'ARIMA'),
(21,5,5,'2025-09-01',103,'MA'),
(22,6,6,'2025-10-01',106,'ARIMA'),
(23,7,7,'2025-11-01',109,'MA'),
(24,8,8,'2025-12-01',112,'ARIMA');

-- ============================================================
-- БЛОК 5. Представления
-- ============================================================

-- 1. Обычное представление: итоговая сумма заказа
-- Вычисляется динамически при каждом обращении к представлению
CREATE OR REPLACE VIEW v_order_total AS
SELECT o.order_id,
       o.customer_id,
       c.name AS customer_name,
       r.name AS region_name,
       o.order_date,
       o.status,
       COALESCE(SUM(i.line_total), 0)::numeric(14,2) AS total_amount,
       COUNT(i.item_no)::integer                      AS items_count
FROM orders o
JOIN customer   c ON c.customer_id = o.customer_id
JOIN region     r ON r.region_id   = c.region_id
LEFT JOIN order_item i ON i.order_id = o.order_id
GROUP BY o.order_id, o.customer_id, c.name, r.name, o.order_date, o.status;

-- 2. Материализованное представление: фактический месячный спрос
-- Агрегирует продажи по месяцам для анализа динамики спроса
CREATE MATERIALIZED VIEW mv_monthly_demand AS
SELECT date_trunc('month', o.order_date)::date AS month,
       i.product_id,
       p.name                                   AS product_name,
       c.region_id,
       SUM(i.quantity)::integer                 AS total_qty,
       SUM(i.line_total)::numeric(14,2)         AS total_revenue,
       COUNT(*)::integer                        AS positions_count,
       ROUND(AVG(i.unit_price), 2)::numeric(12,2) AS avg_unit_price
FROM order_item i
JOIN orders   o ON o.order_id    = i.order_id
JOIN customer c ON c.customer_id = o.customer_id
JOIN product  p ON p.product_id  = i.product_id
WHERE o.status = 'completed'
GROUP BY 1, 2, 3, 4
WITH DATA;

-- Уникальный индекс обязателен для REFRESH CONCURRENTLY
CREATE UNIQUE INDEX ux_mv_monthly_demand
    ON mv_monthly_demand (month, product_id, region_id);

CREATE INDEX ix_mv_monthly_demand_product
    ON mv_monthly_demand (product_id);

CREATE INDEX ix_mv_monthly_demand_region
    ON mv_monthly_demand (region_id);

-- 3. Материализованное представление: точность прогнозов
-- Сравнивает план с фактом, считает абсолютную и процентную ошибку
CREATE MATERIALIZED VIEW mv_forecast_accuracy AS
SELECT f.forecast_id,
       f.product_id,
       p.name            AS product_name,
       f.region_id,
       r.name            AS region_name,
       f.forecast_month,
       f.method,
       f.forecast_qty,
       COALESCE(d.total_qty, 0)::integer AS actual_qty,
       ABS(f.forecast_qty - COALESCE(d.total_qty, 0))::integer AS abs_error,
       ROUND(
           ABS(f.forecast_qty - COALESCE(d.total_qty, 0))
           / NULLIF(COALESCE(d.total_qty, 0), 0) * 100.0,
           4
       )::numeric(10,4)                  AS pct_error
FROM demand_forecast f
JOIN product p ON p.product_id = f.product_id
JOIN region  r ON r.region_id  = f.region_id
LEFT JOIN mv_monthly_demand d
       ON d.product_id  = f.product_id
      AND d.region_id   = f.region_id
      AND d.month       = f.forecast_month
WITH DATA;

CREATE UNIQUE INDEX ux_mv_forecast_accuracy
    ON mv_forecast_accuracy (forecast_id);

CREATE INDEX ix_mv_forecast_accuracy_prod
    ON mv_forecast_accuracy (product_id, region_id);