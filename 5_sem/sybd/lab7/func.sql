-- Процедура 1: Добавление нового дела с проверкой уникальности
CREATE OR REPLACE PROCEDURE proc_add_case(
    p_case_number   VARCHAR(30),
    p_reg_date      DATE,
    p_status        VARCHAR(20),
    p_description   TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_new_case_id INTEGER;
BEGIN
    IF EXISTS (SELECT 1 FROM cases WHERE case_number = p_case_number) THEN
        RAISE NOTICE 'Дело с номером % уже существует. Вставка отменена.', p_case_number;
        RETURN;
    END IF;
	
    SELECT COALESCE(MAX(case_id), 0) + 1 INTO v_new_case_id FROM cases;

    INSERT INTO cases (case_id, case_number, registration_date, status, description)
    VALUES (v_new_case_id, p_case_number, p_reg_date, p_status, p_description);

    RAISE NOTICE 'Дело % успешно добавлено с ID = %.', p_case_number, v_new_case_id;
END;
$$;

CALL proc_add_case('Д-200/2024', '2024-01-15', 'Открыто', 'Тестовое дело для проверки процедуры');

-- Процедура 2: Назначение экспертизы 
CREATE OR REPLACE PROCEDURE proc_assign_examination(
    p_case_id         INTEGER,
    p_employee_id     INTEGER,
    p_appointment_date DATE
)
LANGUAGE plpgsql
AS $$
BEGIN
    -- Проверяем существование дела и сотрудника
    IF NOT EXISTS (SELECT 1 FROM cases WHERE case_id = p_case_id) THEN
        RAISE EXCEPTION 'Дело с ID = % не найдено.', p_case_id;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM employees WHERE employee_id = p_employee_id) THEN
        RAISE EXCEPTION 'Сотрудник с ID = % не найден.', p_employee_id;
    END IF;

    -- Вставляем экспертизу
    INSERT INTO examinations (case_id, employee_id, appointment_date)
    VALUES (p_case_id, p_employee_id, p_appointment_date);

    RAISE NOTICE 'Экспертиза назначена: дело %, сотрудник %.', p_case_id, p_employee_id;
END;
$$;

CALL proc_assign_examination(1, 1, '2024-01-20');

-- Процедура 3: Закрытие дела
CREATE OR REPLACE PROCEDURE proc_close_case(p_case_id INTEGER)
LANGUAGE plpgsql
AS $$
DECLARE
    v_unfinished INTEGER;
BEGIN
    -- Проверяем наличие незавершённых экспертиз
    SELECT COUNT(*) INTO v_unfinished
    FROM examinations
    WHERE case_id = p_case_id AND end_date IS NULL;

    IF v_unfinished > 0 THEN
        RAISE EXCEPTION 'Нельзя закрыть дело %: есть % незавершённых экспертиз.',
                        p_case_id, v_unfinished;
    END IF;

    -- Закрываем дело
    UPDATE cases SET status = 'Закрыто' WHERE case_id = p_case_id;

    RAISE NOTICE 'Дело % закрыто.', p_case_id;
END;
$$;

-- Функция 1: Статистика по делам
CREATE OR REPLACE FUNCTION func_case_statistics(p_status VARCHAR)
RETURNS TABLE (
    case_number     VARCHAR(30),
    evidence_count  BIGINT,
    exam_count      BIGINT,
    file_count      BIGINT
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT
        c.case_number,
        COUNT(DISTINCT e.evidence_id)::BIGINT,
        COUNT(DISTINCT ex.examination_id)::BIGINT,
        COUNT(DISTINCT ao.file_id)::BIGINT
    FROM cases c
    LEFT JOIN evidence e          ON c.case_id = e.case_id
    LEFT JOIN examinations ex     ON c.case_id = ex.case_id
    LEFT JOIN analysis_objects ao ON ex.examination_id = ao.examination_id
    WHERE c.status = p_status
    GROUP BY c.case_id, c.case_number;
END;
$$;

SELECT * FROM func_case_statistics('Открыто');

-- Функция 2 : Нагрузка эксперта
CREATE OR REPLACE FUNCTION func_expert_workload(p_employee_id INTEGER)
RETURNS INTEGER
LANGUAGE sql
AS $$
    SELECT COUNT(*)::INTEGER
    FROM examinations
    WHERE employee_id = p_employee_id;
$$;

SELECT func_expert_workload(1) AS exam_count;