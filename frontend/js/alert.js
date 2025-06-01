export function showAlert(type, message) {
  const container = document.getElementById("alert-container");
  container.innerHTML = "";

  const config = {
    success: {
      bg: "bg-green-100",
      border: "border-green-400",
      text: "text-green-700",
      iconPath: `<path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414L8.414 15 5.293 11.879a1 1 0 00-1.414 1.414l4 4a1 1 0 001.414 0l9-9a1 1 0 00-1.414-1.414z" clip-rule="evenodd"/>`,
    },
    error: {
      bg: "bg-red-100",
      border: "border-red-400",
      text: "text-red-700",
      iconPath: `<path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm-1-4h2v2h-2v-2zm0-8h2v6h-2V6z" clip-rule="evenodd"/>`,
    },
  };

  const style = config[type] || config.success;

  const alertDiv = document.createElement("div");
  alertDiv.className = `flex items-center ${style.bg} ${style.border} border px-4 py-3 rounded shadow-md max-w-sm`;
  alertDiv.setAttribute("role", "alert");

  const icon = document.createElementNS("http://www.w3.org/2000/svg", "svg");
  icon.setAttribute("class", `fill-current w-6 h-6 mr-2 ${style.text}`);
  icon.setAttribute("viewBox", "0 0 20 20");
  icon.innerHTML = style.iconPath;

  const text = document.createElement("span");
  text.className = style.text;
  text.textContent = message;

  alertDiv.appendChild(icon);
  alertDiv.appendChild(text);

  container.appendChild(alertDiv);

  setTimeout(() => {
    alertDiv.remove();
  }, 4000);
}
