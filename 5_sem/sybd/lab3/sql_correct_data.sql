-- копирование бд из 2 лобораторной работы
CREATE DATABASE lab3
WITH TEMPLATE lab2
OWNER = postgres;

--- cases
INSERT INTO cases (case_id, case_number, registration_date, status, description) VALUES
    (1, 'Д-101/2023', '2023-09-01', 'Открыто', 'Кража персональных данных сотрудников ООО Ромашка'),
    (2, 'Д-102/2023', '2023-09-15', 'В работе', 'Мошенничество в сети Интернет, фишинговая рассылка'),
    (3, 'Д-103/2023', '2023-10-02', 'Экспертиза', 'Незаконный доступ к корпоративной базе данных'),
    (4, 'Д-104/2023', '2023-10-18', 'Закрыто', 'Утечка коммерческой тайны через USB-носитель'),
    (5, 'Д-105/2023', '2023-11-05', 'Открыто', 'Кибербуллинг и угрозы по электронной почте'),
    (6, 'Д-106/2023', '2023-11-20', 'Приостановлено', 'Подозрение на подделку электронных документов');

--- emoplouees
INSERT INTO employees (employee_id, full_name, position, specialization, hire_date) VALUES
    (1, 'Иванов Иван Иванович', 'Старший эксперт', 'Компьютерная криминалистика', '2015-03-10'),
    (2, 'Петрова Анна Сергеевна', 'Эксперт', 'Мобильные устройства', '2018-07-22'),
    (3, 'Сидоров Алексей Петрович', 'Эксперт', 'Сетевая криминалистика', '2019-01-15'),
    (4, 'Козлова Мария Дмитриевна', 'Младший эксперт', 'Анализ файговых систем', '2021-09-01'),
    (5, 'Новиков Сергей Владимирович', 'Старший эксперт', 'Восстановление данных', '2012-06-20');

--- evidance 
INSERT INTO evidence (evidence_id, case_id, evidence_number, seizure_date, seizure_location, description) VALUES
    (1, 1, 'ВД-001', '2023-09-02', 'Офис ООО Ромашка, каб. 305', 'Ноутбук подозреваемого'),
    (2, 1, 'ВД-002', '2023-09-02', 'Офис ООО Ромашка, каб. 305', 'Внешний жесткий диск'),
    (3, 2, 'ВД-003', '2023-09-16', 'Место жительства гр. Сидорова', 'Смартфон iPhone 13'),
    (4, 2, 'ВД-004', '2023-09-16', 'Место жительства гр. Сидорова', 'Планшет iPad Air'),
    (5, 3, 'ВД-005', '2023-10-03', 'Серверная ООО ТехноПром', 'Сервер Dell PowerEdge R740'),
    (6, 4, 'ВД-006', '2023-10-19', 'Автомобиль гр. Козлова', 'USB Flash-накопитель'),
    (7, 5, 'ВД-007', '2023-11-06', 'Место жительства гр. Новикова', 'Персональный компьютер'),
    (8, 6, 'ВД-008', '2023-11-21', 'Офис ИП Иванов, каб. 12', 'МФУ с встроенным HDD');

--- storage devices
INSERT INTO storage_devices (device_id, evidence_id, device_type, serial_number, brand, model, capacity_gb) VALUES
    (1, 1, 'Laptop', 'SN-LT-20231001', 'Lenovo', 'ThinkPad T490', 512.00),
    (2, 2, 'HDD', 'SN-HD-20231002', 'Seagate', 'Barracuda 2TB', 2000.00),
    (3, 3, 'Smartphone', 'SN-SP-20231003', 'Apple', 'iPhone 13 Pro', 256.00),
    (4, 4, 'Tablet', 'SN-TB-20231004', 'Apple', 'iPad Air 5', 64.00),
    (5, 5, 'Server HDD', 'SN-SV-20231005', 'Samsung', 'PM9A3 3.84TB', 3840.00),
    (6, 6, 'USB Flash', 'SN-UF-20231006', 'Kingston', 'DataTraveler 128GB', 128.00),
    (7, 7, 'SSD', 'SN-SD-20231007', 'Samsung', '970 EVO Plus', 1000.00),
    (8, 7, 'HDD', 'SN-HD-20231008', 'WD', 'Blue 1TB', 1000.00),
    (9, 8, 'MFP HDD', 'SN-MF-20231009', 'Toshiba', 'DT01ACA100', 1000.00),
    (10, 1, 'SD Card', 'SN-SD-20231010', 'SanDisk', 'Ultra 64GB', 64.00);

