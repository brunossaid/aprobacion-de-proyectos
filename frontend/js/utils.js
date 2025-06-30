// formatear fecha a dd/mm/aaaa
export function formatDate(isoString) {
  const date = new Date(isoString);
  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const year = date.getFullYear();
  return `${day}/${month}/${year}`;
}

// colores de fondo por estado
export function getRowBgClass(statusName) {
  const statusColorMap = {
    Pending: "bg-orange-400 dark:bg-orange-500",
    Approved: "bg-green-300 dark:bg-green-700",
    Rejected: "bg-red-400 dark:bg-red-600",
    Observed: "bg-blue-300 dark:bg-sky-600",
  };

  return statusColorMap[statusName] || "";
}

export const bg_classes = [
  "bg-orange-400",
  "dark:bg-orange-500",
  "bg-green-300",
  "dark:bg-green-700",
  "bg-red-400",
  "dark:bg-red-600",
  "bg-blue-300",
  "dark:bg-sky-600",
];

export function translateStatus(statusName) {
  const translations = {
    Pending: "Pendiente",
    Approved: "Aprobado",
    Rejected: "Rechazado",
    Observed: "Observado",
  };

  return translations[statusName] || statusName;
}

// alerta
export function showAlert(message, type = "info") {
  const colors = {
    success: "bg-green-500 text-white",
    error: "bg-red-500 text-white",
    info: "bg-blue-600 text-white",
    warning: "bg-orange-500 text-white",
  };

  const icons = {
    success: "bi-check-circle",
    error: "bi-x-circle",
    info: "bi-info-circle",
    warning: "bi-exclamation-triangle",
  };

  let container = document.getElementById("alert-container");
  if (!container) {
    container = document.createElement("div");
    container.id = "alert-container";
    container.className =
      "fixed bottom-16 left-1/2 transform -translate-x-1/2 z-50 flex flex-col gap-2 items-center";
    document.body.appendChild(container);
  }

  const alert = document.createElement("div");
  alert.className = `rounded-xl px-6 py-2 shadow-md flex items-center gap-2 animate-fade-in-out transition-opacity duration-500 ${
    colors[type] || colors.info
  }`;

  alert.innerHTML = `
    <div class="flex items-center gap-3">
      <i class="bi ${icons[type] || icons.info} text-3xl"></i>
      <span class="text-xl leading-tight">${message}</span>
    </div>
  `;

  container.appendChild(alert);

  setTimeout(() => {
    alert.classList.add("opacity-0");
    setTimeout(() => alert.remove(), 500);
  }, 3000);
}
