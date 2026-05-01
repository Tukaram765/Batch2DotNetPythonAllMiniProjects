// validationService.js
// Client-side validation + server error mapping

export const validationService = {

    // =========================================
    // AUTH VALIDATION
    // =========================================
    validateAuth({ username, password, confirmPassword }) {
        const errors = {};

        if (!username || username.trim() === "") {
            errors.username = "Username is required";
        }

        if (!password || password.length < 6) {
            errors.password = "Password must be at least 6 characters";
        }

        if (confirmPassword !== undefined && password !== confirmPassword) {
            errors.confirmPassword = "Passwords do not match";
        }

        return errors;
    },

    // =========================================
    //  EMPLOYEE VALIDATION (CLIENT SIDE)
    // =========================================
    validateEmployee(data) {
        const errors = {};

        // First Name
        if (!data.firstName || data.firstName.trim() === "") {
            errors.firstName = "First name required";
        }

        // Last Name
        if (!data.lastName || data.lastName.trim() === "") {
            errors.lastName = "Last name required";
        }

        // Email
        if (!data.email || data.email.trim() === "") {
            errors.email = "Email required";
        } else if (!/^\S+@\S+\.\S+$/.test(data.email)) {
            errors.email = "Invalid email format";
        }

        // REMOVED duplicate check (API handles it now)
        // employeeService.isEmailExists(...) 

        // Phone
        if (!data.phone || !/^\d{10}$/.test(data.phone)) {
            errors.phone = "Phone must be 10 digits";
        }

        // Department
        if (!data.department || data.department === "") {
            errors.department = "Select department";
        }

        // Designation
        if (!data.designation || data.designation.trim() === "") {
            errors.designation = "Designation required";
        }

        // Salary
        if (!data.salary || Number(data.salary) <= 0) {
            errors.salary = "Salary must be positive";
        }

        // Join Date
        if (!data.joinDate || data.joinDate === "") {
            errors.joinDate = "Join date required";
        }

        return errors;
    },

    // =========================================
    // MAP SERVER ERRORS (NEW)
    // =========================================
    mapServerErrors(error) {
        const errors = {};

        // Handle 409 Conflict (email already exists)
        if (error && error.message) {
            if (error.message.toLowerCase().includes("email")) {
                errors.email = "Email already exists";
            }
        }

        return errors;
    }

};