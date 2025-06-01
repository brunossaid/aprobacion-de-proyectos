import { setupNavLinks, loadPage } from "./navigation.js";
import { applySavedTheme, toggleTheme } from "./theme.js";
import { loginView } from "./user.js";

let user = false; // false loguearme, true ya estoy logueado

// cargar la app
document.addEventListener("DOMContentLoaded", () => {
  // const user = localStorage.getItem("user");
  if (!user) {
    loginView();
  } else {
    initializeApp();
  }
});

// cargar drawer y resto de app
export function initializeApp() {
  fetch("components/drawer.html")
    .then((response) => response.text())
    .then((data) => {
      const drawer = document.getElementById("drawer");
      drawer.innerHTML = data;

      document.body.classList.add("drawer-open");

      setupNavLinks();

      const toggleButton = document.getElementById("themeToggle");
      if (toggleButton) {
        toggleButton.addEventListener("click", toggleTheme);
      }

      applySavedTheme();
      loadPage("create-proposal");
    });
}
