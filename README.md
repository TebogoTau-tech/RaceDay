# RaceDay RESTful API (.NET 8.0)

A secure and modular RESTful API built with .NET 8.0 and Entity Framework Core (Code-First) for managing running events, categories, user accounts, and participant enrolments.

---


Student Name: Tebogo Tau
Student Number: ST10473321
Module Code: PROG6212
* **Video Link:** [Insert YouTube Demo Video Link Here]

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