--- digital files
INSERT INTO digital_files (file_id, device_id, file_name, file_hash_sha256, file_size_bytes, file_type, discovery_date) VALUES
    (1, 1, 'secret_data.xlsx', 'a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2', 15360, 'document', '2023-09-03'),
    (2, 1, 'passwords.kdbx', 'b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3', 204800, 'database', '2023-09-03'),
    (3, 2, 'backup_2023.zip', 'c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4', 524288000, 'archive', '2023-09-04'),
    (4, 3, 'chat_history.db', 'd4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5', 2048000, 'database', '2023-09-17'),
    (5, 3, 'photo_001.jpg', 'e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6', 3145728, 'image', '2023-09-17'),
    (6, 4, 'invoice_scan.pdf', 'f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1', 1048576, 'document', '2023-09-18'),
    (7, 5, 'server_log_2023.log', 'a7b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8', 104857600, 'log', '2023-10-04'),
    (8, 5, 'users_dump.sql', 'b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9', 52428800, 'database', '2023-10-04'),
    (9, 6, 'commercial_secret.docx', 'c9d0e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0', 2097152, 'document', '2023-10-20'),
    (10, 7, 'threats_emails.mbox', 'd0e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0e1', 8388608, 'email', '2023-11-07'),
    (11, 7, 'deleted_photos.tar', 'e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0e1f2', 104857600, 'archive', '2023-11-07'),
    (12, 8, 'system_backup.img', 'f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0e1f2a3', 536870912, 'image', '2023-11-08'),
    (13, 9, 'scan_job_0042.tiff', 'a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0e1f2a3b4', 26214400, 'image', '2023-11-22'),
    (14, 10, 'camera_raw.cr2', 'b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5', 7340032, 'image', '2023-09-03');
    
--- examinations
INSERT INTO examinations (examination_id, case_id, employee_id, appointment_date, start_date, end_date, conclusion) VALUES
    (1, 1, 1, '2023-09-05', '2023-09-06', '2023-09-20', 'Обнаружены следы несанкционированного копирования файлов'),
    (2, 2, 2, '2023-09-18', '2023-09-19', '2023-10-10', 'В переписке подтверждена причастность к фишинговой рассылке'),
    (3, 3, 3, '2023-10-05', '2023-10-06', '2023-10-25', 'В логах сервера зафиксирован несанкционированный доступ'),
    (4, 4, 4, '2023-10-20', '2023-10-21', '2023-11-05', 'На USB-накопителе обнаружены документы, содержащие коммерческую тайну'),
    (5, 5, 5, '2023-11-08', '2023-11-09', NULL, NULL),
    (6, 6, 1, '2023-11-23', '2023-11-24', '2023-12-10', 'Документы на МФУ имеют признаки подделки (несоответствие метаданных)');

--- analasys_objects
INSERT INTO analysis_objects (examination_id, file_id, analysis_result, analysis_date) VALUES
    (1, 1, 'Модифицирован', '2023-09-10'),
    (1, 2, 'Без изменений', '2023-09-10'),
    (1, 3, 'Модифицирован', '2023-09-12'),
    (2, 4, 'Без изменений', '2023-09-25'),
    (2, 5, 'Скрыт', '2023-09-25'),
    (2, 6, 'Без изменений', '2023-09-28'),
    (3, 7, 'Модифицирован', '2023-10-15'),
    (3, 8, 'Удалён', '2023-10-15'),
    (4, 9, 'Зашифрован', '2023-10-28'),
    (5, 10, 'Без изменений', '2023-11-15'),
    (5, 11, 'Удалён', '2023-11-18'),
    (5, 12, 'Модифицирован', '2023-11-20'),
    (6, 13, 'Подозрительный', '2023-12-01'),
    (1, 14, 'Без изменений', '2023-09-11');

--- проверка вставки
SELECT 'cases' AS tbl, count(*) FROM cases
UNION ALL SELECT 'employees', count(*) FROM employees
UNION ALL SELECT 'evidence', count(*) FROM evidence
UNION ALL SELECT 'storage_devices', count(*) FROM storage_devices
UNION ALL SELECT 'digital_files', count(*) FROM digital_files
UNION ALL SELECT 'examinations', count(*) FROM examinations
UNION ALL SELECT 'analysis_objects', count(*) FROM analysis_objects;