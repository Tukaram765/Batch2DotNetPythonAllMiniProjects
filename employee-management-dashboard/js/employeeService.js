import { storageService } from "./storageService.js";

export const employeeService = {

  getAll() {
    return storageService.getAll();
  },

  getById(id) {
    return storageService.getById(id);
  },

  add(data) {
    const newEmp = {
  ...data,
  id: storageService.nextId()
};

    storageService.add(newEmp);
    return newEmp;
  },

  update(id, data) {
    storageService.update(id, data);
  },

  remove(id) {
    storageService.remove(id);
  },

  // DUPLICATE EMAIL CHECK
  isEmailExists(email, id = null) {
    return this.getAll().some(e =>
      (e.email || "").toLowerCase() === email.toLowerCase() &&
      e.id !== id
    );
  },

  // SEARCH
  search(query) {
    return this.getAll().filter(e =>
      `${e.firstName} ${e.lastName}`.toLowerCase().includes(query.toLowerCase()) ||
      (e.email || "").toLowerCase().includes(query.toLowerCase())
    );
  },

  // FILTER + SORT (PRODUCTION LEVEL)
  applyFilters(search, dept, status, sortField = "", direction = "asc") {
    let result = [...this.getAll()];

    //  SEARCH
    if (search) {
      result = result.filter(e =>
        `${e.firstName} ${e.lastName}`.toLowerCase().includes(search.toLowerCase()) ||
        (e.email || "").toLowerCase().includes(search.toLowerCase())
      );
    }

    // DEPARTMENT
    if (dept) {
      result = result.filter(e => e.department === dept);
    }

    // STATUS
    if (status) {
      result = result.filter(e => e.status === status);
    }

    // SORTING 
    if (sortField) {
      result.sort((a, b) => {

        if (sortField === "salary") {
          return direction === "asc"
            ? a.salary - b.salary
            : b.salary - a.salary;
        }

        if (sortField === "joinDate") {
          return direction === "asc"
            ? new Date(a.joinDate) - new Date(b.joinDate)
            : new Date(b.joinDate) - new Date(a.joinDate);
        }

        if (sortField === "name") {
          return direction === "asc"
            ? a.lastName.localeCompare(b.lastName)
            : b.lastName.localeCompare(a.lastName);
        }

        return 0;
      });
    }

    return result;
  }
};