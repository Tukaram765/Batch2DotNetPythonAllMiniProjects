import { CONFIG } from "./config.js";

export const AuthService = (function () {

    const STORAGE_KEY = "ems_session";
    let _session = null;

    // =========================================
    // LOAD SESSION
    // =========================================
 function loadSession() {
    const saved = localStorage.getItem(STORAGE_KEY);

    if (saved) {
        try {
            _session = JSON.parse(saved);
        } catch {
            _session = null;
        }
    }
}

    loadSession();

    // =========================================
    // SAVE SESSION
    // =========================================
   function saveSession(data) {
    _session = {
        username: data.username,
        role: data.role || "Viewer",
        token: data.token
    };

    localStorage.setItem(STORAGE_KEY, JSON.stringify(_session));
}

    // =========================================
    // CLEAR SESSION
    // =========================================
    function clearSession() {
        _session = null;
        localStorage.removeItem(STORAGE_KEY);
    }

    // =========================================
    // LOGIN
    // =========================================
    async function login(credentials) {

        console.log("RAW CREDENTIALS:", credentials);

        const payload = {
            username: credentials.username || credentials.Username,
            passwordHash: credentials.password || credentials.Password
        };

        console.log("LOGIN PAYLOAD:", payload);

        const res = await fetch(`${CONFIG.API_BASE_URL}/auth/login`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(payload)
        });

        const text = await res.text();
        console.log("RAW RESPONSE:", text);

        let data;
        try {
            data = JSON.parse(text);
        } catch {
            throw new Error("Backend did not return valid JSON");
        }

        if (!res.ok || !data.success) {
            throw new Error(data.message || "Login failed");
        }

        saveSession(data);

        return data;
    }

   function saveSession(data) {
    localStorage.setItem("token", data.token);
    localStorage.setItem("username", data.username);
    localStorage.setItem("role", data.role);
}

  

    // =========================================
    // REGISTER
    // =========================================
    async function register(user) {

        const payload = {
            username: user.username,
            passwordHash: user.password,     // 👈 FIXED (backend expects Password, not PasswordHash)
            role: user.role || "Viewer"
        };

        const res = await fetch(`${CONFIG.API_BASE_URL}/auth/register`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(payload)
        });

        const text = await res.text();

        let data;
        try {
            data = JSON.parse(text);
        } catch {
            throw new Error("Backend did not return valid JSON");
        }

        if (!res.ok || !data.success) {
            throw new Error(data.message || "Registration failed");
        }

        return data;
    }

    // =========================================
    // GETTERS
    // =========================================
   function getToken() {
    return localStorage.getItem("token");
}

    function getUser() {
        return _session || null;
    }

    function getRole() {
    return localStorage.getItem("role") || "Viewer";
}

    function isLoggedIn() {
        return !!_session?.token;
    }

    function isAdmin() {
        return getRole() === "Admin";
    }

    // =========================================
    // LOGOUT
    // =========================================
    function logout() {
        clearSession();
    }

    return {
        login,
        register,
        getToken,
        getUser,
        getRole,
        isLoggedIn,
        isAdmin,
        logout
    };

})();