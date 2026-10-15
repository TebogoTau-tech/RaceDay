# RaceDay
RaceDay - Full-Stack Web Event Management System (PROG6212 PoE Part 2)

# RaceDay RESTful API (.NET 8.0)

A robust, secure, and modular RESTful API built for managing running events, categories, user accounts, and participant enrolments. Developed as part of an advanced software engineering and database systems module.

---

##  Tech Stack & Architecture

* **Framework:** .NET 8.0 (C#)
* **Data Access:** Entity Framework Core (EF Core) with SQL Server
* **Design Patterns & Modeling:** 
  * Table-per-Hierarchy (TPH) inheritance model for `User` (`Organiser` vs. `Participant`)
  * REST architectural principles with Controller/DTO separation
* **Security & Auth:** Password hashing via BCrypt and JWT/Token-ready controller architecture
* **Testing:** xUnit, Moq, and EF Core In-Memory Database for high-coverage unit tests

---

##  Database Schema & TPH Design
The system employs **Table-per-Hierarchy (TPH)** inheritance mapping for users, allowing both `Organiser` and `Participant` entities to share a single `Users` table distinguished by a discriminator column. 
* **Events:** Linked to organizers and categories.
* **Enrolments:** Enforces strict validation constraints (e.g., preventing duplicate event registrations).

---

## Getting Started & Setup

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed on your machine.
* SQL Server or LocalDB.

### Installation & Database Migration
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd RaceDay

## Commit & Workflow Strategy
All feature commits follow conventional commit naming guidelines.

---
YouTube Link : [link]
