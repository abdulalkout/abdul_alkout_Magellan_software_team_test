<!-- Put the PostgreSQL script of Part 1 here. -->

CREATE DATABASE Part;

CREATE TABLE item (
id SERIAL PRIMARY KEY,
item_name VARCHAR(50) NOT NULL,
parent_item INTEGER REFERENCES item(id),
cost INTEGER NOT NULL,
req_date DATE NOT NULL
);

INSERT INTO item (item_name, parent_item, cost, req_date) VALUES
('Item1', null, 500, '2024-02-20'),
('Sub1', 1, 200, '2024-02-10'),
('Sub2', 1, 300, '2024-01-05'),
('Sub3', 2, 300, '2024-01-02'),
('Sub4', 2, 400, '2024-01-02'),
('Item2', null, 600, '2024-03-15'),
('Sub1', 6, 200, '2024-02-25');
