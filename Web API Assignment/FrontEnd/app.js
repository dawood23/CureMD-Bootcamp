// ===== CONFIG =====
const API_BASE = "/api"; // same origin (Kestrel). If different port: "https://localhost:5001/api"

// ===== UTIL =====
const App = (function () {
  function setToken(t) {
    localStorage.setItem("token", t);
  }
  function getToken() {
    return localStorage.getItem("token");
  }
  function logout() {
    localStorage.removeItem("token");
  }

  function parseJwt(token) {
    try {
      const payload = token.split(".")[1];
      return JSON.parse(atob(payload.replace(/-/g, "+").replace(/_/g, "/")));
    } catch {
      return null;
    }
  }
  function getUserIdFromToken() {
    const t = getToken();
    if (!t) return null;
    const p = parseJwt(t);
    return p && (p.userId || p.sub || p.nameid);
  }
  function getUsernameFromToken() {
    const t = getToken();
    if (!t) return null;
    const p = parseJwt(t);
    return (
      p &&
      (p.unique_name ||
        p.username ||
        p["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] ||
        p.sub)
    );
  }

  // global AJAX auth header
  $.ajaxSetup({
    beforeSend: function (xhr) {
      const t = getToken();
      if (t) xhr.setRequestHeader("Authorization", "Bearer " + t);
    },
  });

  function ajaxJSON(url, method, data) {
    return $.ajax({
      url,
      method,
      contentType: "application/json",
      data: data ? JSON.stringify(data) : undefined,
    });
  }

  // ===== AUTH =====
  function login(username, password) {
    return ajaxJSON(`${API_BASE}/Auth/login`, "POST", {
      username,
      password,
    }).then((res) => {
      if (!res || !res.token) throw new Error("Invalid response from server");
      setToken(res.token);
      return res;
    });
  }

  // ====== VISIT TYPES ======
  const visitTypes = {
    load() {
      $("#vt-msg").text("");
      $.get(`${API_BASE}/VisitTypes`)
        .done((list) => {
          const tb = $("#vt-grid tbody");
          tb.empty();
          list.forEach((v) => {
            tb.append(`<tr>
              <td>${v.visitTypeID}</td>
              <td>${v.typeName}</td>
              <td>${v.baseFee}</td>
              <td>${v.estimatedDuration}</td>
              <td class="action">
                <button onclick="App.visitTypes.fill(${
                  v.visitTypeID
                }, '${escapeHtml(v.typeName)}', ${v.baseFee}, ${
              v.estimatedDuration
            })">Edit</button>
                <button class="del" onclick="App.visitTypes.remove(${
                  v.visitTypeID
                })">Delete</button>
              </td>
            </tr>`);
          });
        })
        .fail((err) => $("#vt-msg").text("Failed to load visit types"));
    },
    fill(id, name, fee, dur) {
      $("#vt-id").val(id);
      $("#vt-name").val(unescapeHtml(name));
      $("#vt-fee").val(fee);
      $("#vt-duration").val(dur);
      $("#vt-form-title").text("Edit Visit Type");
    },
    reset() {
      $("#vt-id").val("");
      $("#vt-name").val("");
      $("#vt-fee").val("");
      $("#vt-duration").val("");
      $("#vt-form-title").text("Add / Edit Visit Type");
      $("#vt-msg").text("");
    },
    save() {
      const id = +($("#vt-id").val() || 0);
      const payload = {
        visitTypeID: id,
        typeName: $("#vt-name").val().trim(),
        baseFee: parseFloat($("#vt-fee").val()),
        estimatedDuration: parseInt($("#vt-duration").val() || "0", 10),
      };
      if (!payload.typeName) {
        $("#vt-msg").text("Type name is required");
        return;
      }
      if (isNaN(payload.baseFee) || payload.baseFee < 0) {
        $("#vt-msg").text("Base fee must be >= 0");
        return;
      }

      const userId = getUserIdFromToken() || 1;
      const method = id ? "PUT" : "POST";
      ajaxJSON(`${API_BASE}/VisitTypes/${userId}`, method, payload)
        .done(() => {
          $("#vt-msg").text("Saved");
          App.visitTypes.load();
          App.visitTypes.reset();
        })
        .fail(() => $("#vt-msg").text("Save failed"));
    },
    remove(id) {
      const userId = getUserIdFromToken() || 1;
      $.ajax({
        url: `${API_BASE}/VisitTypes/${id}/${userId}`,
        method: "DELETE",
      })
        .done(() => App.visitTypes.load())
        .fail(() => alert("Delete failed"));
    },
  };

  // ====== DOCTORS ======
  const doctors = {
    load() {
      $("#dr-msg").text("");
      $.get(`${API_BASE}/Doctors`)
        .done((list) => {
          const tb = $("#dr-grid tbody");
          tb.empty();
          list.forEach((d) => {
            tb.append(`<tr>
             <td>${d.doctorID}</td>
             <td>${d.firstName}</td>
             <td>${d.lastName}</td>
             <td>${d.phoneNumber || ""}</td>
             <td>${d.email || ""}</td>
             <td class="action">
               <button onclick="App.doctors.fill(${d.doctorID}, '${escapeHtml(
              d.firstName
            )}', '${escapeHtml(d.lastName)}', '${escapeHtml(
              d.phoneNumber || ""
            )}', '${escapeHtml(d.email || "")}')">Edit</button>
               <button class="del" onclick="App.doctors.remove(${
                 d.doctorID
               })">Delete</button>
             </td>
           </tr>`);
          });
        })
        .fail(() => $("#dr-msg").text("Failed to load doctors"));
    },
    fill(id, f, l, p, e) {
      $("#dr-id").val(id);
      $("#dr-first").val(unescapeHtml(f));
      $("#dr-last").val(unescapeHtml(l));
      $("#dr-phone").val(unescapeHtml(p));
      $("#dr-email").val(unescapeHtml(e));
      $("#dr-form-title").text("Edit Doctor");
    },
    reset() {
      $("#dr-id,#dr-first,#dr-last,#dr-phone,#dr-email").val("");
      $("#dr-form-title").text("Add / Edit Doctor");
      $("#dr-msg").text("");
    },
    save() {
      const id = +($("#dr-id").val() || 0);
      const payload = {
        doctorID: id,
        firstName: $("#dr-first").val().trim(),
        lastName: $("#dr-last").val().trim(),
        phoneNumber: $("#dr-phone").val().trim(),
        email: $("#dr-email").val().trim(),
      };
      if (!payload.firstName || !payload.lastName) {
        $("#dr-msg").text("First/Last name required");
        return;
      }
      const userId = getUserIdFromToken() || 1;
      const method = id ? "PUT" : "POST";
      ajaxJSON(`${API_BASE}/Doctors/${userId}`, method, payload)
        .done(() => {
          $("#dr-msg").text("Saved");
          doctors.load();
          doctors.reset();
        })
        .fail(() => $("#dr-msg").text("Save failed"));
    },
    remove(id) {
      const userId = getUserIdFromToken() || 1;
      $.ajax({ url: `${API_BASE}/Doctors/${id}/${userId}`, method: "DELETE" })
        .done(() => doctors.load())
        .fail(() => alert("Delete failed"));
    },
  };

  // ====== PATIENTS ======
  const patients = {
    load() {
      $("#pt-msg").text("");
      $.get(`${API_BASE}/Patients`)
        .done((list) => {
          const tb = $("#pt-grid tbody");
          tb.empty();
          list.forEach((p) => {
            tb.append(`<tr>
              <td>${p.patientID}</td>
              <td>${p.firstName}</td>
              <td>${p.lastName}</td>
              <td>${p.dateOfBirth ? p.dateOfBirth.substring(0, 10) : ""}</td>
              <td>${p.phoneNumber || ""}</td>
              <td>${p.email || ""}</td>
              <td class="action">
                <button onclick="App.patients.fill(${
                  p.patientID
                }, '${escapeHtml(p.firstName)}', '${escapeHtml(
              p.lastName
            )}', '${
              p.dateOfBirth ? p.dateOfBirth.substring(0, 10) : ""
            }', '${escapeHtml(p.phoneNumber || "")}', '${escapeHtml(
              p.email || ""
            )}', '${escapeHtml(p.address || "")}', '${escapeHtml(
              p.emergencyContact || ""
            )}')">Edit</button>
                <button class="del" onclick="App.patients.remove(${
                  p.patientID
                })">Delete</button>
              </td>
            </tr>`);
          });
        })
        .fail(() => $("#pt-msg").text("Failed to load patients"));
    },
    fill(id, f, l, dob, ph, em, addr, emc) {
      $("#pt-id").val(id);
      $("#pt-first").val(unescapeHtml(f));
      $("#pt-last").val(unescapeHtml(l));
      $("#pt-dob").val(dob || "");
      $("#pt-phone").val(unescapeHtml(ph));
      $("#pt-email").val(unescapeHtml(em));
      $("#pt-address").val(unescapeHtml(addr));
      $("#pt-emergency").val(unescapeHtml(emc));
      $("#pt-form-title").text("Edit Patient");
    },
    reset() {
      $(
        "#pt-id,#pt-first,#pt-last,#pt-dob,#pt-phone,#pt-email,#pt-address,#pt-emergency"
      ).val("");
      $("#pt-form-title").text("Add / Edit Patient");
      $("#pt-msg").text("");
    },
    save() {
      const id = +($("#pt-id").val() || 0);
      const payload = {
        patientID: id,
        firstName: $("#pt-first").val().trim(),
        lastName: $("#pt-last").val().trim(),
        dateOfBirth: $("#pt-dob").val() || null,
        phoneNumber: $("#pt-phone").val().trim(),
        email: $("#pt-email").val().trim(),
        address: $("#pt-address").val().trim(),
        emergencyContact: $("#pt-emergency").val().trim(),
      };
      if (!payload.firstName || !payload.lastName) {
        $("#pt-msg").text("First/Last name required");
        return;
      }
      const userId = getUserIdFromToken() || 1;
      const method = id ? "PUT" : "POST";
      ajaxJSON(`${API_BASE}/Patients/${userId}`, method, payload)
        .done(() => {
          $("#pt-msg").text("Saved");
          patients.load();
          patients.reset();
        })
        .fail(() => $("#pt-msg").text("Save failed"));
    },
    remove(id) {
      const userId = getUserIdFromToken() || 1;
      $.ajax({ url: `${API_BASE}/Patients/${id}/${userId}`, method: "DELETE" })
        .done(() => patients.load())
        .fail(() => alert("Delete failed"));
    },
  };

  // ====== USERS ======
  const users = {
    load() {
      $("#us-msg").text("");
      $.get(`${API_BASE}/Users`)
        .done((list) => {
          const tb = $("#us-grid tbody");
          tb.empty();
          list.forEach((u) => {
            tb.append(`<tr>
              <td>${u.userID}</td>
              <td>${u.username}</td>
              <td>${u.firstName} ${u.lastName}</td>
              <td>${u.roleID}</td>
              <td class="action">
                <button onclick="App.users.fill(${u.userID}, '${escapeHtml(
              u.username
            )}', '${escapeHtml(u.passwordHash || "")}', ${
              u.roleID
            }, '${escapeHtml(u.firstName)}', '${escapeHtml(
              u.lastName
            )}')">Edit</button>
                <button class="del" onclick="App.users.remove(${
                  u.userID
                })">Delete</button>
              </td>
            </tr>`);
          });
        })
        .fail(() => $("#us-msg").text("Failed to load users"));
    },
    fill(id, un, ph, role, fn, ln) {
      $("#us-id").val(id);
      $("#us-username").val(unescapeHtml(un));
      $("#us-passhash").val(unescapeHtml(ph));
      $("#us-role").val(role);
      $("#us-first").val(unescapeHtml(fn));
      $("#us-last").val(unescapeHtml(ln));
      $("#us-form-title").text("Edit User");
    },
    reset() {
      $("#us-id,#us-username,#us-passhash,#us-role,#us-first,#us-last").val("");
      $("#us-form-title").text("Add / Edit User");
      $("#us-msg").text("");
    },
    save() {
      const id = +($("#us-id").val() || 0);
      const payload = {
        userID: id,
        username: $("#us-username").val().trim(),
        passwordHash: $("#us-passhash").val().trim(),
        roleID: parseInt($("#us-role").val() || "0", 10),
        firstName: $("#us-first").val().trim(),
        lastName: $("#us-last").val().trim(),
      };
      if (
        !payload.username ||
        !payload.passwordHash ||
        !payload.firstName ||
        !payload.lastName
      ) {
        $("#us-msg").text("All fields required");
        return;
      }
      const userId = getUserIdFromToken() || 1;
      const method = id ? "PUT" : "POST";
      ajaxJSON(`${API_BASE}/Users/${userId}`, method, payload)
        .done(() => {
          $("#us-msg").text("Saved");
          users.load();
          users.reset();
        })
        .fail(() => $("#us-msg").text("Save failed"));
    },
    remove(id) {
      const userId = getUserIdFromToken() || 1;
      $.ajax({ url: `${API_BASE}/Users/${id}/${userId}`, method: "DELETE" })
        .done(() => users.load())
        .fail(() => alert("Delete failed"));
    },
  };

  // ====== VISITS ======
  const visits = {
    load() {
      $("#vi-msg").text("");
      $.get(`${API_BASE}/Visits`)
        .done((list) => {
          const tb = $("#vi-grid tbody");
          tb.empty();
          list.forEach((v) => {
            tb.append(`<tr>
              <td>${v.visitID}</td>
              <td>${v.patientID}</td>
              <td>${v.doctorID ?? ""}</td>
              <td>${v.visitTypeID}</td>
              <td>${v.visitDate ? v.visitDate.substring(0, 10) : ""}</td>
              <td>${v.visitTime ?? ""}</td>
              <td>${v.status ?? ""}</td>
              <td>${v.fee ?? ""}</td>
              <td class="action">
                <button onclick="App.visits.fill(${v.visitID}, ${
              v.patientID
            }, ${v.doctorID ?? "null"}, ${v.visitTypeID}, '${
              v.visitDate ? v.visitDate.substring(0, 10) : ""
            }', '${v.visitTime ?? ""}', '${escapeHtml(
              v.description || ""
            )}', '${escapeHtml(v.notes || "")}', '${escapeHtml(
              v.status || "Scheduled"
            )}', ${v.fee ?? "null"})">Edit</button>
                <button class="del" onclick="App.visits.remove(${
                  v.visitID
                })">Delete</button>
              </td>
            </tr>`);
          });
        })
        .fail(() => $("#vi-msg").text("Failed to load visits"));
    },
    fill(id, p, d, t, date, time, desc, notes, status, fee) {
      $("#vi-id").val(id);
      $("#vi-patient").val(p);
      $("#vi-doctor").val(d ?? "");
      $("#vi-type").val(t);
      $("#vi-date").val(date || "");
      $("#vi-time").val(time || "");
      $("#vi-desc").val(unescapeHtml(desc));
      $("#vi-notes").val(unescapeHtml(notes));
      $("#vi-status").val(status || "Scheduled");
      $("#vi-fee").val(fee ?? "");
      $("#vi-form-title").text("Edit Visit");
    },
    reset() {
      $(
        "#vi-id,#vi-patient,#vi-doctor,#vi-type,#vi-date,#vi-time,#vi-desc,#vi-notes,#vi-fee"
      ).val("");
      $("#vi-status").val("Scheduled");
      $("#vi-form-title").text("Add / Edit Visit");
      $("#vi-msg").text("");
    },
    save() {
      const id = +($("#vi-id").val() || 0);
      const payload = {
        visitID: id,
        patientID: parseInt($("#vi-patient").val() || "0", 10),
        doctorID: $("#vi-doctor").val()
          ? parseInt($("#vi-doctor").val(), 10)
          : null,
        visitTypeID: parseInt($("#vi-type").val() || "0", 10),
        visitDate: $("#vi-date").val(),
        visitTime: $("#vi-time").val(),
        description: $("#vi-desc").val().trim(),
        notes: $("#vi-notes").val().trim(),
        status: $("#vi-status").val(),
        fee: $("#vi-fee").val() ? parseFloat($("#vi-fee").val()) : null,
        createdBy: getUserIdFromToken() || 1, // matches schema (Visits.CreatedBy NOT NULL)
      };
      if (
        !payload.patientID ||
        !payload.visitTypeID ||
        !payload.visitDate ||
        !payload.visitTime
      ) {
        $("#vi-msg").text("Patient, VisitType, Date & Time are required");
        return;
      }
      const performedByUserId = getUserIdFromToken() || 1;
      const method = id ? "PUT" : "POST";
      ajaxJSON(`${API_BASE}/Visits/${performedByUserId}`, method, payload)
        .done(() => {
          $("#vi-msg").text("Saved");
          visits.load();
          visits.reset();
        })
        .fail(() => $("#vi-msg").text("Save failed"));
    },
    remove(id) {
      const performedByUserId = getUserIdFromToken() || 1;
      $.ajax({
        url: `${API_BASE}/Visits/${id}/${performedByUserId}`,
        method: "DELETE",
      })
        .done(() => visits.load())
        .fail(() => alert("Delete failed"));
    },
  };

  // ====== ACTIVITY LOGS (read-only) ======
  const logs = {
    load() {
      $.get(`${API_BASE}/ActivityLog`)
        .done((list) => {
          const tb = $("#log-grid tbody");
          tb.empty();
          list.forEach((l) => {
            tb.append(`<tr>
              <td>${l.logID}</td>
              <td>${l.userID}</td>
              <td>${l.action}</td>
              <td>${l.tableAffected}</td>
              <td>${l.recordID ?? ""}</td>
              <td>${l.status}</td>
              <td>${l.timestamp ? formatDateTime(l.timestamp) : ""}</td>
            </tr>`);
          });
        })
        .fail(() => alert("Failed to load logs"));
    },
  };

  // helpers
  function escapeHtml(s) {
    return String(s).replace(
      /[&<>"']/g,
      (m) =>
        ({
          "&": "&amp;",
          "<": "&lt;",
          ">": "&gt;",
          '"': "&quot;",
          "'": "&#39;",
        }[m])
    );
  }
  function unescapeHtml(s) {
    const e = document.createElement("textarea");
    e.innerHTML = s;
    return e.value;
  }
  function formatDateTime(v) {
    try {
      return new Date(v).toLocaleString();
    } catch {
      return v;
    }
  }

  return {
    login,
    getToken,
    logout,
    getUserIdFromToken,
    getUsernameFromToken,
    visitTypes,
    doctors,
    patients,
    users,
    visits,
    logs,
  };
})();
