CREATE TABLE table_roles (
    id  SERIAL NOT NULL PRIMARY KEY,
    role TEXT NOT NULL
);

CREATE TABLE table_accounts (
    id SERIAL NOT NULL PRIMARY KEY,
    login TEXT NOT NULL
        CONSTRAINT constraint_unique_login UNIQUE,
    password TEXT NOT NULL
        CONSTRAINT constraint_check_password CHECK ( length(password) > 8 ),
    id_role INT NOT NULL,
    CONSTRAINT constraint_foreign_key_id_role
        FOREIGN KEY (id_role) REFERENCES table_roles(id)
            ON UPDATE NO ACTION
            ON DELETE NO ACTION
);

CREATE TABLE table_users (
    id SERIAL NOT NULL PRIMARY KEY,
    last_name TEXT NOT NULL,
    first_name TEXT NOT NULL,
    email TEXT NOT NULL,
    CONSTRAINT constraint_foreign_key_id
        FOREIGN KEY (id) REFERENCES table_accounts(id)
            ON UPDATE NO ACTION
            ON DELETE NO ACTION
);

CREATE TABLE table_projects (
    id SERIAL NOT NULL PRIMARY KEY,
    title TEXT NOT NULL,
    description TEXT NULL
);

CREATE TABLE table_projects_users (
    id SERIAL NOT NULL PRIMARY KEY,
    id_user INT NOT NULL,
    id_project INT NOT NULL,
    CONSTRAINT constraint_foreign_key_id_user
        FOREIGN KEY (id_user) REFERENCES table_users(id)
            ON UPDATE NO ACTION
            ON DELETE NO ACTION,
    CONSTRAINT constraint_foreign_key_id_project
        FOREIGN KEY (id_project) REFERENCES table_projects(id)
            ON UPDATE NO ACTION
            ON DELETE NO ACTION
);