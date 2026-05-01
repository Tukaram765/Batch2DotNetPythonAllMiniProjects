import { employeeService } from "./employeeService.js";

export const validationService = {

  validateAuth({ username, password, confirmPassword }) {
    let errors = {};

    if (!username) errors.username = "Username is required";

    if (!password || password.length < 6)
      errors.password = "Password must be at least 6 characters";

    if (confirmPassword !== undefined && password !== confirmPassword)
      errors.confirmPassword = "Passwords do not match";

    return errors;
  },

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

else if (employeeService.isEmailExists(data.email, Number(data.id))) {
  errors.email = "Email already registered";
}


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
  if (!data.salary || data.salary <= 0) {
    errors.salary = "Salary must be positive";
  }

  // Join Date
  if (!data.joinDate || data.joinDate === "") {
    errors.joinDate = "Join date required";
  }

  return errors;
}
};