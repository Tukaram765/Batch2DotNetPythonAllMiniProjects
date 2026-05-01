
import { admins } from "./data.js";

let users = [...admins]; 


let currentUser = null;

export const authService = {

  signup(username, password) {
    const exists = users.find(u => u.username === username);
    if (exists) return { error: "Username already exists" };

    users.push({ username, password });
    return { success: true };
  },

  login(username, password) {
    const user = users.find(
      u => u.username === username && u.password === password
    );

    if (!user) return { error: "Invalid credentials" };

    currentUser = user;
    return { success: true };

    
  },

  logout() {
    currentUser = null;
  },

  isLoggedIn() {
    return currentUser !== null;
  }
};