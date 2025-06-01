// cambiar tema
export function toggleTheme() {
  const html = document.documentElement;
  const isDark = html.classList.toggle("dark");
  localStorage.setItem("theme", isDark ? "dark" : "light");
  updateThemeToggleUI(isDark);
}

// chequear tema guardado
export function applySavedTheme() {
  const savedTheme = localStorage.getItem("theme");
  const html = document.documentElement;

  const isDark =
    savedTheme === "dark" ||
    (!savedTheme && window.matchMedia("(prefers-color-scheme: dark)").matches);

  html.classList.toggle("dark", isDark);
  updateThemeToggleUI(isDark);
}

// actualizar icono y texto del drawer
function updateThemeToggleUI(isDark) {
  const icon = document.getElementById("themeIcon");
  const label = document.getElementById("themeLabel");

  if (icon) {
    icon.classList.remove("bi-brightness-low-fill", "bi-moon-fill");
    icon.classList.add(isDark ? "bi-brightness-low-fill" : "bi-moon-fill");
  }

  if (label) {
    label.textContent = isDark ? "Modo Claro" : "Modo Oscuro";
  }
}
