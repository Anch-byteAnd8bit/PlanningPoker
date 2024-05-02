BEGIN;

CREATE SCHEMA poker

    CREATE TABLE projects
    (
        id          SERIAL PRIMARY KEY,
        started_at  DATE         NOT NULL,
        name        VARCHAR(255) NOT NULL,
        description TEXT,
        chat_id     INTEGER
    )

    CREATE TABLE members
    (
        id      SERIAL PRIMARY KEY,
        chat_id INTEGER NOT NULL
    )

    CREATE TABLE sprints
    (
        id          SERIAL PRIMARY KEY,
        project_id  INTEGER REFERENCES projects (id),
        iteration   INTEGER NOT NULL,
        started_at  DATE    NOT NULL,
        finished_at DATE    NOT NULL
    )

    CREATE TABLE sprint_tasks
    (
        id          SERIAL PRIMARY KEY,
        sprint_id   INTEGER REFERENCES sprints (id),
        name        VARCHAR(255) NOT NULL,
        description TEXT
    );

COMMIT;
