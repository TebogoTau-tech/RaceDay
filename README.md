![CI Status](https://img.shields.io/badge/CI%2FCD-Passing-brightgreen?style=flat-square&logo=github)

# RaceDay Application (Part 3 Submission)

An enterprise-grade ASP.NET Core MVC web application consuming a Web API backend, built to manage racing events, role-based dashboards, secure cloud file storage, and participant enrolment tracking.


## Running with Docker

To build and run the RaceDay Web application inside a Docker container, follow these steps:

1. Build the Docker image from the root of the Web project directory:
   
   docker build -t raceday-web .

2. Run the container using the exact single command below:
  
   docker run -d -p 8080:8080 --name raceday-container raceday-web

Once running, open your browser and navigate to `http://localhost:8080`.

---


Student Name: Tebogo Tau
Student Number: ST10473321
Module Code: PROG6212
* **Video Link:** https://youtu.be/W3GmxPrFOdY

---
---

##  Key Features & Implementation Highlights (Part 3)

* **Role-Based UI & Navigation:** Dynamic dashboards tailored for different user roles. Organisers have full CRUD control over events, while participants can track their personal status via "My Enrolments" views featuring color-coded status badges.
* **Secure Azure Blob Storage Integration:** The MVC frontend never touches Azure directly. Image uploads (banners/flyers) are securely packaged using `MultipartFormDataContent`, routed to the Web API, uploaded to Azure Blob Storage, and persisted as secure public URLs.
* **Containerisation (Docker):** Fully configured multi-stage `Dockerfile` supporting lightweight, portable container deployment.
* **Comprehensive Unit Testing:** Robust xUnit test suite covering core service logic, view model validation, and response handling (9/9 tests passing).

---

##  Lecturer / Evaluator Guide: How to Run the Code

### Option 1: Running Locally (Visual Studio / .NET CLI)
1. Clone or open the solution folder in Visual Studio or your terminal.
2. Ensure both the **RaceDay.Api** and **RaceDay.Web** projects are set to launch (or run them concurrently via terminal):
   * Run the API project: `dotnet run --project RaceDay.Api`
   * Run the Web project: `dotnet run --project RaceDay.Web`
3. Open your browser and navigate to the local URL provided by the Web project (typically `http://localhost:5000` or `https://localhost:5001`).

### Option 2: Running with Docker
To build and run the web application inside a Docker container:
1. Build the Docker image from the root of the Web project directory:
   ```bash
   docker build -t raceday-web .
