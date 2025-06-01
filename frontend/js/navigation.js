import { setupCreateProposalForm } from "./create-proposal.js";
import { loadUserDetails, loadUserList, logout } from "./user.js";

// cargar paginas
export function loadPage(page) {
  fetch(`pages/${page}.html`)
    .then((res) => res.text())
    .then((html) => {
      document.getElementById("main").innerHTML = html;
      updateNavActiveState(page);

      const logoutButton = document.getElementById("logout-btn");
      if (logoutButton) {
        logoutButton.addEventListener("click", () => {
          logout();
        });
      }
      if (page === "user") {
        loadUserDetails();
        loadUserList();
      }
      if (page === "create-proposal") {
        setupCreateProposalForm();
      }
    })
    .catch((err) => {
      document.getElementById("main").innerHTML =
        "<p class='text-red-500'>Error cargando la página.</p>";
    });
}

// switchear paginas
export function setupNavLinks() {
  document.addEventListener("click", (e) => {
    const target = e.target.closest("[data-page]");
    if (target) {
      e.preventDefault();
      const page = target.getAttribute("data-page");
      loadPage(page);
    }
  });
}

// actualizar estado activo del drawer
function updateNavActiveState(activePage) {
  const navLinks = document.querySelectorAll(".page-link");

  navLinks.forEach((link) => {
    link.classList.remove("bg-stone-300", "dark:bg-stone-800", "font-semibold");
  });

  const activeLink = document.querySelector(
    `[data-page="${activePage}"]:not(.drawer-title)`
  );

  if (activeLink) {
    activeLink.classList.add(
      "bg-stone-300",
      "dark:bg-stone-800",
      "font-semibold"
    );
  }
}
