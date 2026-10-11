-- 1. Конкатенация: добавить элемент (для строк нужен ARRAY[...])
UPDATE suspects
SET aliases = aliases || ARRAY['dima_smirnov']
WHERE suspect_id = 1;

-- 2. array_append: добавить в конец (массив, элемент)
UPDATE suspects
SET aliases = array_append(aliases, 'artem_k')
WHERE suspect_id = 2;

-- 3. array_prepend: добавить в начало (элемент, массив!) — порядок обратный
UPDATE suspects
SET aliases = array_prepend('kovalev_ai', aliases)
WHERE suspect_id = 3;

-- 4. array_remove: удалить элемент
UPDATE suspects
SET aliases = array_remove(aliases, 'Гриша')
WHERE suspect_id = 4;

-- 5. Обращение по индексу: заменить 2-й элемент
UPDATE suspects
SET aliases[2] = 'index_new'
WHERE suspect_id = 5;

-- 6. Срез массива: заменить диапазон элементов
UPDATE suspects
SET aliases[1:2] = ARRAY['slice_new_1','slice_new_2']
WHERE suspect_id = 6;

-- Выборки по массиву aliases 
SELECT 1 AS "№", 'array_position: индекс клички smirnoff_d' AS "Операция",
       (SELECT array_position(aliases, 'smirnoff_d')::text
        FROM suspects WHERE suspect_id = 1) AS "Результат"
UNION ALL
SELECT 2, '@> содержит все элементы (Смирн + dima_smirnov)',
       (SELECT string_agg(full_name, ', ')
        FROM suspects
        WHERE aliases @> ARRAY['Смирн','dima_smirnov']::varchar[])
UNION ALL
SELECT 3, '&& есть общий элемент (kovalev_ai или artem_k)',
       (SELECT string_agg(full_name, ', ')
        FROM suspects
        WHERE aliases && ARRAY['kovalev_ai','artem_k']::varchar[])
UNION ALL
SELECT 4, 'NOT && кто НЕ имеет Гриша/admin_root',
       (SELECT string_agg(full_name, ', ')
        FROM suspects
        WHERE NOT (aliases && ARRAY['Гриша','admin_root']::varchar[]))
UNION ALL
SELECT 5, 'unnest: развёртка массива Смирнова в строку',
       (SELECT string_agg(x, ', ')
        FROM unnest((SELECT aliases FROM suspects WHERE suspect_id = 1)) x)
ORDER BY 1;

--- json

SELECT emp_ID FROM expert_profile
WHERE methods->'methods' ? 'хеширование';

SELECT emp_ID, methods->'methods' AS methods_list
FROM expert_profile;

SELECT count(*) FROM expert_profile WHERE methods ? 'method';
SELECT count(*) FROM expert_profile WHERE methods ? 'methods';

UPDATE expert_profile
SET methods = '{"methods": ["хеширование"]}'::jsonb
WHERE emp_ID = 2;

UPDATE expert_profile
SET methods = jsonb_set(methods, '{methods}', '["хеширование","анализ мобильных устройств"]')
WHERE emp_ID = 2;


SELECT 1 AS "№", 'JSON' AS "Раздел",
       'Перезапись объекта (метод = только хеширование)' AS "Операция",
       'expert_profile emp_ID=2' AS "Объект",
       ('{"methods":["хеширование"]}'::jsonb)::text AS "Результат"

UNION ALL
SELECT 2, 'JSON', 'jsonb_set (итог: хеширование + анализ моб. устройств)', 'expert_profile emp_ID=2',
        (SELECT methods::text FROM expert_profile WHERE emp_ID = 2)

UNION ALL
SELECT 3, 'JSON', 'Выборка владеющих методом "хеширование" (аналог ?)', 'expert_profile',
        (SELECT string_agg(emp_ID::text, ', ') FROM expert_profile
         WHERE EXISTS (SELECT 1 FROM jsonb_array_elements_text(methods->'methods') AS t(v)
                       WHERE t.v = 'хеширование'))

UNION ALL
SELECT 4, 'JSON', 'Обращение к ключу через -> (список методов)', 'expert_profile',
        (SELECT string_agg(emp_ID::text || ': ' || (methods->'methods')::text, ' | ')
         FROM expert_profile)

UNION ALL
SELECT 5, 'JSON', 'Проверка наличия ключа "method" (аналог ? + count)', 'expert_profile',
        (SELECT count(*)::text FROM expert_profile WHERE jsonb_exists(methods, 'method'))

UNION ALL
SELECT 6, 'JSON', 'Проверка наличия ключа "methods" (аналог ? + count)', 'expert_profile',
        (SELECT count(*)::text FROM expert_profile WHERE jsonb_exists(methods, 'methods'))


