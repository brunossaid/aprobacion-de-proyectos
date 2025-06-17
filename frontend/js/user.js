import { initializeApp } from "./main.js";
import { applySavedTheme } from "./theme.js";
import { getUsers } from "./api/index.js";

export function loginView() {
  fetch("pages/login.html")
    .then((res) => res.text())
    .then((html) => {
      document.getElementById("main").innerHTML = html;
      applySavedTheme();
      setupAuthForm();
      loadUsers();
    });
}
let loadedUsers = [];

async function loadUsers() {
  try {
    const users = await getUsers();
    loadedUsers = users;

    const select = document.getElementById("user-select");
    if (!select) return;

    const defaultOption = document.createElement("option");
    defaultOption.disabled = true;
    defaultOption.selected = true;
    defaultOption.hidden = true;
    defaultOption.textContent = "Selecciona un usuario...";
    select.appendChild(defaultOption);

    users.forEach((user) => {
      const option = document.createElement("option");
      option.value = user.id;
      option.textContent = user.name;
      select.appendChild(option);
    });

    select.addEventListener("change", () => {
      const firstOption = select.querySelector("option[disabled]");
      if (firstOption) {
        firstOption.remove();
      }
    });
  } catch (error) {
    console.error(error);
  }
}

function setupAuthForm() {
  const form = document.getElementById("auth-form");
  form?.addEventListener("submit", (e) => {
    e.preventDefault();
    const selectedId = document.getElementById("user-select")?.value;
    const selectedUser = loadedUsers.find((u) => u.id == selectedId);
    if (selectedUser) {
      localStorage.setItem("user", JSON.stringify(selectedUser));
      initializeApp();
    }
  });
}

export function logout() {
  const drawer = document.getElementById("drawer");
  drawer.innerHTML = "";

  document.body.classList.remove("drawer-open");

  localStorage.removeItem("user");
  localStorage.removeItem("lastPage");
  loginView();
}

export async function loadUserDetails() {
  const userStr = localStorage.getItem("user");
  if (!userStr) return;

  const user = JSON.parse(userStr);

  document.getElementById("name").value = user.name || "";
  document.getElementById("email").value = user.email || "";
  document.getElementById("role").value = user.role?.name || "";
}

export async function loadUserList() {
  try {
    const users = await getUsers();

    const userListSection = document.getElementById("user-list");
    if (!userListSection) return;

    const selectedUser = JSON.parse(localStorage.getItem("user"));

    userListSection.innerHTML = "";

    const ul = document.createElement("ul");
    ul.className = "w-full divide-y divide-stone-400 dark:divide-stone-700";

    users.forEach((user) => {
      const li = document.createElement("li");
      li.className =
        "px-3 py-3 sm:py-4 cursor-pointer hover:opacity-70 transition-opacity flex items-center space-x-4 rtl:space-x-reverse";

      if (selectedUser && String(selectedUser.id) === String(user.id)) {
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
        localStorage.setItem("user", JSON.stringify(user));
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
