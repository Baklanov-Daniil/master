-- Таблица подозреваемых (столбец aliases - массив)
CREATE TABLE suspects (
    suspect_id SERIAL PRIMARY KEY,
    case_id INTEGER NOT NULL,
    full_name VARCHAR(150) NOT NULL,
    aliases VARCHAR(100)[],  -- Массив псевдонимов
    status VARCHAR(30) NOT NULL,
    description TEXT,
    CONSTRAINT fk_suspects_case FOREIGN KEY (case_id) REFERENCES cases(case_id)
);

INSERT INTO suspects (suspect_id, case_id, full_name, aliases, status, description) VALUES
(1, 1, 'Смирнов Дмитрий Олегович',  '{"Смирн","Димон-хакер","smirnoff_d"}',          'Задержан',  'операция || ARRAY'),
(2, 2, 'Ковалёв Артём Игоревич',    '{"KovalArt","артёмка_228"}',                     'В розыске', 'операция array_append'),
(3, 3, 'Григорьев Павел Сергеевич', '{"Гриша","pavel_g","admin_root"}',               'Задержан',  'операция array_prepend'),
(4, 1, 'Тестовый Удаление',         '{"Гриша","delete_me"}',                          'Задержан',  'операция array_remove'),
(5, 2, 'Тестовый Индекс',           '{"index_old","index_target"}',                   'В розыске', 'операция aliases[2]'),
(6, 3, 'Тестовый Срез',             '{"slice_1","slice_2","slice_3"}',                'Задержан',  'операция срез [1:2]');

--- json

CREATE TABLE expert_profile (
    emp_ID      integer PRIMARY KEY REFERENCES employees(employee_id),
    methods     jsonb,  
    cases_count jsonb  
);

INSERT INTO expert_profile (emp_ID, methods, library, cases_count) VALUES
(1, '{"methods": ["хеширование", "восстановление данных"]}',
    '{"library": ["SWAT", "FTK"]}',
    '{"cases": 142}'),
(2, '{"methods": ["анализ мобильных устройств"]}',
    '{"library": ["Magnet AXIOM"]}',
    '{"cases": 87}');

Select * from expert_profile