import { employeeService } from "../js/employeeService.js";
import { storageService } from "../js/storageService.js";

describe("Employee Service Tests (No Mock)", () => {

  beforeEach(() => {
    storageService._setData([
      {
        id: 1,
        firstName: "John",
        lastName: "Doe",
        email: "john@test.com",
        department: "Engineering",
        status: "Active",
        salary: 50000,
        joinDate: "2023-01-01"
      },
      {
        id: 2,
        firstName: "Jane",
        lastName: "Smith",
        email: "jane@test.com",
        department: "HR",
        status: "Inactive",
        salary: 60000,
        joinDate: "2022-01-01"
      }
    ]);
  });

  test("Add Employee", () => {
    employeeService.add({
      firstName: "New",
      lastName: "User",
      email: "new@test.com",
      department: "Marketing",
      status: "Active",
      salary: 40000,
      joinDate: "2024-01-01"
    });

    expect(storageService.getAll().length).toBe(3);
  });

  test("Update Employee", () => {
    employeeService.update(1, { firstName: "Updated" });

    expect(storageService.getById(1).firstName).toBe("Updated");
  });

  test("Delete Employee", () => {
    employeeService.remove(1);

    expect(storageService.getAll().length).toBe(1);
  });

  test("Search Employee", () => {
    const result = employeeService.search("john");

    expect(result.length).toBe(1);
  });

  test("Filter by Department", () => {
    const result = employeeService.applyFilters("", "Engineering", "");

    expect(result.length).toBe(1);
  });

  test("Sort by Salary Desc", () => {
    const result = employeeService.applyFilters("", "", "", "salary", "desc");

    expect(result[0].salary).toBe(60000);
  });

});