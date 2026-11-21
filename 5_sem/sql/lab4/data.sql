-- Добавляем операторов
INSERT INTO bike_operators (name) VALUES ('Velobike'), ('CityRide');

-- Добавляем станции (Points)
INSERT INTO bike_stations (name, capacity, operator_id, geometry) VALUES
('Station A', 20, 1, ST_GeomFromText('POINT(397000 6096000)', 32643)),
('Station B', 15, 1, ST_GeomFromText('POINT(397500 6096500)', 32643)),
('Station C', 10, 2, ST_GeomFromText('POINT(398000 6097000)', 32643)),
('Remote Station', 5, 2, ST_GeomFromText('POINT(400500 6099500)', 32643));

-- Добавляем остановки (Points)
INSERT INTO transit_stops (name, geometry) VALUES
('Stop Near A', ST_GeomFromText('POINT(397100 6096100)', 32643)),
('Stop Far', ST_GeomFromText('POINT(399000 6098000)', 32643)),
('Stop Near B', ST_GeomFromText('POINT(397600 6096600)', 32643));

-- Добавляем велодорожки (LineStrings)
INSERT INTO bike_paths (name, path_type, geometry) VALUES
('Main Path', 'protected', ST_GeomFromText('LINESTRING(396000 6095000, 398000 6097000, 399000 6098000)', 32643)),
('Side Path', 'shared', ST_GeomFromText('LINESTRING(397000 6096000, 397500 6096500)', 32643));
('Crossing Line', 'protected', ST_GeomFromText('LINESTRING(397500 6095000, 397500 6098000)', 32643)),
('Toucher Start', 'shared', ST_GeomFromText('LINESTRING(396500 6095500, 397000 6096000)', 32643)),
('Parallel Line', 'shared', ST_GeomFromText('LINESTRING(396100 6095100, 398100 6097100)', 32643));

-- Добавляем жилые кварталы (Polygons)
INSERT INTO residential_blocks (code, geometry) VALUES
('Block 1', ST_GeomFromText('POLYGON((396500 6095500, 397500 6095500, 397500 6096500, 396500 6096500, 396500 6095500))', 32643)),
('Block 2', ST_GeomFromText('POLYGON((397500 6096500, 398500 6096500, 398500 6097500, 397500 6097500, 397500 6096500))', 32643)),
('Block Empty', ST_GeomFromText('POLYGON((399000 6098000, 400000 6098000, 400000 6099000, 399000 6099000, 399000 6098000))', 32643));

-- Добавляем зоны обслуживания (Polygons)
INSERT INTO service_areas (name, operator_id, geometry) VALUES
('Zone Velobike North', 1, ST_GeomFromText('POLYGON((396000 6095000, 398000 6095000, 398000 6097000, 396000 6097000, 396000 6095000))', 32643)),
('Zone CityRide South', 2, ST_GeomFromText('POLYGON((397000 6096000, 399000 6096000, 399000 6098000, 397000 6098000, 397000 6096000))', 32643));