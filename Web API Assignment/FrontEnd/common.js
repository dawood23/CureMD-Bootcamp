const API_BASE = "http://localhost:5243/api"; 

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

function getPerformedByUserId() {
  const token = getToken();
  if (!token) return null;
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.userId || payload.userID || payload.UserId || null;
  } catch (e) {
    return null;
  }
}

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
