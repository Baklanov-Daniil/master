```
CREATE DATABASE digital_forensics
WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Russian_Russia.1251'
    LC_CTYPE = 'Russian_Russia.1251'
    TABLESPACE = pg_default;
```

```
-- 1. Дела
CREATE TABLE cases (
    case_id SERIAL PRIMARY KEY,
    case_number VARCHAR(30) UNIQUE NOT NULL,
    registration_date DATE NOT NULL,
    status VARCHAR(20) NOT NULL,
    description TEXT
);

-- 2. Сотрудники
CREATE TABLE employees (
    employee_id SERIAL PRIMARY KEY,
    full_name VARCHAR(150) NOT NULL,
    position VARCHAR(50) NOT NULL,
    specialization VARCHAR(100),
    hire_date DATE NOT NULL
);

-- 3. Вещественные доказательства
CREATE TABLE evidence (
    evidence_id SERIAL PRIMARY KEY,
    case_id INTEGER NOT NULL,
    evidence_number VARCHAR(30) UNIQUE NOT NULL,
    seizure_date DATE NOT NULL,
    seizure_location VARCHAR(200) NOT NULL,
    description TEXT
);

-- 4. Устройства хранения
CREATE TABLE storage_devices (
    device_id SERIAL PRIMARY KEY,
    evidence_id INTEGER NOT NULL,
    device_type VARCHAR(30) NOT NULL,
    serial_number VARCHAR(50) UNIQUE NOT NULL,
    brand VARCHAR(50),
    model VARCHAR(50),
    capacity_gb NUMERIC(10,2)
);

-- 5. Цифровые файлы
CREATE TABLE digital_files (
    file_id SERIAL PRIMARY KEY,
    device_id INTEGER NOT NULL,
    file_name VARCHAR(255) NOT NULL,
    file_hash_sha256 CHAR(64) UNIQUE NOT NULL,
    file_size_bytes BIGINT NOT NULL,
    file_type VARCHAR(30) NOT NULL,
    discovery_date DATE NOT NULL
);

-- 6. Экспертизы
CREATE TABLE examinations (
    examination_id SERIAL PRIMARY KEY,
    case_id INTEGER NOT NULL,
    employee_id INTEGER NOT NULL,
    appointment_date DATE NOT NULL,
    start_date DATE,
    end_date DATE,
    conclusion TEXT,
    CONSTRAINT chk_dates CHECK (start_date IS NULL OR appointment_date <= start_date)
);

-- 7. Объекты анализа (связующая таблица M:N)
CREATE TABLE analysis_objects (
    examination_id INTEGER NOT NULL,
    file_id INTEGER NOT NULL,
    analysis_result VARCHAR(100) NOT NULL,
    analysis_date DATE NOT NULL,
    PRIMARY KEY (examination_id, file_id)
);
```

```
-- Связь: вещдок → дело
ALTER TABLE evidence 
    ADD FOREIGN KEY (case_id) REFERENCES cases(case_id);

-- Связь: устройство → вещдок
ALTER TABLE storage_devices 
    ADD FOREIGN KEY (evidence_id) REFERENCES evidence(evidence_id);

-- Связь: файл → устройство
ALTER TABLE digital_files 
    ADD FOREIGN KEY (device_id) REFERENCES storage_devices(device_id);

-- Связь: экспертиза → дело
ALTER TABLE examinations 
    ADD FOREIGN KEY (case_id) REFERENCES cases(case_id);

-- Связь: экспертиза → сотрудник
ALTER TABLE examinations 
    ADD FOREIGN KEY (employee_id) REFERENCES employees(employee_id);

-- Связь: объект анализа → экспертиза
ALTER TABLE analysis_objects 
    ADD FOREIGN KEY (examination_id) REFERENCES examinations(examination_id);

-- Связь: объект анализа → файл
ALTER TABLE analysis_objects 
    ADD FOREIGN KEY (file_id) REFERENCES digital_files(file_id);

-- Ограничение: дата начала экспертизы не раньше даты назначения
ALTER TABLE examinations 
    ADD CHECK (start_date IS NULL OR appointment_date <= start_date);
```