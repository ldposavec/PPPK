INSERT INTO public."Patients" ("FirstName", "LastName", "Oib", "DateOfBirth", "Gender", "NumberOfPatient") VALUES
('John', 'Doe', '12345678901', '1985-06-15', 0, '00001'),
('Jane', 'Smith', '23456789012', '1990-09-25', 1, '00002'),
('Michael', 'Brown', '34567890123', '1978-12-05', 2, '00003');

INSERT INTO public."Examinations" ("PatientId", "DateOfExam", "ExamType", "PicturePath") VALUES
(1, '2024-01-15 10:00:00+00', 1, NULL),
(2, '2024-02-10 14:30:00+00', 2, NULL),
(3, '2024-03-05 09:15:00+00', 1, NULL);

INSERT INTO public."MedDocumentations" ("Diagnosis", "PatientId", "StartIllness", "EndIllness") VALUES
('Flu', 1, '2024-01-01 00:00:00+00', '2024-01-10 00:00:00+00'),
('Pneumonia', 2, '2024-02-01 00:00:00+00', NULL),
('Diabetes Type 2', 3, '2023-11-01 00:00:00+00', NULL);

INSERT INTO public."Prescriptions" ("PatientId", "Medication", "Dose", "Frequency", "Duration") VALUES
(1, 'Paracetamol', '500mg', 'Twice daily', '5 days'),
(2, 'Amoxicillin', '250mg', 'Three times daily', '7 days'),
(3, 'Metformin', '850mg', 'Once daily', 'Indefinite');
