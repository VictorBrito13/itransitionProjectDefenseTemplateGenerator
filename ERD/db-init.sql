DROP DATABASE IF EXISTS itransition_template_manager;

CREATE DATABASE IF NOT EXISTS itransition_template_manager;

USE itransition_template_manager;

-- =========================== Inserttions
-- Topics
INSERT INTO topics (name) VALUES ("Education");

-- Users
INSERT INTO users (username, email, password) VALUES
("test1","test1@test.com", SHA2("1234", 256)),
("test2","test2@test.com", SHA2("1234", 256)),
("test3","test3@test.com", SHA2("1234", 256)),
("test4","test4@test.com", SHA2("1234", 256)),
("test5","test5@test.com", SHA2("1234", 256)),
("test6","test6@test.com", SHA2("1234", 256));

-- =========================== Full text indexes
-- Tempalte
CREATE FULLTEXT INDEX idx_fulltext ON users(Username, Email);