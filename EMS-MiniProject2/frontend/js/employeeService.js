import { storageService } from "./storageService.js";

export const employeeService = {

    // =========================================
    //  GET ALL
    // =========================================
    async getAll(params = {}) {
        try {
            const data = await storageService.getAll(params);
            console.log("GET ALL SUCCESS:", data);
            return data;
        } catch (err) {
            console.error("GET ALL ERROR:", err);
            throw new Error(err.message || "Failed to fetch employees");
        }
    },

    // =========================================
    // GET BY ID
    // =========================================
    async getById(id) {
        try {
            const data = await storageService.getById(id);
            console.log("GET BY ID SUCCESS:", data);
            return data;
        } catch (err) {
            console.error("GET BY ID ERROR:", err);
            throw new Error(err.message || "Failed to fetch employee");
        }
    },

    // =========================================
    // ADD EMPLOYEE
    // =========================================
    async add(data) {
        try {
            console.log("ADDING EMPLOYEE:", data);

            const res = await storageService.add(data);

            console.log("ADD SUCCESS:", res);
            return res;

        } catch (err) {
            console.error("ADD ERROR FULL:", err);

            //  FIX: Extract validation errors properly
            if (err.message) {
                try {
                    const parsed = JSON.parse(err.message);

                    // Case 1: ASP.NET validation errors
                    if (parsed.errors) {
                        const messages = Object.values(parsed.errors)
                            .flat()
                            .join("\n");

                        throw new Error(messages);
                    }

                    // Case 2: Custom API message (like email exists)
                    if (parsed.message) {
                        throw new Error(parsed.message);
                    }

                } catch {
                    // if not JSON, just show raw message
                    throw new Error(err.message);
                }
            }

            throw new Error("Failed to add employee");
        }
    },

    // =========================================
    // UPDATE EMPLOYEE
    // =========================================
    async update(id, data) {
        try {
            console.log("UPDATING EMPLOYEE:", id, data);

            const res = await storageService.update(id, data);

            console.log("UPDATE SUCCESS:", res);
            return res;

        } catch (err) {
            console.error("UPDATE ERROR FULL:", err);

            if (err.message) {
                try {
                    const parsed = JSON.parse(err.message);

                    if (parsed.errors) {
                        const messages = Object.values(parsed.errors)
                            .flat()
                            .join("\n");

                        throw new Error(messages);
                    }

                    if (parsed.message) {
                        throw new Error(parsed.message);
                    }

                } catch {
                    throw new Error(err.message);
                }
            }

            throw new Error("Failed to update employee");
        }
    },

    // =========================================
    // DELETE EMPLOYEE
    // =========================================
    async remove(id) {
        try {
            console.log("DELETING EMPLOYEE:", id);

            await storageService.remove(id);

            console.log("DELETE SUCCESS");
            return true;

        } catch (err) {
            console.error("DELETE ERROR:", err);
            throw new Error(err.message || "Failed to delete employee");
        }
    }
};