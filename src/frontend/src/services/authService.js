import http from "./httpService";
import configInfo from '../config.json';

const apiEndpoint = configInfo.identity;
const tokenKey = "token";

http.setJwt(getJwt());

export async function register(user) {
  const response = await http.post(apiEndpoint + "/register?callbackUri=http://localhost:3000/login", {
    email: user.email,
    password: user.password,
  });
  return response;
}

export async function login(user) {
  const response = await http.post(apiEndpoint + "/login", { 
    email: user.email, 
    password: user.password 
  });
  if (response.data && response.data.succeeded) {
    localStorage.setItem(tokenKey, response.data.value.accessToken);
  }
  return response;
}

export async function forgotPassword(forgotPasswordDto) {
  const response = await http.post(apiEndpoint + "/forgotPassword", { 
    email: forgotPasswordDto.email,
  });
  return response;
}

export async function resetPassword(resetPasswordDto) {
  const response = await http.post(apiEndpoint + "/resetPassword", { 
    email: resetPasswordDto.email, 
    resetCode: resetPasswordDto.code,
    newPassword: resetPasswordDto.newPassword
  });
  return response;
}

export function loginWithJwt(jwt) {
  localStorage.setItem(tokenKey, jwt);
}

export function logout() {
  localStorage.removeItem(tokenKey);
}

export async function getCurrentUser() {
  const response = await http.get(apiEndpoint + "/account");
  return response;
}

export function getJwt() {
  return localStorage.getItem(tokenKey);
}

export async function hasRole(role) {
  try {
    const response = await getCurrentUser();
    if (response.data && response.data.succeeded) {
      const user = response.data.value;
      return user.roles.some(r => r == role);
    }
  } catch (error) {
    return false;
  }
}

export async function isAuthenticated() {
  try {
    const response = await getCurrentUser();
    return response.data && response.data.succeeded;
  } catch (error) {
    return false;
  }
}

export default {
  isAuthenticated,
  hasRole,
  forgotPassword,
  resetPassword,
  register,
  login,
  loginWithJwt,
  logout,
  getCurrentUser,
  getJwt
};
