--- дубликаты

INSERT INTO cases (case_id, case_number, registration_date, status, description)
VALUES (1, 'Д-ПРОВЕРКА-PK1', '2023-10-01', 'Открыто', 'Проверка первичного ключа');

INSERT INTO employees (employee_id, full_name, position, specialization, hire_date)
VALUES (1, 'Проверка PK сотрудник', 'Эксперт', 'Тестовая', '2023-10-01');

--- fk

INSERT INTO evidence (evidence_id, case_id, evidence_number, seizure_date, seizure_location, description)
VALUES (100, 999, 'ВД-ПРОВЕРКА-FK1', '2023-10-01', 'Место теста', 'Проверка связи с делом');

INSERT INTO examinations (examination_id, case_id, employee_id, appointment_date, start_date, end_date, conclusion)
VALUES (100, 1, 999, '2023-10-01', '2023-10-02', NULL, 'Проверка связи с сотрудником'); 

--- check

INSERT INTO examinations (examination_id, case_id, employee_id, appointment_date, start_date, end_date, conclusion)
VALUES (101, 1, 1, '2023-10-10', '2023-10-01', NULL, 'Проверка CHECK дат');

--- Not Null

INSERT INTO employees (employee_id, full_name, position, specialization, hire_date)
VALUES (50, NULL, 'Эксперт', 'Тестовая специализация', '2023-12-01');

INSERT INTO digital_files (file_id, device_id, file_name, file_hash_sha256, file_size_bytes, file_type, discovery_date)
VALUES (50, 1, NULL, '0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdef', 1024, 'other', '2023-12-01');