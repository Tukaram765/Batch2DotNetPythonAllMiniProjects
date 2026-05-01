
import { authService } from "./authService.js";
import { employeeService } from "./employeeService.js";
import { uiService } from "./uiService.js";
import { dashboardService } from "./dashboardService.js";
import { validationService } from "./validationService.js";



$(document).ready(function () {

     const navbar = $("#mainNavbar");

function showView(viewClass) {
    $(".view").hide();
    $(viewClass).show();

    forceCloseAllModals();

    if (viewClass === ".login-view" || viewClass === ".register-view") {
        navbar.hide();
    } else {
        navbar.show();
    }
}

  $(".view").hide();
  showView(".login-view");

  $(document).on("click", ".signup-link", function (e) {
    e.preventDefault();
    showView(".register-view");
  });

  $(document).on("click", ".login-link", function (e) {
    e.preventDefault();
    showView(".login-view");
  });

  $(".nav-dashboard").click(function (e) {
  e.preventDefault();
  showView(".dashboard-view");

  loadDashboard();
  loadRecentEmployees();

  $(".navbar-nav .nav-link").removeClass("active bg-soft-blue text-dark-blue text-white");
  $(this).addClass("active bg-soft-blue text-dark-blue");
});

$(".nav-employees").click(function (e) {
  e.preventDefault();
  showView(".employee-view");

  loadEmployees();

  $(".navbar-nav .nav-link").removeClass("active bg-soft-blue text-dark-blue text-white");
  $(this).addClass("active bg-soft-blue text-dark-blue");
});

  // SIGNUP
  $(".register-view form").submit(function (e) {
    e.preventDefault();
    const username = $(this).find("input[type=text]").val().trim();
    const password = $(this).find("input[type=password]").eq(0).val();
    const confirmPassword = $(this).find("input[type=password]").eq(1).val();

    const errors = validationService.validateAuth({
      username,
      password,
      confirmPassword
    });

    if (Object.keys(errors).length > 0) {
      uiService.showToast("Validation failed", "danger");
      return;
    }

    const res = authService.signup(username, password);
    if (res.error) uiService.showToast(res.error, "danger");
    else {
      uiService.showToast("Signup successful", "success");
      showView(".login-view");
      this.reset();
    }
  });

  // LOGIN
  $(".login-view form").submit(function (e) {
    e.preventDefault();
    const username = $(this).find("input[type=text]").val().trim();
    const password = $(this).find("input[type=password]").val();

    const errors = validationService.validateAuth({ username, password });
    if (Object.keys(errors).length > 0) {
      uiService.showToast("Validation failed", "danger");
      return;
    }

    const res = authService.login(username, password);
    if (res.error) uiService.showToast("Invalid credentials", "danger");
    else {
      $(".view").hide();
      showView(".dashboard-view");
      loadDashboard();
      loadEmployees();
      loadRecentEmployees();

      $(".navbar-nav .nav-link").removeClass("active bg-soft-blue text-dark-blue text-white");
      $(".nav-dashboard").addClass("active bg-soft-blue text-dark-blue");
    }
  });

  function forceCloseAllModals() {
    document.querySelectorAll('.modal.show').forEach(modalEl => {
        const instance = bootstrap.Modal.getInstance(modalEl);
        if (instance) {
            instance.hide();
        }
    });

    document.body.classList.remove("modal-open");
    document.body.style.overflow = "auto";
    document.body.style.paddingRight = "";

    document.querySelectorAll(".modal-backdrop").forEach(el => el.remove());
}

  // LOAD DASHBOARD
  function loadDashboard() {
    const summary = dashboardService.getSummary();

    $("#totalEmployees").text(summary.total);
    $("#activeEmployees").text(summary.active);
    $("#inactiveEmployees").text(summary.inactive);
    $("#departmentCount").text(summary.departments);

    const recent = dashboardService.getRecentEmployees(5);
    uiService.renderRecentEmployees(recent);

    const breakdown = dashboardService.getDepartmentBreakdown();
    uiService.renderDepartmentBreakdown(breakdown);
  }

  function loadDepartmentBreakdown() {
    const data = employeeService.getAll();
    const breakdown = data.reduce((acc, e) => {
      if (!acc[e.department]) acc[e.department] = 0;
      acc[e.department]++;
      return acc;
    }, {});

    uiService.renderDepartmentBreakdown(breakdown);
  }

  // RECENT EMPLOYEES
  function loadRecentEmployees() {
    const recent = dashboardService.getRecentEmployees();
    uiService.renderRecentEmployees(recent);
  }

  // EMPLOYEE TABLE
  function loadEmployees() {
    const data = employeeService.getAll().slice().reverse();
    uiService.renderEmployees(data);
    $("#recordCount").text(data.length);
  $("#totalCount").text(data.length);
    loadDepartments();
  }

  function loadDepartments() {
    const data = employeeService.getAll();
    const unique = [...new Set(data.map(e => e.department))];
    const select = $("#deptFilter");

    select.empty().append(`<option value="">All Departments</option>`);
    unique.forEach(d => select.append(`<option value="${d}">${d}</option>`));
  }

const modalEl = document.getElementById("addEmployeeModal");


modalEl.addEventListener("hidden.bs.modal", () => {
  document.body.classList.remove("modal-open");
  document.body.style.overflow = "auto";
  document.body.style.paddingRight = "";

  document.querySelectorAll(".modal-backdrop").forEach(el => el.remove());
});

  // ADD / UPDATE EMPLOYEE
  const form = document.getElementById("addEmployeeForm");
  form.addEventListener("submit", function (e) {
    e.preventDefault();
    const formData = Object.fromEntries(new FormData(this).entries());

    if (formData.salary) formData.salary = Number(formData.salary);

    const errors = validationService.validateEmployee(formData);
    this.querySelectorAll(".text-danger.custom-error").forEach(el => el.remove());

    if (Object.keys(errors).length > 0) {
      for (let key in errors) {
        const input = this.querySelector(`[name="${key}"]`);
        if (input) {
          const errorEl = document.createElement("div");
          errorEl.classList.add("text-danger", "small", "custom-error");
          errorEl.textContent = errors[key];
          input.parentElement.appendChild(errorEl);
        }
      }
      return;
    }

   if (formData.id && formData.id !== "") {
  employeeService.update(Number(formData.id), formData);
  uiService.showToast("Employee Updated", "success");
} else {
  employeeService.add(formData);
  uiService.showToast("Employee Added", "success");
}

    
    this.reset();
this.querySelector("[name='id']").value = "";

$("#addEmployeeModalLabel").text("Add Employee");
$("#submitBtn").text("Save Employee");



loadEmployees();
loadDashboard();
loadRecentEmployees();
  });

  // DELETE EMPLOYEE
  let deleteId = null;
  $(document).on("click", ".deleteBtn", function () {
    deleteId = Number($(this).data("id"));
    const modal = new bootstrap.Modal(document.getElementById("deleteModal"));
modal.show();
  });

$("#confirmDeleteBtn").click(function () {
  if (deleteId !== null) {
    employeeService.remove(deleteId);
    uiService.showToast("Employee Deleted", "success");

    loadEmployees();
    loadDashboard();
    loadRecentEmployees();

    deleteId = null;

    const modal = bootstrap.Modal.getInstance(document.getElementById("deleteModal"));
    modal.hide();
  }
});

  // SEARCH / FILTER
  $("#searchInput, #deptFilter, input[name='statusFilter']").on("input change", function () {
    const search = $("#searchInput").val();
    const dept = $("#deptFilter").val();
    let status = "";
    if ($("#activeStatus").is(":checked")) status = "Active";
    else if ($("#inactiveStatus").is(":checked")) status = "Inactive";

    const filtered = employeeService.applyFilters(search, dept, status);
    uiService.renderEmployees(filtered);
    $("#recordCount").text(filtered.length);
  $("#totalCount").text(employeeService.getAll().length);
  });

  // VIEW / EDIT BUTTONS
  $(document).on("click", ".viewBtn", function () {
    const emp = employeeService.getById(Number($(this).data("id")));
    uiService.showModal("view", emp);
  });

  $(document).on("click", ".editBtn", function () {
    const emp = employeeService.getById(Number($(this).data("id")));
    uiService.showModal("edit", emp);
  });


  // LOGOUT
$(document).on("click", ".logout-btn", function () {
  
  localStorage.removeItem("loggedInUser");

  $(".view").hide();
  showView(".login-view");



  $("#mainNavbar").hide();

  uiService.showToast("Logged out successfully", "success");
});
  

});


