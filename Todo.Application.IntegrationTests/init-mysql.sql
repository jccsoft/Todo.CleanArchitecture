USE tododb;

CREATE TABLE todoitems (
  id char(36) NOT NULL PRIMARY KEY,
  title varchar(50) NOT NULL,
  created_on_utc datetime,
  completed_on_utc datetime
);

INSERT INTO todoitems (id, title, created_on_utc) VALUES
('7db5edbf-ddd5-416d-9724-16600672733d', 'MySql 1', '2024-10-1'),
('2631a5b0-4779-4a8a-9caf-25792fe37c17', 'MySql 2', '2024-10-2'),
('5dcafa62-3569-41c1-bfe5-ac2e7ec1b2b0', 'MySql 3', '2024-10-3'),
('27c5536d-f91e-45f6-8a72-1493322efe9b', 'MySql 4', '2024-10-4');


CREATE TABLE outbox_messages (
  id CHAR(36) NOT NULL,
  occurred_on_utc DATETIME NOT NULL,
  message_type VARCHAR(50) NOT NULL,
  content JSON NOT NULL,
  processed_on_utc DATETIME NULL,
  message_error VARCHAR(150) NULL,
  PRIMARY KEY (id));