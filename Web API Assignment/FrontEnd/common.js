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

function getJwtPayload() {
  const token = getToken();
  if (!token) return null;
  try {
    return JSON.parse(atob(token.split(".")[1]));
  } catch {
    return null;
  }
}

function getUserRole() {
  const token = getToken();
  if (!token) return null;
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const roleClaim = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        if(roleClaim) return roleClaim;
        else return null
    }
    catch {
    return null;
  }
}



function restrictAccess(allowedRoles) {
  const role = getUserRole();
  if (!role) {
    alert("You must log in first.");
    window.location.href = "index.html";
    return;
  }
  if (!allowedRoles.includes(role)) {
    alert("Access denied!");
    window.location.href = "dashboard.html";
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
