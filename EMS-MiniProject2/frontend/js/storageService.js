import { CONFIG } from "./config.js";
import { AuthService } from "./authService.js";

export const storageService = (function () {

    // =========================================
    // COMMON HEADERS (WITH JWT TOKEN)
    // =========================================
    function _headers(withAuth = true) {
        const headers = {
            "Content-Type": "application/json"
        };

        if (withAuth) {
            const token = AuthService.getToken?.();
            console.log("TOKEN:", token);

            if (token) {
                headers["Authorization"] = `Bearer ${token}`;
            }
        }

        return headers;
    }

    // =========================================
    // GET ALL
    // =========================================
    async function getAll(params = {}) {
        const query = new URLSearchParams(params).toString();

        const res = await fetch(
            `${CONFIG.API_BASE_URL}/employees?${query}`,
            {
                method: "GET",
                headers: _headers()
            }
        );

        const text = await res.text();
        console.log("GET ALL RESPONSE:", text);

        if (!res.ok) {
            throw new Error(text || "Failed to fetch employees");
        }

        return JSON.parse(text);
    }

    // =========================================
    // GET BY ID
    // =========================================
    async function getById(id) {
        const res = await fetch(
            `${CONFIG.API_BASE_URL}/employees/${id}`,
            {
                method: "GET",
                headers: _headers()
            }
        );

        const text = await res.text();
        console.log("GET BY ID RESPONSE:", text);

        if (!res.ok) {
            throw new Error(text || "Employee not found");
        }

        return JSON.parse(text);
    }

 // =========================================
//  ADD EMPLOYEE (FINAL FIX)
// =========================================
async function add(emp) {

    console.log("ORIGINAL EMP:", emp);

    //  FIX: CREATE CLEAN PAYLOAD (NO ID, NO EXTRA FIELDS)
    const payload = {
        firstName: emp.firstName,
        lastName: emp.lastName,
        email: emp.email,
        phone: emp.phone,
        department: emp.department,
        designation: emp.designation,
        salary: Number(emp.salary),
        joinDate: emp.joinDate,
        status: emp.status
    };

    console.log(" FINAL PAYLOAD:", payload);

    const res = await fetch(
        `${CONFIG.API_BASE_URL}/employees`,
        {
            method: "POST",
            headers: _headers(),
            body: JSON.stringify(payload) 
        }
    );

    const text = await res.text();
    console.log("ADD RESPONSE:", text);

     if (!res.ok) {

        // HANDLE 403 HERE
        if (res.status === 403) {
            throw new Error("You are not authorized to perform this action");
        }

        if (res.status === 401) {
            throw new Error("Session expired. Please login again");
        }

        throw new Error(text || "Failed to add employee");
    }

    return JSON.parse(text);
}

    // =========================================
    // UPDATE EMPLOYEE
    // =========================================
    async function update(id, updatedData) {

        console.log(" UPDATING:", id, updatedData);

        const res = await fetch(
            `${CONFIG.API_BASE_URL}/employees/${id}`,
            {
                method: "PUT",
                headers: _headers(),
                body: JSON.stringify(updatedData)
            }
        );

        const text = await res.text();
        console.log(" UPDATE RESPONSE:", text);

        if (!res.ok) {
            try {
                const err = JSON.parse(text);

                if (err.errors) {
                    const messages = Object.values(err.errors)
                        .flat()
                        .join(", ");

                    throw new Error(messages);
                }

                throw new Error(err.message || text);

            } catch {
                throw new Error(text || "You are not Authorized");
            }
        }

        return JSON.parse(text);
    }

    // =========================================
    //  DELETE EMPLOYEE
    // =========================================
    async function remove(id) {

        console.log(" DELETING:", id);

        const res = await fetch(
            `${CONFIG.API_BASE_URL}/employees/${id}`,
            {
                method: "DELETE",
                headers: _headers()
            }
        );

        const text = await res.text();
        console.log(" DELETE RESPONSE:", text);

        if (!res.ok) {

    // HANDLE SPECIFIC STATUS CODES
    if (res.status === 403) {
        throw new Error("You are not authorized");
    }

    if (res.status === 401) {
        throw new Error("Session expired");
    }

    throw new Error(text || "Failed to delete employee");
}

        return true;
    }

    // =========================================
    //  DASHBOARD DATA
    // =========================================
    async function getDashboard() {
        const res = await fetch(
            `${CONFIG.API_BASE_URL}/employees/dashboard`,
            {
                method: "GET",
                headers: _headers()
            }
        );

        const text = await res.text();
        console.log(" DASHBOARD RESPONSE:", text);

        if (!res.ok) {
            throw new Error(text || "Failed to fetch dashboard");
        }

        return JSON.parse(text);
    }

    return {
        getAll,
        getById,
        add,
        update,
        remove,
        getDashboard
    };

})();