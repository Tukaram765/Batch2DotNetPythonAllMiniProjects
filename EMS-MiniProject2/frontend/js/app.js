// app.js
import { CONFIG } from "./config.js";
import { AuthService } from "./authService.js";
import { employeeService } from "./employeeService.js";
import { uiService } from "./uiService.js";
import { dashboardService } from "./dashboardService.js";
import { validationService } from "./validationService.js";

$(document).ready(function () {

    const navbar = $("#mainNavbar");

    // =========================================
    // STATE (NEW)
    // =========================================
    let _state = {
        page: 1,
        pageSize: 9,
        search: "",
        department: "",
        status: "",
        sortBy: "name",
        sortDir: "asc"
    };

    // ===============================
// VIEW SWITCHING
// ===============================
const dashboardView = document.querySelector(".dashboard-view");
const employeeView = document.querySelector(".employee-view");

const dashboardLink = document.getElementById("dashboardLink");
const employeesLink = document.getElementById("employeesLink");

// Show Dashboard
dashboardLink.addEventListener("click", (e) => {
    e.preventDefault();

    dashboardView.style.display = "block";
    employeeView.style.display = "none";

    dashboardLink.classList.add("active");
    employeesLink.classList.remove("active");
});

// Show Employees
employeesLink.addEventListener("click", (e) => {
    e.preventDefault();

    dashboardView.style.display = "none";
    employeeView.style.display = "block";

    employeesLink.classList.add("active");
    dashboardLink.classList.remove("active");
});

    // =========================================
    // VIEW HANDLING
    // =========================================
    function showView(viewClass) {
        $(".view").hide();
        $(viewClass).show();

        if (viewClass === ".login-view" || viewClass === ".register-view") {
            navbar.hide();
        } else {
            navbar.show();
        }
    }

    showView(".login-view");

    // =========================================
// SWITCH LOGIN / REGISTER
// =========================================
$(document).on("click", ".signup-link", function (e) {
    e.preventDefault();
    showView(".register-view");
});

$(document).on("click", ".login-link", function (e) {
    e.preventDefault();
    showView(".login-view");
});

    // =========================================
    //  REGISTER
    // =========================================
    $(".register-view form").submit(async function (e) {
        e.preventDefault();

        const username = $(this).find("input[type=text]").val().trim();
        const password = $(this).find("input[type=password]").eq(0).val();
        const confirmPassword = $(this).find("input[type=password]").eq(1).val();
        const role = $(".role-select").val(); // 👈 NEW

        const errors = validationService.validateAuth({
            username,
            password,
            confirmPassword
        });

        if (Object.keys(errors).length) {
            uiService.showToast("Validation failed", "danger");
            return;
        }

        try {
            await AuthService.register({ username, password, role});
            uiService.showToast("Signup successful");
            showView(".login-view");
            this.reset();
        } catch (err) {
            uiService.showToast(err.message, "danger");
        }
    });

    // =========================================
    // LOGIN
    // =========================================
    $(".login-view form").submit(async function (e) {
        e.preventDefault();

        const username = $(this).find("input[type=text]").val().trim();
        const password = $(this).find("input[type=password]").val();

        try {
            await AuthService.login({ username, password });

            showView(".dashboard-view");
            uiService.applyRoleUI();

            await loadDashboard();
            await loadEmployees();

        } catch (err) {
            uiService.showToast(err.message, "danger");
        }
    });

    // =========================================
    // LOAD DASHBOARD
    // =========================================
    async function loadDashboard() {
        try {
            const data = await dashboardService.getSummary();
            uiService.renderDashboard(data);

            uiService.renderDepartmentFilter(data.departmentBreakdown);
        } catch {
            uiService.showToast("Failed to load dashboard", "danger");
        }
    }

    // =========================================
    // LOAD EMPLOYEES (WITH PAGINATION)
    // =========================================
  async function loadEmployees() {
    try {
        const result = await employeeService.getAll(_state);

        uiService.renderEmployees(result);

        //  ADD THIS LINE HERE
        uiService.applyRoleUI();

        uiService.renderPagination(result, (newPage) => {
            _state.page = newPage;
            loadEmployees();
        });

    } catch {
        uiService.showToast("Failed to load employees", "danger");
    }
}

    // =========================================
    // SEARCH / FILTER (SERVER SIDE)
    // =========================================
    $("#searchInput").on("input", debounce(function () {
        _state.search = $(this).val();
        _state.page = 1;
        loadEmployees();
    }, 350));

    $("#deptFilter").on("change", function () {
        _state.department = $(this).val();
        _state.page = 1;
        loadEmployees();
    });

    $("input[name='statusFilter']").on("change", function () {
        if ($("#activeStatus").is(":checked")) _state.status = "Active";
        else if ($("#inactiveStatus").is(":checked")) _state.status = "Inactive";
        else _state.status = "";

        _state.page = 1;
        loadEmployees();
    });

    // =========================================
    // ADD / UPDATE
    // =========================================
    $("#addEmployeeForm").submit(async function (e) {
        e.preventDefault();

        const formData = Object.fromEntries(new FormData(this).entries());
        formData.salary = Number(formData.salary);

        const errors = validationService.validateEmployee(formData);

        if (Object.keys(errors).length) {
            uiService.showInlineErrors(errors);
            return;
        }

        try {
            if (formData.id) {
                await employeeService.update(formData.id, formData);
                uiService.showToast("Employee updated");
            } else {
                await employeeService.add(formData);
                uiService.showToast("Employee added");
            }

            this.reset();
            loadEmployees();
            loadDashboard();

        } catch (err) {
        console.error("ADD ERROR:", err);

        // IMPORTANT FIX
        uiService.showToast(err.message || "Something went wrong", "danger");
    }
    });

    // =========================================
    // DELETE
    // =======================================
    let deleteId = null;

   $(document).on("click", ".deleteBtn", function () {
    deleteId = $(this).data("id");
    new bootstrap.Modal("#deleteModal").show();
});

$("#confirmDeleteBtn").click(async function () {
    if (!deleteId) return;

    try {
        await employeeService.remove(deleteId);

        uiService.showToast("Employee deleted successfully", "success");

        loadEmployees();
        loadDashboard();

        deleteId = null;
        bootstrap.Modal.getInstance("#deleteModal").hide();

    } catch (err) {

        console.error("DELETE ERROR:", err.message);

        //  HANDLE AUTH ERRORS
        if (err.message.includes("not authorized")) {
            uiService.showToast(" You are not authorized", "danger");
        }
        else if (err.message.includes("Session expired")) {
            uiService.showToast("⚠️ Session expired. Please login again", "warning");

            setTimeout(() => {
                AuthService.logout();
                showView(".login-view");
            }, 1500);
        }
        else {
            uiService.showToast(err.message || "Delete failed", "danger");
        }
    }
});

    // =========================================
    // VIEW / EDIT
    // =========================================
    $(document).on("click", ".viewBtn", async function () {
        const emp = await employeeService.getById($(this).data("id"));
        uiService.showModal("view", emp);
    });

    $(document).on("click", ".editBtn", async function () {
        const emp = await employeeService.getById($(this).data("id"));
        uiService.showModal("edit", emp);
    });

    // =========================================
    //  LOGOUT
    // =========================================
    $(".logout-btn").click(function () {
        AuthService.logout();
        showView(".login-view");
        uiService.showToast("Logged out");
    });

    // =========================================
    //  DEBOUNCE (UTILITY)
    // =========================================
    function debounce(fn, delay) {
        let timer;
        return function () {
            clearTimeout(timer);
            timer = setTimeout(() => fn.apply(this, arguments), delay);
        };
    }

});