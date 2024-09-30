INSERT INTO contacts.users
(id, "name", email, email_confirmed, "password")
VALUES
('5bb4bd9d-e688-4379-a77b-7972830c067e', 'John Doe', 'john.doe@example.com', true, '$2a$12$5THvcYal/2sG5JC0/4zZpuqBG31CMDvqAmZY7RADLeyCbnBJgIJC2'),
(gen_random_uuid(), 'Jane Smith', 'jane.smith@example.com', true, '$2a$12$5THvcYal/2sG5JC0/4zZpuqBG31CMDvqAmZY7RADLeyCbnBJgIJC2'),
(gen_random_uuid(), 'Michael Brown', 'michael.brown@example.com', true, '$2a$12$5THvcYal/2sG5JC0/4zZpuqBG31CMDvqAmZY7RADLeyCbnBJgIJC2'),
('78d4466c-6c23-465a-8e89-be4cf653fa24', 'Emily Davis', 'emily.davis@example.com', true, '$2a$12$5THvcYal/2sG5JC0/4zZpuqBG31CMDvqAmZY7RADLeyCbnBJgIJC2'),
(gen_random_uuid(), 'Aleksa Kuzman', 'aleksa.kuzman@example.com', true, '$2a$12$5THvcYal/2sG5JC0/4zZpuqBG31CMDvqAmZY7RADLeyCbnBJgIJC2');
