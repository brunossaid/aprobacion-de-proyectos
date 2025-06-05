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

// traduccion de estados
export function translateStatus(statusName) {
  const translations = {
    Pending: "Pendiente",
    Approved: "Aprobado",
    Rejected: "Rechazado",
    Observed: "Observado",
  };

  return translations[statusName] || statusName;
}
