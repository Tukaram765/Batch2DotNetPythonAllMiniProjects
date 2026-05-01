
import { employeeService } from "./employeeService.js";

export const dashboardService = {
  // Returns summary counts
  getSummary() {
    const data = employeeService.getAll();

    return {
      total: data.length,
      active: data.filter(e => e.status === "Active").length,
      inactive: data.filter(e => e.status === "Inactive").length,
      departments: [...new Set(data.map(e => e.department))].length
    };
  },

  // Returns last added employees
  getRecentEmployees(n = 5) {
    return employeeService.getAll()
      .sort((a, b) => b.id - a.id) 
      .slice(0, n);
  },

  // Returns a breakdown of employees per department
  getDepartmentBreakdown() {
    const data = employeeService.getAll();
    return data.reduce((acc, emp) => {
      if (!acc[emp.department]) acc[emp.department] = 0;
      acc[emp.department]++;
      return acc;
    }, {});
  }
};