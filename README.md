# RaceDay
RaceDay - Full-Stack Web Event Management System (PROG6212 PoE Part 1)

RaceDay is a web-based event management application tailored for South African road running, walking and cycling event (such as the Soweto Marathon and Cape Town Tour).
 
---

## System Access & Roles 

The system is configured around two primary user roles:

1. **Organiser:**
   - Create, update, and manage road events and distance categories.
   - Capture and publish official race finish times and positions.
   - View participant enrolments across event categories.

2. **Participant:**
   - Register account profile details and manage personal info.
   - Browse upcoming road events and distance categories.
   - Enrol into race categories and view personal race history.

---

## Repository Documentation Structure

- `/docs/erd-diagram.png` - Entity Relationship Diagram representing all 6 system entities.
- `/docs/api-endpoint-plan.md` - Complete RESTful API endpoint specifications.
- `/docs/schema.sql` - T-SQL schema setup script and sample seed data for SSMS.
- `/.github/workflows/validate-docs.yml` - CI/CD pipeline verifying documentation file existence.

---

## CI/CD Pipeline

The GitHub Actions workflow automatically validates that all required documentation files exist in the `/docs` directory on every push to `main`.

---

## Video Presentation

- **YouTube Link:** [Insert your unlisted YouTube video URL here]
## Tech Stack
- **Backend Framework:** .NET Core C# API
- **Database:** Microsoft SQL Server (SSMS)
