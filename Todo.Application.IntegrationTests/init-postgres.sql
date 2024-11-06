create table todoitems (
  Id uuid not null primary key,
  Title text not null,
  CreatedOnUtc timestamptz not null,
  CompletedOnUtc timestamptz
);

insert into	todoitems (Id, Title, CreatedOnUtc) values
('7db5edbf-ddd5-416d-9724-16600672733d', 'Postgres 1', '2024-10-1'),
('2631a5b0-4779-4a8a-9caf-25792fe37c17', 'Postgres 2', '2024-10-2'),
('5dcafa62-3569-41c1-bfe5-ac2e7ec1b2b0', 'Postgres 3', '2024-10-3'),
('27c5536d-f91e-45f6-8a72-1493322efe9b', 'Postgres 4', '2024-10-4');

create table outbox_messages (
  Id uuid not null primary key,
  OccurredOnUtc timestamptz not null,
  MessageType text not null,
  content jsonb not null,
  ProcessedOnUtc timestamptz null,
  MessageError text null);