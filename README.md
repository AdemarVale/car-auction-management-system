# CAR-AUCTION-MANAGEMENT-SYSTEM

**Author**: Ademar Vale 1
**Date**: 14/07/2024

## Project Structure
Followed onion architecture to develop this API:

![image](https://github.com/user-attachments/assets/7cfde206-3f08-421d-b30a-064ad5c52d67)

Which translates to having 5 different sections in project structure, each one representing a layer with different responsibilities.

* API: endpoints declaration
* Application: business logic handler
* Domain: domain definition
* Infrastructure: external services communication & dependencies
* Persistence: persistence related logic (database, cache, etc.)

![image](https://github.com/user-attachments/assets/797337fc-cb35-4985-85bd-39f0d597d3fb)

The API is the entry point, and every other component is either a class library or a test solution.

Regarding unit & integration testing, developed two different test solutions with a total of 63 tests covering every business feature and identified edge cases.
Integration tests require a containerization application like docker desktop.

## How to run

Dockerized the application in order to run it everywhere, the only dependency is to have a containerization application.

In the project root, execute 'docker-compose up --build' in order to run the application and all dependencies.

* SWAGGER: http://localhost/swagger/index.html
* Logs observability (SEQ): http://localhost:8081/#/events?range=1d

## Decisions

* In order to store & manage business logic, a RDBMS was chosen, specifically PostgreSQL.
* Used Entity Framework as ORM.
* Used SEQ as log observability (non-requested feature).
* Used xUnit as testing framework.
* **Used Table-per-hierarchy and discriminator configuration** in order to store vehicles and all its types. Even though they have different properties, all queries & validations are against the same entity: vehicle.

## Implementation details and patterns

* API versioning
* Swagger
* Heartbeat & Healthcheck endpoints
* Version endpoints
* Serilog & SEQ as logging package & collector
* MediatR package for Mediator Pattern implementation

## Database structure

![image](https://github.com/user-attachments/assets/7b068707-9000-472f-9fbd-67b8cda0e2ed)

