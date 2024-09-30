INSERT INTO contacts.contacts (id, "name", phone_number, "user_id")
VALUES
(gen_random_uuid(), 'John Doe', '123-456-7890','5bb4bd9d-e688-4379-a77b-7972830c067e'),
('d490f7a6-eb62-448e-bfa5-9825f526122e', 'Jane Smith', '987-654-3210','78d4466c-6c23-465a-8e89-be4cf653fa24'),
('cc8cc779-3df6-44e1-8813-06defe9a2540', 'Alice Johnson', '555-123-4567','5bb4bd9d-e688-4379-a77b-7972830c067e'),
(gen_random_uuid(), 'Bob Williams', '444-555-6666','5bb4bd9d-e688-4379-a77b-7972830c067e'),
('aeed9052-02fd-4c82-b9cb-f2d11781a28c', 'Eve Brown', '333-222-1111','5bb4bd9d-e688-4379-a77b-7972830c067e');