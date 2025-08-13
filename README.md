
# Clinic Booking System API


This project is a comprehensive backend API for a clinic booking platform. I developed it to demonstrate and solidify my skills in building robust, scalable, and secure web APIs using modern .NET technologies and professional software architecture patterns.

-----

##  Key Features

The API provides a complete solution for managing clinic appointments, with dedicated functionalities for three distinct user roles: Admins, Doctors, and Patients.

#### **General & Authentication**

  -  Secure user registration and login using **JWT (JSON Web Tokens)**.
  -  Full password management lifecycle (Forgot Password, Reset Password).
  -  Role-based authorization to protect endpoints and secure data.

#### **Admin Panel (`Admin` Role)**

  -  A powerful dashboard with system-wide statistics (e.g., total users, top doctors).
  -  Full user management: view all users with their roles, block/unblock accounts.
  -  Ability to **promote** a regular user to a `Doctor` role.
  -  Full CRUD management for medical `Specialties`.
  -  Moderation tools for managing user-submitted `Reviews`.

#### **Doctor Portal (`Doctor` Role)**

  -  A personal dashboard with key performance metrics (total appointments, average rating, etc.).
  -  Full control over their professional profile (bio, consultation fees, experience).
  -  A flexible system to set and update weekly **working hours**.
  -  An **on-demand schedule generation** system that creates bookable slots based on working hours.
  -  Manage the entire appointment lifecycle: view, **confirm**, **complete**, or **cancel** bookings.

#### **Patient Experience (`Patient` Role)**

  -  Advanced search and filtering for doctors (by name, specialty).
  -  **Pagination** for all list-based endpoints to ensure high performance.
  -  View detailed doctor profiles and read patient reviews.
  -  Seamlessly book available time slots.
  -  View personal appointment history.
  -  Full control over their own reviews (**Create**, **Update**, **Delete**).

-----

##  Architectural Concepts

This project was built with a strong emphasis on clean, maintainable, and scalable architecture.

  * **Layered (3-Tier) Architecture:** A clear separation of concerns between the Presentation Layer (`API`), Business Logic Layer (`BLL`), and Data Access Layer (`DAL`).
  * **Repository & Unit of Work Patterns:** The DAL uses these patterns to abstract data access logic and ensure atomic transactions, maintaining data integrity across complex operations.
  * **Result Pattern for Services:** The BLL uses a unified `Result<T>` pattern for all service responses, providing robust and predictable error handling without relying on exceptions for control flow.
  * **Global Exception Handling:** A custom middleware catches all unhandled exceptions, logs them, and returns a clean, standardized error response to the client.

-----

##  Tech Stack

  * **Framework:** ASP.NET Core 8
  * **Database:** Entity Framework Core 8 with SQL Server 
  * **Authentication:** ASP.NET Core Identity & JWT Bearer Tokens
  * **Architecture:** 3-Tier, Repository, Unit of Work

-----

##  Getting Started

To run this project locally, follow these steps:

1.  **Clone the repository:**

    ```bash
    git clone https://github.com/FadyASadek/Clinic-System-API.git
    ```

2.  **Configure User Secrets:**
    This project uses User Secrets to store sensitive data. Navigate to the main API project directory in your terminal and set the following secrets:

    ```bash
    dotnet user-secrets set "ConnectionStrings:SQLDataBase" "Your_Database_Connection_String"
    dotnet user-secrets set "JWT:SecretKey" "Your_Very_Strong_And_Secret_JWT_Key"
    ```

3.  **Apply Database Migrations:**

    ```bash
    dotnet ef database update
    ```

4.  **Run the application:**

    ```bash
    dotnet run
    ```

    The API will be running, and you can access the Swagger documentation at `/swagger`.
