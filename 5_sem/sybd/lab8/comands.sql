-- Транзакция
BEGIN;

UPDATE cases SET status = 'Экспертиза' WHERE case_id = 3;
SAVEPOINT sp1;

INSERT INTO evidence (evidence_id, case_id, evidence_number, seizure_date, seizure_location, description)
VALUES (100, 3, 'ВД-ТРАНЗ-100', '2024-01-10', 'Тестовое место', 'Вещдок для проверки транзакции');
SAVEPOINT sp2;

INSERT INTO examinations (examination_id, case_id, employee_id, appointment_date)
VALUES (100, 3, 1, '2024-01-11');

ROLLBACK TO SAVEPOINT sp2;

COMMIT;

-- Триггер 1. Аудит изменения статуса дела (AFTER UPDATE)
CREATE OR REPLACE FUNCTION fn_log_case_status_change()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.status <> OLD.status THEN
        RAISE NOTICE 'Аудит: Статус дела % изменен с "%" на "%"', 
                     NEW.case_number, OLD.status, NEW.status;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_case_status_audit
AFTER UPDATE ON cases
FOR EACH ROW
EXECUTE FUNCTION fn_log_case_status_change();

UPDATE cases SET status = 'В работе' WHERE case_id = 1;

-- Триггер 2. Защита хеша файла от изменения (BEFORE UPDATE)
CREATE OR REPLACE FUNCTION fn_protect_file_hash()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.file_hash_sha256 <> OLD.file_hash_sha256 THEN
        RAISE EXCEPTION 'Изменение хеша файла % запрещено политикой безопасности!', NEW.file_name;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_protect_hash
BEFORE UPDATE ON digital_files
FOR EACH ROW
EXECUTE FUNCTION fn_protect_file_hash();

UPDATE digital_files 
SET file_hash_sha256 = '0000000000000000000000000000000000000000000000000000000000000000' 
WHERE file_id = 1;

-- Триггер 3. Защита сотрудника от удаления (BEFORE DELETE)
CREATE OR REPLACE FUNCTION fn_protect_employee_delete()
RETURNS TRIGGER AS $$
BEGIN
    IF EXISTS (SELECT 1 FROM examinations WHERE employee_id = OLD.employee_id) THEN
        RAISE EXCEPTION 'Нельзя удалить сотрудника %, так как он назначен на экспертизы', OLD.full_name;
    END IF;
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_protect_employee
BEFORE DELETE ON employees
FOR EACH ROW
EXECUTE FUNCTION fn_protect_employee_delete();

DELETE FROM employees WHERE employee_id = 1;

-- Триггер событий 1. Логирование создания таблиц
CREATE OR REPLACE FUNCTION fn_event_log_create()
RETURNS EVENT_TRIGGER AS $$
BEGIN
    RAISE NOTICE 'Событие DDL: Выполнена команда %, создана новая таблица', tg_tag;
END;
$$ LANGUAGE plpgsql;

CREATE EVENT TRIGGER evt_log_create
ON ddl_command_end
WHEN TAG IN ('CREATE TABLE')
EXECUTE FUNCTION fn_event_log_create();

CREATE TABLE temp_table_for_event (id INT);
DROP TABLE temp_table_for_event;

-- Триггер событий 2. Блокировка удаления таблиц
CREATE OR REPLACE FUNCTION fn_event_block_drop()
RETURNS EVENT_TRIGGER AS $$
BEGIN
    RAISE EXCEPTION 'Политика безопасности: Удаление таблиц (команда %) запрещено!', tg_tag;
END;
$$ LANGUAGE plpgsql;

CREATE EVENT TRIGGER evt_block_drop
ON sql_drop
WHEN TAG IN ('DROP TABLE')
EXECUTE FUNCTION fn_event_block_drop();

CREATE TABLE temp_table_to_drop (id INT);
DROP TABLE temp_table_to_drop;

DROP EVENT TRIGGER IF EXISTS evt_block_drop;
DROP EVENT TRIGGER IF EXISTS evt_log_create;