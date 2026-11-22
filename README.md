 # Patients MVC

A clinic management web application built with ASP.NET Core MVC (.NET 8),
Entity Framework Core and SQLite, as the term project of CTIS 479 at Bilkent University.

## Features
- CRUD for branches, doctors and patients with server-side and client-side validation
- One-to-many (Branch - Doctor) and many-to-many (Doctor - Patient) relationships
- Calculated patient fields: age and Body Mass Index (BMI) with WHO category
- Cookie authentication with Admin and User roles; role-based authorization
- Study cohort: a session-based temporary patient list with average age and BMI

## Architecture
N-layered: CORE (base classes), APP (entities, models, services), MVC (controllers, views).
Services are injected through interfaces (IService<TRequest, TResponse>).

## Run
1. Open ClinicMVC.sln in Visual Studio 2022 and run the MVC project.
2. Go to /seeddb to fill the database with sample data.
3. Log in with admin/admin (Admin) or user/user (User).

## Note
All patient data in this project is fictional sample data. Passwords are stored in plain text
for course purposes only; a production system would hash them.