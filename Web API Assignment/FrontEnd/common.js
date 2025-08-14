// /js/common.js
const API_BASE = "https://localhost:5001/api"; // Change if needed

function getToken() {
  return localStorage.getItem("jwtToken");
}

function setToken(token) {
  localStorage.setItem("jwtToken", token);
}

function clearToken() {
  localStorage.removeItem("jwtToken");
  window.location.href = "index.html";
}

// Automatically add Authorization header to all AJAX calls
$.ajaxSetup({
  beforeSend: function (xhr) {
    let token = getToken();
    if (token) {
      xhr.setRequestHeader("Authorization", "Bearer " + token);
    }
  },
  error: function (xhr) {
    if (xhr.status === 401) {
      alert("Unauthorized. Please log in again.");
      clearToken();
    }
  },
});
