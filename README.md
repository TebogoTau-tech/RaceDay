# RaceDay RESTful API (.NET 8.0)

A secure and modular RESTful API built with .NET 8.0 and Entity Framework Core (Code-First) for managing running events, categories, user accounts, and participant enrolments.

---


Student Name: Tebogo Tau
Student Number: ST10473321
Module Code: PROG6212
* **Video Link:** https://youtu.be/FP9MSLGpp1c

---

## Tech Stack & Architecture
Framework: .NET 8.0 (C#)
Database & ORM: SQL Server via Entity Framework Core (EF Core) Code-First
Design Pattern: Table-per-Hierarchy (TPH) inheritance model for the `User` base class, differentiating `Organiser` and `Participant` entities in a single table.
Testing: xUnit, Moq, and EF Core In-Memory Database for unit testing.
Security: Password hashing using BCrypt.

---

Step-by-Step Testing Guide 

Follow these steps to run and test the application locally:

1. Prerequisites
Ensure you have the following installed on your machine:
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Visual Studio 2022 (or Visual Studio Code) with SQL Server / LocalDB.


2. Clone and Open the Project
   1. Clone the repository to your local machine:
   ```bash
   git clone: https://github.com/TebogoTau-tech/RaceDay.git

   Open the solution file (RaceDay.sln) in Visual Studio.


 3. Setup the Database
Open the Package Manager Console in Visual Studio (Tools > NuGet Package Manager > Package Manager Console).

Run the following command to apply migrations and generate the SQL Server database:

dotnet ef database update --project RaceDay.Api


4. Running the Application (Swagger UI)
In Visual Studio, set RaceDay.Api as the Startup Project (Right-click project -> Set as Startup Project).

Press F5 (or click the green Start button).

Your default web browser will automatically open to the Swagger UI page (https://localhost:xxxx/swagger), where you can interactively test all API endpoints.


5. Running the Unit Tests
To verify the test suite:

Open Test Explorer in Visual Studio (Test > Test Explorer).

Click Run All to execute all unit tests verifying controller logic, registrations, and enrolments.
(Alternatively, run dotnet test from your terminal).


Core API Endpoints Reference


POST/api/Auth/register-participant -- Registers a new participant account with a securely hashed password.

POST/api/Auth/login -- Authenticates users and returns role metadata (Organiser vs. Participant).

GET/api/Events/categories -- Retrieves all available race categories.

POST/api/Events/enrol -- Enrols a participant into a specific event category (includes duplicate checks).

GET/api/Events/participant/{id}/enrolments -- Retrieves a participant's registered event history.
