USE tododb;

CREATE TABLE todoitems (
  Id char(36) NOT NULL PRIMARY KEY,
  Title varchar(50) NOT NULL,
  CreatedOnUtc datetime,
  CompletedOnUtc datetime
);

INSERT INTO todoitems (Id, Title, CreatedOnUtc) VALUES
('7db5edbf-ddd5-416d-9724-16600672733d', 'MySql 1', '2024-10-1'),
('2631a5b0-4779-4a8a-9caf-25792fe37c17', 'MySql 2', '2024-10-2'),
('5dcafa62-3569-41c1-bfe5-ac2e7ec1b2b0', 'MySql 3', '2024-10-3'),
('27c5536d-f91e-45f6-8a72-1493322efe9b', 'MySql 4', '2024-10-4');


CREATE TABLE outbox_messages (
  Id CHAR(36) NOT NULL,
  OccurredOnUtc DATETIME NOT NULL,
  MessageType VARCHAR(50) NOT NULL,
  Content JSON NOT NULL,
  ProcessedOnUtc DATETIME NULL,
  MessageError VARCHAR(150) NULL,
  PRIMARY KEY (Id));
