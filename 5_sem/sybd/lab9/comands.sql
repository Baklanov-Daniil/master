-- Задача 1. Процедура с явным курсором и циклом (вывод уведомлений)
-- Цель: Пройтись по всем делам и вывести их статусы через RAISE NOTICE, используя классическую схему: DECLARE -> OPEN -> FETCH -> CLOSE.
CREATE OR REPLACE PROCEDURE proc_list_cases()
LANGUAGE plpgsql
AS $$
DECLARE
    cur_cases CURSOR FOR 
        SELECT case_number, status FROM cases;
    v_rec RECORD;
BEGIN
    OPEN cur_cases;
    
    LOOP
        FETCH cur_cases INTO v_rec;
        EXIT WHEN NOT FOUND;
        
        RAISE NOTICE 'Дело: %, Статус: %', v_rec.case_number, v_rec.status;
    END LOOP;

    CLOSE cur_cases;
END;
$$;

CALL proc_list_cases();

-- Задача 2. Функция, возвращающая таблицу через курсор
-- Цель: Найти все файлы, которые были признаны «подозрительными» (Модифицирован, Удалён, Подозрительный) в ходе экспертиз, и вернуть их списком.
CREATE OR REPLACE FUNCTION func_suspicious_files()
RETURNS TABLE (
    f_name VARCHAR, 
    f_result VARCHAR
)
LANGUAGE plpgsql
AS $$
DECLARE
    cur_files CURSOR FOR
        SELECT df.file_name, ao.analysis_result
        FROM digital_files df
        JOIN analysis_objects ao ON df.file_id = ao.file_id
        WHERE ao.analysis_result IN ('Модифицирован', 'Удалён', 'Подозрительный');
    v_rec RECORD;
BEGIN
    OPEN cur_files;
    
    LOOP
        FETCH cur_files INTO v_rec;
        EXIT WHEN NOT FOUND;
        
        f_name := v_rec.file_name;
        f_result := v_rec.analysis_result;
        
        RETURN NEXT; 
    END LOOP;

    CLOSE cur_files;
END;
$$;
 
SELECT * FROM func_suspicious_files();

-- Задача 3. Функция, возвращающая refcursor (ссылку на курсор)
-- Цель: Получить список экспертиз по конкретному делу. Курсор создается внутри функции, а выборка из него (FETCH) делается уже снаружи.
CREATE OR REPLACE FUNCTION func_get_exams(p_case_id INTEGER)
RETURNS refcursor
LANGUAGE plpgsql
AS $$
DECLARE
    ref refcursor := 'my_exams_cursor';
BEGIN
    OPEN ref FOR 
        SELECT examination_id, appointment_date, conclusion
        FROM examinations 
        WHERE case_id = p_case_id;
        
    RETURN ref;
END;
$$;

SELECT func_get_exams(1);
FETCH ALL FROM my_exams_cursor;