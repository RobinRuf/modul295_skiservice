CREATE DATABASE skiservice_backend CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

CREATE TABLE customers (
    ID int NOT NULL AUTO_INCREMENT,
    Name varchar(255) NOT NULL,
    Email varchar(255) NOT NULL UNIQUE,
    Phone varchar(50),
    PRIMARY KEY (ID)
);

CREATE TABLE employees (
    ID int NOT NULL AUTO_INCREMENT,
    Username varchar(255) NOT NULL UNIQUE,
    Password varchar(255) NOT NULL,
    Name varchar(255) NOT NULL,
    loginAttempts INT DEFAULT 0,
    isLocked BOOLEAN DEFAULT FALSE;

    PRIMARY KEY (EmployeeID)
);

INSERT INTO employees (Username, Name, Password, loginAttempts, isLocked) VALUES
('employee1', 'Max Mustermann', SHA2('password', 256), 0, FALSE),
('employee2', 'Anna Schmidt', SHA2('password', 256), 0, FALSE),
('employee3', 'Felix Bauer', SHA2('password', 256), 0, FALSE),
('employee4', 'Laura Fischer', SHA2('password', 256), 0, FALSE),
('employee5', 'Niklas Weber', SHA2('password', 256), 0, FALSE),
('employee6', 'Julia Meyer', SHA2('password', 256), 0, FALSE),
('employee7', 'Leon Wagner', SHA2('password', 256), 0, FALSE),
('employee8', 'Sophie Becker', SHA2('password', 256), 0, FALSE),
('employee9', 'Tobias Hoffmann', SHA2('password', 256), 0, FALSE),
('employee10', 'Marie Schulz', SHA2('password', 256), 0, FALSE);

CREATE TABLE statuses (
    ID int NOT NULL AUTO_INCREMENT,
    Status varchar(50) NOT NULL UNIQUE,
    PRIMARY KEY (ID)
);

INSERT INTO statuses (ID, Status) VALUES
(1, 'Offen'),
(2, 'In Arbeit'),
(3, 'Abgeschlossen'),
(4, 'Gelöscht');

CREATE TABLE services (
    ID int NOT NULL AUTO_INCREMENT,
    ServiceType varchar(255) NOT NULL UNIQUE,
    PRIMARY KEY (ID)
);

INSERT INTO services (ID, ServiceType) VALUES
(1, 'Kleiner Service'),
(2, 'Grosser Service'),
(3, 'Rennski Service'),
(4, 'Bindung montieren und einstellen'),
(5, 'Fell zuschneiden pro Paar'),
(6, 'Heisswachsen');

CREATE TABLE priorities (
    ID int NOT NULL AUTO_INCREMENT,
    Priority varchar(50) NOT NULL UNIQUE,
    PRIMARY KEY (ID)
);

INSERT INTO priorities (ID, Priority) VALUES
(1, 'Tief'),
(2, 'Standard'),
(3, 'Express');

CREATE TABLE serviceorders (
    OrderID int NOT NULL AUTO_INCREMENT,
    EmployeeID int,
    CustomerID int NOT NULL,
    ServiceType int NOT NULL,
    Priority int NOT NULL,
    Status int NOT NULL DEFAULT 1,
    CreationDate datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    StartDate datetime,
    CompletionDate datetime,
    Comment text,
    Sum varchar(50),
    PRIMARY KEY (OrderID),
    FOREIGN KEY (EmployeeID) REFERENCES employees(ID),
    FOREIGN KEY (CustomerID) REFERENCES customers(ID),
    FOREIGN KEY (ServiceType) REFERENCES services(ID),
    FOREIGN KEY (Priority) REFERENCES priorities(ID),
    FOREIGN KEY (Status) REFERENCES statuses(ID)
);

CREATE USER 'api'@'localhost' IDENTIFIED BY 'h82BFS9832UGA3g0bZSDFUgkr9abs8BCisd8q';

GRANT SELECT, INSERT, UPDATE ON skiservice_backend.customers TO 'api'@'localhost';

GRANT SELECT, INSERT, UPDATE ON skiservice_backend.serviceorders TO 'api'@'localhost';

GRANT SELECT, UPDATE ON skiservice_backend.employees TO 'api'@'localhost';

GRANT SELECT ON skiservice_backend.priorities TO 'api'@'localhost';

GRANT SELECT ON skiservice_backend.services TO 'api'@'localhost';

GRANT SELECT ON skiservice_backend.statuses TO 'api'@'localhost';