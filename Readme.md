# How to run
docker compose up --build -d
Docker will spin up 2 containers one for Db and one for app

docker ps to see runnign containers

## SQL scripts Users

INSERT INTO contacts.users (id, "name", email, email_confirmed, "password")
VALUES ('550e8400-e29b-41d4-a716-446655440000', 'Alice Smith', 'alice@example.com', false, 'password123');

INSERT INTO contacts.users (id, "name", email, email_confirmed, "password")
VALUES ('550e8400-e29b-41d4-a716-446655440001', 'Bob Johnson', 'bob@example.com', false, 'securePass456');

INSERT INTO contacts.users (id, "name", email, email_confirmed, "password")
VALUES ('550e8400-e29b-41d4-a716-446655440002', 'Charlie Brown', 'charlie@example.com', false, 'mySecret789');

INSERT INTO contacts.users (id, "name", email, email_confirmed, "password")
VALUES ('550e8400-e29b-41d4-a716-446655440003', 'Diana Prince', 'diana@example.com', false, 'WonderWoman123');

INSERT INTO contacts.users (id, "name", email, email_confirmed, "password")
VALUES ('550e8400-e29b-41d4-a716-446655440004', 'Ethan Hunt', 'ethan@example.com', false, 'MissionImpossible456');


## SQL scripts contacts

-- Contacts for Alice Smith
INSERT INTO contacts.contacts (id, "name", phone_number, user_id)
VALUES (gen_random_uuid(), 'Alice Contact 1', '123-456-7890', '550e8400-e29b-41d4-a716-446655440000'::uuid);

INSERT INTO contacts.contacts (id, "name", phone_number, user_id)
VALUES (gen_random_uuid(), 'Alice Contact 2', '098-765-4321', '550e8400-e29b-41d4-a716-446655440000'::uuid);

-- Contacts for Bob Johnson
INSERT INTO contacts.contacts (id, "name", phone_number, user_id)
VALUES (gen_random_uuid(), 'Bob Contact 1', '234-567-8901', '550e8400-e29b-41d4-a716-446655440001'::uuid);

INSERT INTO contacts.contacts (id, "name", phone_number, user_id)
VALUES (gen_random_uuid(), 'Bob Contact 2', '876-543-2109', '550e8400-e29b-41d4-a716-446655440001'::uuid);

-- Contacts for Charlie Brown
INSERT INTO contacts.contacts (id, "name", phone_number, user_id)
VALUES (gen_random_uuid(), 'Charlie Contact 1', '345-678-9012', '550e8400-e29b-41d4-a716-446655440002'::uuid);

INSERT INTO contacts.contacts (id, "name", phone_number, user_id)
VALUES (gen_random_uuid(), 'Charlie Contact 2', '765-432-1098', '550e8400-e29b-41d4-a716-446655440002'::uuid);

-- Contacts for Diana Prince
INSERT INTO contacts.contacts (id, "name", phone_number, user_id)
VALUES (gen_random_uuid(), 'Diana Contact 1', '456-789-0123', '550e8400-e29b-41d4-a716-446655440003'::uuid);

INSERT INTO contacts.contacts (id, "name", phone_number, user_id)
VALUES (gen_random_uuid(), 'Diana Contact 2', '654-321-0987', '550e8400-e29b-41d4-a716-446655440003'::uuid);

-- Contacts for Ethan Hunt
INSERT INTO contacts.contacts (id, "name", phone_number, user_id)
VALUES (gen_random_uuid(), 'Ethan Contact 1', '567-890-1234', '550e8400-e29b-41d4-a716-446655440004'::uuid);

INSERT INTO contacts.contacts (id, "name", phone_number, user_id)
VALUES (gen_random_uuid(), 'Ethan Contact 2', '543-210-9876', '550e8400-e29b-41d4-a716-446655440004'::uuid);

