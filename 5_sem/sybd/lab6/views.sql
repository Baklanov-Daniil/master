-- создание предсталения
CREATE OR REPLACE VIEW v_cases_evidence AS
SELECT 
    c.case_id, 
    c.case_number, 
    c.status, 
    COUNT(e.evidence_id) AS evidence_count
FROM cases c
LEFT JOIN evidence e ON c.case_id = e.case_id
GROUP BY c.case_id, c.case_number, c.status;

SELECT * FROM v_cases_evidence

-- создание материализованного предсталения
CREATE MATERIALIZED VIEW mv_file_stats AS
SELECT 
    file_type, 
    COUNT(*) AS file_count, 
    SUM(file_size_bytes) AS total_size_bytes,
    ROUND(AVG(file_size_bytes)::numeric, 2) AS avg_size_bytes
FROM digital_files
GROUP BY file_type;

SELECT * FROM mv_file_stats

-- материализованное представление NO DATA  
CREATE MATERIALIZED VIEW mv_open_cases AS
SELECT case_id, case_number, registration_date, description
FROM cases
WHERE status = 'Открыто'
ORDER BY registration_date ASC
WITH NO DATA;

REFRESH MATERIALIZED VIEW mv_open_cases;
SELECT * FROM mv_open_cases;

-- Для запроса (b): Экспертизы с именами экспертов
CREATE OR REPLACE VIEW v_expert_stats AS
SELECT e.full_name, ex.examination_id, ex.start_date
FROM employees e 
JOIN examinations ex ON e.employee_id = ex.employee_id;

-- Для запроса (c): Детализация анализов файлов
CREATE OR REPLACE VIEW v_analysis_details AS
SELECT df.file_type, sd.device_type, ao.analysis_result
FROM analysis_objects ao
JOIN digital_files df ON ao.file_id = df.file_id
JOIN storage_devices sd ON df.device_id = sd.device_id;

-- a) Запрос на выборку, содержащий вложенный запрос
SELECT case_number, status, evidence_count
FROM v_cases_evidence
WHERE evidence_count > (
    SELECT AVG(evidence_count)::numeric 
    FROM v_cases_evidence 
    WHERE evidence_count > 0
)
ORDER BY evidence_count DESC;

-- b) Запрос на выборку с группировкой и простой сортировкой
SELECT full_name, COUNT(examination_id) AS exam_count
FROM v_expert_stats
GROUP BY full_name
ORDER BY exam_count DESC;

-- c) Запрос на выборку с группировкой и многоуровневой сортировкой
SELECT file_type, device_type, COUNT(*) AS analysis_count
FROM v_analysis_details
GROUP BY file_type, device_type
ORDER BY file_type ASC, analysis_count DESC;