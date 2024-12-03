# JWT Authentication and Data Visualization Project

## Project Overview

This ASP.NET Core web application demonstrates:
- User authentication with JWT tokens.
- API endpoints for managing and visualizing data stored in Microsoft SQL Server Management Studio (SSMS).
- Visual representation of data using bar and pie charts with filtering options.

---

## Features

### 1. Authentication
- **Register and Login Pages**:
  - Secure user registration with hashed passwords using `BCrypt.Net-Next`.
  - Token-based authentication via `Microsoft.AspNetCore.Authentication.JwtBearer`.

### 2. Statistika Management
- **Statistika Controller**:
  - API endpoints to create and retrieve `Statistika` entries stored in SSMS.
  - A view displaying all `Statistika` entries for easy access.

### 3. Data Visualization
- **Report Controller**:
  - Bar and pie charts for representing `Statistika` data.
  - Filtering options to customize the displayed data.

---

## Technologies Used

### Authentication and Security
- `BCrypt.Net-Next` (v4.0.3): Password hashing.
- `Microsoft.AspNetCore.Authentication.JwtBearer` (v6.0.36): JWT authentication.
- `System.IdentityModel.Token.JWT` (v6.36.0): JWT token management.

### Entity Framework Core
- `Microsoft.EntityFrameworkCore` (v6.0.36): ORM for database interaction.
- `Microsoft.EntityFrameworkCore.SqlServer` (v6.0.36): SQL Server integration.
- `Microsoft.EntityFrameworkCore.Tool` (v6.0.36): EF Core tooling.
- `Microsoft.AspNetCore.Identity.EntityFramework` (v6.0.36): Identity management.

### Additional Libraries
- `Portable.BouncyCastle` (v1.9.0): Cryptographic utilities.
- `Swashbuckle.AspNetCore` (v6.9.0): Swagger API documentation.

---

## Database Installation
A BACPAC file is included in the repository.

## Installation and Setup

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/sstojani/SoftExpress_Challange.git
   cd <repository-directory>
