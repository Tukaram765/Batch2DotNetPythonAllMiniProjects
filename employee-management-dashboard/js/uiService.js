
export const uiService = {

  //VIEW MANAGEMENT
  showSection(sectionClass) {
    $(".view").hide();
    $(sectionClass).show();
  },

  // EMPLOYEE TABLE
  renderEmployees(data) {
    const tbody = $("#employeeTableBody");
    tbody.empty();

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

      // Avatar
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

          <td class="text-center">
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

  // PREFILL FORM
 fillForm(emp) {
  const form = $("#addEmployeeForm")[0];

  Object.keys(emp).forEach(key => {
    if (form[key]) form[key].value = emp[key];
  });

  if (form["id"]) form["id"].value = emp.id;
},

  // INLINE VALIDATION
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

  // NOTIFICATIONS
  showToast(message, type = "success") {
    const toast = $(`
      <div class="toast align-items-center text-bg-${type} border-0 position-fixed top-0 end-0 m-3 shadow">
        <div class="d-flex">
          <div class="toast-body">${message}</div>
          <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
        </div>
      </div>
    `);

    $("body").append(toast);
    const bsToast = new bootstrap.Toast(toast[0], { delay: 2500 });
    bsToast.show();
    toast.on("hidden.bs.toast", () => toast.remove());
  },

  // RECENT EMPLOYEES
  renderRecentEmployees(employees) {
    const container = $(".employee-list");
    container.empty();

    if (employees.length === 0) {
      container.append(`<p class="text-muted">No recent employees</p>`);
      return;
    }

    employees.forEach(e => {
      const initials = e.firstName[0] + e.lastName[0];

      container.append(`
        <div class="d-flex align-items-center justify-content-between mb-3 border-bottom pb-3">
          <div class="d-flex align-items-center">
            <div class="avatar bg-primary text-white me-3">${initials}</div>
            <div>
              <h6 class="mb-0 fw-bold">${e.firstName} ${e.lastName}</h6>
              <small class="text-muted">${e.designation}</small>
            </div>
          </div>
          <div class="d-flex gap-2">
            <span class="badge bg-primary-subtle text-primary border border-primary px-3">${e.department}</span>
            <span class="badge ${e.status === "Active" ? "bg-success" : "bg-danger"} px-3">${e.status}</span>
          </div>
        </div>
      `);
    });
  },

  // DEPARTMENT BREAKDOWN
 renderDepartmentBreakdown(breakdown) {
  const tbody = $("#departmentTableBody");
  tbody.empty();

  const total = Object.values(breakdown).reduce((a, b) => a + b, 0);

  if (total === 0) {
    tbody.append(`<tr><td colspan="4" class="text-center text-muted">No data</td></tr>`);
    return;
  }

  Object.keys(breakdown).forEach(dept => {
    const count = breakdown[dept];
    const percent = ((count / total) * 100).toFixed(0);

    // Optional dynamic colors
    let colorClass = "bg-primary";
    if (dept === "Marketing") colorClass = "bg-warning";
    else if (dept === "HR") colorClass = "bg-info";
    else if (dept === "Finance") colorClass = "bg-success";
    else if (dept === "Operations") colorClass = "bg-secondary";

    tbody.append(`
      <tr>
        <td>
          <span class="badge rounded-pill ${colorClass} px-3">
            ${dept}
          </span>
        </td>

        <td class="text-center fw-bold">${count}</td>

        <td style="width:40%">
          <div class="progress custom-progress">
            <div class="progress-bar ${colorClass}" style="width:${percent}%"></div>
          </div>
        </td>

        <td class="text-end text-muted">${percent}%</td>
      </tr>
    `);
  });
},

  // MODAL VIEW / EDIT
  showModal(type, emp) {
   if (type === "view") {

  const initials = emp.firstName[0] + emp.lastName[0];

  $("#viewAvatar").text(initials.toUpperCase());
  $("#viewFullName").text(`${emp.firstName} ${emp.lastName}`);
  $("#viewDeptBadge").text(emp.department);

  $("#viewEmail").text(emp.email);
  $("#viewPhone").text(emp.phone);
  $("#viewDesignation").text(emp.designation);
  $("#viewSalary").text(`₹${Number(emp.salary).toLocaleString("en-IN")}`);

  $("#viewJoinDate").text(
    new Date(emp.joinDate).toLocaleDateString("en-GB", {
      day: "2-digit",
      month: "short",
      year: "numeric"
    })
  );

  $("#viewStatusBadge")
    .text(emp.status)
    .removeClass("bg-success bg-danger")
    .addClass(emp.status === "Active" ? "bg-success" : "bg-danger");

  new bootstrap.Modal(document.getElementById("viewEmployeeModal")).show();
}else if (type === "edit") {
  this.fillForm(emp);

  $("#addEmployeeModalLabel").text("Edit Employee");
  $("#submitBtn").text("Update Employee");

  new bootstrap.Modal(document.getElementById("addEmployeeModal")).show();
}
  }
};