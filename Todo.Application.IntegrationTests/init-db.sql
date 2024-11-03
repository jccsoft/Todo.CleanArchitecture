USE tododb;

DROP TABLE IF EXISTS todoitems;

CREATE TABLE todoitems (
  Id char(36) NOT NULL PRIMARY KEY,
  Title varchar(50) NOT NULL,
  IsCompleted tinyint(1) DEFAULT 0,
  CreatedOnUtc datetime,
  CompletedOnUtc datetime
);

INSERT INTO todoitems (Id, Title, CreatedOnUtc) VALUES
('7db5edbf-ddd5-416d-9724-16600672733d', 'Sample 1', '2024-10-1'),
('2631a5b0-4779-4a8a-9caf-25792fe37c17', 'Sample 2', '2024-10-2'),
('5dcafa62-3569-41c1-bfe5-ac2e7ec1b2b0', 'Sample 3', '2024-10-3'),
('27c5536d-f91e-45f6-8a72-1493322efe9b', 'Sample 4', '2024-10-4');


CREATE TABLE outbox_messages (
  Id CHAR(36) NOT NULL,
  OccurredOnUtc DATETIME NOT NULL,
  Type VARCHAR(50) NOT NULL,
  Content JSON NOT NULL,
  ProcessedOnUtc DATETIME NULL,
  Error VARCHAR(150) NULL,
  PRIMARY KEY (Id));
