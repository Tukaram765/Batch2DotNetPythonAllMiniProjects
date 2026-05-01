
import { employees } from "./data.js";

let empData = [...employees];

export const storageService = {


  // Get all employees
  getAll() {
    return empData;
  },

  // Get employee by ID
  getById(id) {
     return empData.find(e => e.id === Number(id)); 
  },

  // Add new employee
  add(emp) {
    empData.push(emp);
  },

  // Update employee
  update(id, updatedData) {
    const index = empData.findIndex(e => e.id === Number(id));

    if (index !== -1) {
      empData[index] = {
        ...empData[index],
        ...updatedData
      };
    }
  },

  // Remove employee
  remove(id) {
    empData = empData.filter(e => e.id !== Number(id));
  },

  // Generate next ID
  nextId() {
    return empData.length
      ? Math.max(...empData.map(e => e.id)) + 1
      : 1;
  },



  // Set custom data 
  _setData(newData) {
    empData = [...newData];
  },

  // Reset to original data
  _reset() {
    empData = [...employees];
  }
};