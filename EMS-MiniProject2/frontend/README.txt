Project Name: Employee Management System (EMS)

Name: Tukaram  
Batch: B2_.Net Python  

----------------------------------------
PROJECT DESCRIPTION
----------------------------------------
This is a front-end Employee Management System built using:
- HTML
- CSS
- Bootstrap 5
- JavaScript (ES6 Modules)
- jQuery

The application allows:
- Admin registration and login
- Adding, updating, deleting employees
- Viewing employee details
- Dashboard analytics (total, active, inactive, departments)
- Search and filter functionality

Data is managed using localStorage (no backend required).

----------------------------------------
HOW TO RUN THE APPLICATION
----------------------------------------

1. Download.

2. Open the project in any code editor (e.g., VS Code).

3. IMPORTANT:
   Since the project uses ES6 modules (import/export),
   you must run it using a local server.

4. Run using any of the following methods:

   OPTION 1 (VS Code - Recommended):
   - Install "Live Server" extension
   - Right-click on index.html
   - Click "Open with Live Server"


5. Application will open in browser.

----------------------------------------
LOGIN DETAILS (DEMO)
----------------------------------------
Username: Tukaram
Password: Tukaram@2003 

(You can also create a new account using Register)

----------------------------------------
HOW TO RUN TESTS
----------------------------------------

----------------------------------------
HOW TO RUN TESTS
----------------------------------------

This project uses Jest for testing.

1. Open terminal in project folder:
   C:\project\employee-management-dashboard

2. Initialize npm (if not already done):
   npm init -y

3. Install Jest:
   npm install jest --save-dev

4. Enable ES Modules support:

   In package.json, add:
   {
     "type": "module",
     "scripts": {
       "test": "jest"
     }
   }

5. Create Jest config file (IMPORTANT for ES Modules):

   Create file: jest.config.js

   Add:
   export default {
     testEnvironment: "jsdom",
     transform: {}
   };

6. Run tests:
   npm test

----------------------------------------
TEST FILES INCLUDED
----------------------------------------

- tests/employeeService.test.js
- tests/authService.test.js
- tests/dashboardService.test.js

----------------------------------------
WHAT IS TESTED
----------------------------------------

Employee Service:
- Add employee
- Update employee
- Delete employee
- Search functionality
- Filtering
- Sorting

Auth Service:
- Signup
- Duplicate user handling
- Login success/failure
- Logout

Dashboard Service:
- Summary counts
- Department breakdown
- Recent employees

----------------------------------------
EXPECTED OUTPUT
----------------------------------------

All tests should PASS.

Example:

 PASS  tests/employeeService.test.js
 PASS  tests/authService.test.js
 PASS  tests/dashboardService.test.js

----------------------------------------
IMPORTANT NOTES
----------------------------------------

1. Tests use localStorage via storageService
2. beforeEach() resets test data
3. No backend required
4. If tests fail:
   - Check import paths
   - Ensure "type": "module" is set
   - Ensure Jest is installed correctly

----------------------------------------

----------------------------------------
Manual Testing Steps:
----------------------------------------

1. Register a new user
2. Login with valid credentials
3. Add a new employee
4. Edit employee details
5. Delete employee
6. Use search and filter options
7. Verify dashboard updates

All functionalities should work without page reload.

----------------------------------------
KNOWN ISSUES / NOTES
----------------------------------------
- No backend/database integration
- Refreshing browser keeps data (until localStorage is cleared)

----------------------------------------
END OF FILE
----------------------------------------