// dashboardService.js
// Fetches dashboard data from API (single call)

import { storageService } from "./storageService.js";

export const dashboardService = {

    // =========================================
    // GET DASHBOARD SUMMARY (FROM API)
    // =========================================
    async getSummary() {
        return await storageService.getDashboard();
    }

};