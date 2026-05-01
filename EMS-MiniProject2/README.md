# Employee Management System - Mini Project 2

---

## Student Details
- **Name:** Tukaram Bhangare  
- **Batch:** B2  
- **Course:** .NET with Python  

---

## Project Overview
This is a full-stack Employee Management System built using:

### Backend (.NET 8 Web API)
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core (Code First)
- SQL Server 2022
- JWT Authentication
- BCrypt Password Hashing
- Swagger / OpenAPI

### Frontend
- HTML5
- CSS3
- Bootstrap 5
- JavaScript (ES6+)
- jQuery

---

## Backend Setup (API)

### Step 1: Run Database Migration
```bash
dotnet ef database update


----------------------------------------------------------------------------
## Run Backend
----------------------------------------------------------------------------

dotnet run


----------------------------------------------------------------------------
## Run Published DLL
----------------------------------------------------------------------------

cd EMS.API\bin\Release\net8.0\publish
dotnet EMS.API.dll

----------------------------------------------------------------------------
## Backend URLs
----------------------------------------------------------------------------
Swagger UI:
http://localhost:7220/swagger/index.html


----------------------------------------------------------------------------
## Frontend Setup
----------------------------------------------------------------------------
step 1

Open frontend folder:
frontend/index.html

step 2

Open with Live Server

Frontend URL:
http://127.0.0.1:5501/frontend/


API Configuration:
frontend/js/config.js
const API_BASE_URL = "http://localhost:5000/api";



---------------------------------------------------------
How to Run Full Project
1. Start Backend
dotnet EMS.API.dll
2. Start Frontend

Open:

index.html → Live Server

--------------------------------------------------------------

Project Structure

-----------------------------------------------------------------

EMS-MiniProject2/
│
├── EMS.API/          (.NET Web API Backend)
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Data/
│   ├── DTOs/
│   └── Program.cs
│
└── frontend/
    ├── index.html
    ├── css/
    └── js/
        ├── config.js
        ├── authService.js
        ├── storageService.js
        ├── employeeService.js
        ├── uiService.js
        └── app.js

