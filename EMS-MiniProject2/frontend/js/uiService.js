// uiService.js

import { AuthService } from "./authService.js";

export const uiService = {

    // =========================================
    // VIEW MANAGEMENT
    // =========================================
    showSection(sectionClass) {
        $(".view").hide();
        $(sectionClass).show();
    },

    // =========================================
    // EMPLOYEE TABLE
    // =========================================
    renderEmployees(pagedResult) {
        const tbody = $("#employeeTableBody");
        tbody.empty();

        const data = pagedResult.data || [];

        if (data.length === 0) {
            tbody.append(`
                <tr>
                    <td colspan="10" class="text-center text-muted">
                        No employees found
                    </td>
                </tr>
            `);
            return;
        }

        data.forEach(e => {

            const salary = Number(e.salary).toLocaleString("en-IN");

            const date = new Date(e.joinDate).toLocaleDateString("en-GB", {
                day: "2-digit",
                month: "short",
                year: "numeric"
            });

            const initials = (e.firstName?.[0] || "") + (e.lastName?.[0] || "");

            tbody.append(`
                <tr>
                    <td class="ps-4 text-muted">#${e.id}</td>

                    <td>
                        <div class="avatar-circle bg-primary text-white">
                            ${initials.toUpperCase()}
                        </div>
                    </td>

                    <td class="fw-bold">${e.firstName} ${e.lastName}</td>
                    <td class="text-muted small">${e.email}</td>

                    <td>
                        <span class="badge bg-primary-subtle text-primary px-3">
                            ${e.department}
                        </span>
                    </td>

                    <td>${e.designation}</td>

                    <td class="fw-semibold">₹${salary}</td>

                    <td class="text-muted">${date}</td>

                    <td>
                        <span class="badge rounded-pill ${e.status === "Active" ? "bg-success" : "bg-danger"} px-3">
                            ${e.status}
                        </span>
                    </td>

                    <td class="text-center action-col">
                        <button class="btn btn-sm btn-outline-primary viewBtn" data-id="${e.id}">
                            <i class="bi bi-eye"></i>
                        </button>

                        <button class="btn btn-sm btn-outline-warning editBtn" data-id="${e.id}">
                            <i class="bi bi-pencil"></i>
                        </button>

                        <button class="btn btn-sm btn-outline-danger deleteBtn" data-id="${e.id}">
                            <i class="bi bi-trash"></i>
                        </button>
                    </td>
                </tr>
            `);
        });
    },

    // =========================================
    // PAGINATION
    // =========================================
    renderPagination(pagedResult, onPageChange) {
        const container = $("#pagination");
        container.empty();

        if (!pagedResult || pagedResult.totalPages <= 1) return;

        const { page, totalPages } = pagedResult;

        let html = `<ul class="pagination justify-content-center">`;

        html += `
            <li class="page-item ${!pagedResult.hasPrevPage ? "disabled" : ""}">
                <a class="page-link" href="#" data-page="${page - 1}">Prev</a>
            </li>
        `;

        for (let i = 1; i <= totalPages; i++) {
            html += `
                <li class="page-item ${i === page ? "active" : ""}">
                    <a class="page-link" href="#" data-page="${i}">${i}</a>
                </li>
            `;
        }

        html += `
            <li class="page-item ${!pagedResult.hasNextPage ? "disabled" : ""}">
                <a class="page-link" href="#" data-page="${page + 1}">Next</a>
            </li>
        `;

        html += `</ul>`;

        container.html(html);

        container.find("a").click(function (e) {
            e.preventDefault();
            const newPage = Number($(this).data("page"));
            if (!isNaN(newPage)) onPageChange(newPage);
        });
    },

    // =========================================
    // ROLE-BASED UI (FIXED)
    // =========================================
    applyRoleUI(role) {

    const currentRole = role || localStorage.getItem("role");

    // Reset UI first (show everything)
    $(".editBtn, .deleteBtn, .btn-add-employee").show();

    // Always show badge logic
    if (currentRole === "Admin") {

        $("#roleBadge")
            .text("Admin")
            .removeClass("bg-secondary")
            .addClass("bg-success");

        $("#viewerNotice").addClass("d-none");

    } else {

        // Viewer mode → hide actions
        $(".editBtn, .deleteBtn, .btn-add-employee").hide();

        $("#roleBadge")
            .text("Viewer")
            .removeClass("bg-success")
            .addClass("bg-secondary");

        $("#viewerNotice").removeClass("d-none");
    }
},

    // =========================================
    //  PREFILL FORM
    // =========================================
    fillForm(emp) {
        const form = $("#addEmployeeForm")[0];

        Object.keys(emp).forEach(key => {
            if (form[key]) form[key].value = emp[key];
        });
         if (emp.joinDate) {
        const date = new Date(emp.joinDate)
            .toISOString()
            .split("T")[0]; // yyyy-MM-dd

        form.joinDate.value = date;
    }

        if (form["id"]) form["id"].value = emp.id;
    },

    // =========================================
    // INLINE ERRORS
    // =========================================
    showInlineErrors(errors) {
        $(".error-text").remove();

        Object.keys(errors).forEach(key => {
            const input = $(`[name="${key}"]`);
            input.after(`
                <small class="text-danger error-text">
                    ${errors[key]}
                </small>
            `);
        });
    },

    // =========================================
    // TOAST
    // =========================================
    showToast(message, type = "success") {
        const toast = $(`
            <div class="toast align-items-center text-bg-${type} border-0 position-fixed top-0 end-0 m-3 shadow">
                <div class="d-flex">
                    <div class="toast-body">${message}</div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto"
                        data-bs-dismiss="toast"></button>
                </div>
            </div>
        `);

        $("body").append(toast);

        const bsToast = new bootstrap.Toast(toast[0], { delay: 2500 });
        bsToast.show();

        toast.on("hidden.bs.toast", () => toast.remove());
    },

    // =========================================
    // DASHBOARD
    // =========================================
    renderDashboard(data) {
        $("#totalEmployees").text(data.totalEmployees);
        $("#activeEmployees").text(data.active);
        $("#inactiveEmployees").text(data.inactive);
        $("#departmentCount").text(data.totalDepartments);

        this.renderRecentEmployees(data.recentEmployees);
        this.renderDepartmentBreakdown(data.departmentBreakdown);
    },

    renderRecentEmployees(employees) {
        const container = $("#recentEmployees");
        container.empty();

        employees.forEach(e => {
            const initials = (e.firstName?.[0] || "") + (e.lastName?.[0] || "");

            container.append(`
                <div class="d-flex align-items-center justify-content-between mb-3 border-bottom pb-3">
                    <div class="d-flex align-items-center">
                        <div class="avatar bg-primary text-white me-3">
                            ${initials.toUpperCase()}
                        </div>
                        <div>
                            <h6 class="mb-0 fw-bold">${e.firstName} ${e.lastName}</h6>
                            <small class="text-muted">${e.designation}</small>
                        </div>
                    </div>
                    <span class="badge bg-primary">${e.department}</span>
                </div>
            `);
        });
    },

   renderDepartmentBreakdown(list) {
    const tbody = $("#departmentTableBody");
    tbody.empty();

    list.forEach(item => {

        const dept = item.department || item.Department;
        const count = item.count || item.Count;
        const percentage = (item.percentage ?? item.Percentage ?? 0);

        tbody.append(`
            <tr>
                <td>${dept}</td>
                <td>${count}</td>
                <td>${percentage.toFixed(1)}%</td>
            </tr>
        `);
    });
},

renderDepartmentFilter(list) {
    const select = $("#deptFilter");

    select.find("option:not(:first)").remove();

    list.forEach(item => {
        const dept = item.department || item.Department;
        select.append(`
            <option value="${dept}">${dept}</option>
        `);
    });
},

    // =========================================
    // MODALS
    // =========================================
    showModal(type, emp) {

      if (type === "view") {

    const firstName = emp.firstName || emp.FirstName || "";
    const lastName  = emp.lastName || emp.LastName || "";
    const status    = emp.status || emp.Status || "Inactive";

    const initials =
        (firstName?.[0] || "") + (lastName?.[0] || "");

    const date = emp.joinDate
        ? new Date(emp.joinDate).toLocaleDateString("en-GB", {
            day: "2-digit",
            month: "short",
            year: "numeric"
        })
        : "-";

    $("#viewFullName").text(`${firstName} ${lastName}`);
    $("#viewEmail").text(emp.email);
    $("#viewPhone").text(emp.phone);
    $("#viewDesignation").text(emp.designation);
    $("#viewSalary").text(`₹${emp.salary}`);

    $("#viewAvatar").text(initials.toUpperCase());
    $("#viewJoinDate").text(date);

    $("#viewStatus")
        .text(status)
        .removeClass("bg-success bg-danger")
        .addClass(status === "Active" ? "bg-success" : "bg-danger");

    new bootstrap.Modal(document.getElementById("viewEmployeeModal")).show();
}

        if (type === "edit") {

            this.fillForm(emp);

            $("#addEmployeeModalLabel").text("Edit Employee");
            $("#submitBtn").text("Update Employee");

            new bootstrap.Modal(document.getElementById("addEmployeeModal")).show();
        }
    }
};