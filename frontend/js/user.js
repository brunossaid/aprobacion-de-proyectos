import { initializeApp } from "./main.js";

// login
export function loginView() {
  fetch("pages/login.html")
    .then((res) => res.text())
    .then((html) => {
      document.getElementById("main").innerHTML = html;
      setupAuthForm();
      loadUsers();
    });
}

// cargar usuarios para el select
async function loadUsers() {
  try {
    const res = await fetch("http://localhost:5103/api/User");
    if (!res.ok) throw new Error("Error al cargar usuarios");
    const users = await res.json();

    const select = document.getElementById("user-select");
    if (!select) return;

    users.forEach((user) => {
      const option = document.createElement("option");
      option.value = user.id;
      option.textContent = user.name;
      select.appendChild(option);
    });
  } catch (error) {
    console.error(error);
  }
}

// manejar login del form
function setupAuthForm() {
  const form = document.getElementById("auth-form");
  form?.addEventListener("submit", (e) => {
    e.preventDefault();
    const selectedUser = document.getElementById("user-select")?.value;
    if (selectedUser) {
      localStorage.setItem("user", selectedUser);
      const selectedUserText =
        document.getElementById("user-select").selectedOptions[0].textContent;
      console.log(`hola ${selectedUserText}!`);
      initializeApp();
    }
  });
}

// desloguear usuario
export function logout() {
  const drawer = document.getElementById("drawer");
  drawer.innerHTML = "";

  document.body.classList.remove("drawer-open");

  console.log("chau!");

  localStorage.removeItem("user");
  loginView();
}

// cargar datos del usuario
export async function loadUserDetails() {
  const userId = localStorage.getItem("user");
  if (!userId) return;

  try {
    const res = await fetch(`http://localhost:5103/api/User/${userId}`);
    if (!res.ok) throw new Error("Error al cargar datos del usuario");

    const user = await res.json();

    document.getElementById("name").value = user.name || "";
    document.getElementById("email").value = user.email || "";
    document.getElementById("role").value = user.role.name || "";
  } catch (error) {
    console.error(error);
  }
}

// cargar lista de usuarios
export async function loadUserList() {
  try {
    const res = await fetch("http://localhost:5103/api/User");
    if (!res.ok) throw new Error("Error al cargar usuarios");
    const users = await res.json();

    const userListSection = document.getElementById("user-list"); // O el id del contenedor que tengas para la lista
    if (!userListSection) return;

    const selectedUserId = localStorage.getItem("user");

    userListSection.innerHTML = "";

    const ul = document.createElement("ul");
    ul.className = "w-full divide-y divide-stone-400 dark:divide-stone-700";

    users.forEach((user) => {
      const li = document.createElement("li");
      li.className =
        "px-3 py-3 sm:py-4 cursor-pointer flex items-center space-x-4 rtl:space-x-reverse";

      if (selectedUserId === String(user.id)) {
        li.classList.add("bg-teal-600", "text-white", "rounded-md");
      }

      li.innerHTML = `
        <i class="bi bi-person-fill text-4xl mb-1"></i>
        <div class="flex-1 min-w-0">
          <p class="text-md font-medium truncate">${user.name}</p>
          <p class="text-md truncate">${user.email}</p>
        </div>
        <div class="inline-flex items-center text-base font-semibold gap-2">
          ${user.role.name}
        </div>
      `;

      li.addEventListener("click", () => {
        localStorage.setItem("user", user.id);
        loadUserDetails();
        loadUserList();
      });

      ul.appendChild(li);
    });

    userListSection.appendChild(ul);
  } catch (error) {
    console.error(error);
  }
}
