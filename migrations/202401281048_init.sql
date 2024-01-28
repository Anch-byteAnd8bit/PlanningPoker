CREATE TABLE project (
    id SERIAL PRIMARY KEY,
    start_date DATE NOT NULL,
    name VARCHAR(255) NOT NULL,
    description TEXT
);

CREATE TABLE sprint (
    id SERIAL PRIMARY KEY,
    project_id INTEGER REFERENCES project(id),
    iteration_number INTEGER NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL
);

CREATE TABLE task (
    id SERIAL PRIMARY KEY,
    sprint_id INTEGER REFERENCES sprint(id),
    name VARCHAR(255) NOT NULL,
    description TEXT
);