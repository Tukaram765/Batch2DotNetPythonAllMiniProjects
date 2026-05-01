import { authService } from "../js/authService.js";

describe("Auth Service Tests", () => {

  test("Signup Success", () => {
    const res = authService.signup("user_101", "pass_101");
    expect(res.success).toBe(true);
  });

  test("Duplicate Username", () => {
    authService.signup("dup_user_202", "pass_202");
    const res = authService.signup("dup_user_202", "pass_202");

    expect(res.error).toBeDefined();
  });

  test("Login Success", () => {
    authService.signup("login_user_303", "pass_303");
    const res = authService.login("login_user_303", "pass_303");

    expect(res.success).toBe(true);
  });

  test("Login Failure", () => {
    const res = authService.login("wrong_user_404", "wrong_pass_404");
    expect(res.error).toBeDefined();
  });

  test("Logout", () => {
    authService.signup("logout_user_505", "pass_505");
    authService.login("logout_user_505", "pass_505");
    authService.logout();

    expect(authService.isLoggedIn()).toBe(false);
  });

});