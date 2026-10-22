-- 2.1. запрос с использованием всех типов соединений;
-- Дела + вещдоки: только те дела, у которых есть вещдоки
SELECT 
    c.case_number,
    c.status,
    e.evidence_number,
    e.seizure_location
FROM cases c
INNER JOIN evidence e ON c.case_id = e.case_id
ORDER BY c.case_number;

-- Все дела + их вещдоки
SELECT 
    c.case_number,
    c.status,
    e.evidence_number,
    e.seizure_location
FROM cases c
LEFT JOIN evidence e ON c.case_id = e.case_id
ORDER BY c.case_number;

-- Все вещдоки + их дела (если дела нет — NULL)
SELECT 
    c.case_number,
    c.status,
    e.evidence_number,
    e.seizure_location
FROM cases c
RIGHT JOIN evidence e ON c.case_id = e.case_id
ORDER BY e.evidence_number;

-- Все дела И все вещдоки
SELECT 
    c.case_number,
    c.status,
    e.evidence_number,
    e.seizure_location
FROM cases c
FULL JOIN evidence e ON c.case_id = e.case_id
ORDER BY COALESCE(c.case_number, '—'), COALESCE(e.evidence_number, '—');

-- 2.2. Запрос с использованием оператора DISTINCT
-- Уникальные статусы дел
SELECT DISTINCT status
FROM cases
ORDER BY status;

-- 2.3. Запрос с использованием оператора BETWEEN
-- Файлы размером от 1 МБ до 100 МБ (1 048 576 – 104 857 600 байт)
SELECT 
    file_name,
    file_size_bytes,
    file_type,
    discovery_date
FROM digital_files
WHERE file_size_bytes BETWEEN 1048576 AND 104857600
ORDER BY file_size_bytes DESC;

-- 2.4. Запрос с использованием оператора LIMIT
-- Топ-5 самых крупных файлов
SELECT 
    file_name,
    file_size_bytes,
    device_id
FROM digital_files
ORDER BY file_size_bytes DESC
LIMIT 5;

-- 2.5. Запрос с использованием оператора OFFSET
-- Следующие 3 файла после топ-5 (позиции 6–8)
SELECT 
    file_name,
    file_size_bytes,
    device_id
FROM digital_files
ORDER BY file_size_bytes DESC
LIMIT 3 OFFSET 5;

-- 2.6. Запрос с шаблонами LIKE / NOT LIKE
-- LIKE: файлы с расширением .db или .xlsx (содержат "db" или "xlsx")
SELECT 
    file_name,
    file_hash_sha256,
    file_type
FROM digital_files
WHERE file_name LIKE '%.db' OR file_name LIKE '%.xlsx';

-- NOT LIKE: все файлы, НЕ являющиеся документами
SELECT 
    file_name,
    file_type
FROM digital_files
WHERE file_type NOT LIKE 'document';

-- 2.7. Запрос с регулярным выражением POSIX
-- Файлы, имя которых начинается с буквы и содержит цифры
SELECT 
    file_name,
    file_type
FROM digital_files
WHERE file_name ~ '^[a-zA-Z].*\d+';

-- Файлы, НЕ оканчивающиеся на .log
SELECT 
    file_name,
    file_type
FROM digital_files
WHERE file_name !~ '\.log$';

-- 2.8. Агрегированные данные (3 статистические функции)
-- Статистика по файлам: количество, средний размер, максимальный хеш (лексикографически)
SELECT 
    COUNT(*) AS total_files,
    ROUND(AVG(file_size_bytes)::numeric, 2) AS avg_size_bytes,
    MAX(file_size_bytes) AS max_size_bytes,
    MIN(discovery_date) AS earliest_discovery,
    MAX(discovery_date) AS latest_discovery
FROM digital_files;

-- 2.9. Вложенный запрос
-- Дела, у которых есть экспертизы, начатые после 2023-10-01
SELECT 
    case_number,
    status,
    registration_date
FROM cases
WHERE case_id IN (
    SELECT DISTINCT case_id
    FROM examinations
    WHERE start_date > '2023-10-01'
)
ORDER BY registration_date;

-- 2.10. Условное выражение CASE
-- Классификация файлов по размеру
SELECT 
    file_name,
    file_size_bytes,
    CASE
        WHEN file_size_bytes < 1048576 THEN 'Малый (<1 МБ)'
        WHEN file_size_bytes BETWEEN 1048576 AND 10485760 THEN 'Средний (1–10 МБ)'
        WHEN file_size_bytes BETWEEN 10485761 AND 104857600 THEN 'Крупный (10–100 МБ)'
        ELSE 'Очень крупный (>100 МБ)'
    END AS size_category
FROM digital_files
ORDER BY file_size_bytes;

-- 2.11. Группировка + простая сортировка
-- Количество файлов по типу, отсортировано по убыванию количества
SELECT 
    file_type,
    COUNT(*) AS file_count
FROM digital_files
GROUP BY file_type
ORDER BY file_count DESC;

-- 2.12. Группировка с условием + многоуровневая сортировка
-- Дела с количеством вещдоков >= 2, сортировка: сначала по количеству, потом по номеру дела
SELECT 
    c.case_number,
    c.status,
    COUNT(e.evidence_id) AS evidence_count
FROM cases c
JOIN evidence e ON c.case_id = e.case_id
GROUP BY c.case_id, c.case_number, c.status
HAVING COUNT(e.evidence_id) >= 2
ORDER BY evidence_count DESC, c.case_number ASC;