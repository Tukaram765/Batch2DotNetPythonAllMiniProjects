import { dashboardService } from "../js/dashboardService.js";
import { storageService } from "../js/storageService.js";

describe("Dashboard Service Tests (No Mock)", () => {

  beforeEach(() => {
    storageService._setData([
      { id: 1, department: "Engineering", status: "Active" },
      { id: 2, department: "HR", status: "Inactive" },
      { id: 3, department: "Engineering", status: "Active" }
    ]);
  });

  test("Summary Counts", () => {
    const summary = dashboardService.getSummary();

    expect(summary.total).toBe(3);
    expect(summary.active).toBe(2);
    expect(summary.inactive).toBe(1);
    expect(summary.departments).toBe(2);
  });

  test("Department Breakdown", () => {
    const breakdown = dashboardService.getDepartmentBreakdown();

    expect(breakdown.Engineering).toBe(2);
    expect(breakdown.HR).toBe(1);
  });

  test("Recent Employees", () => {
    const recent = dashboardService.getRecentEmployees(2);

    expect(recent.length).toBe(2);
  });

});