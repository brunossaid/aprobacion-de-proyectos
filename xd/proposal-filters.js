import { getRowBgClass, translateStatus } from "./utils.js";

export async function setupProposalsForm() {
  await setupFiltersAndInitialTable();
  await filterProposals();
}

async function setupFiltersAndInitialTable() {
  await loadStatusOptions();
  await filterProposals();

  const form = document.getElementById("project-filter");
  form.addEventListener("submit", (event) => {
    event.preventDefault();
    filterProposals();
  });
}

// cargar estados en el select
async function loadStatusOptions() {
  try {
    const response = await fetch("http://localhost:5103/api/ApprovalStatus");
    if (!response.ok) throw new Error("Error al obtener los estados");

    const statuses = await response.json();
    const select = document.getElementById("status");

    select.innerHTML = `<option value="">Cualquiera</option>`;
    statuses.forEach((status) => {
      const option = document.createElement("option");
      option.value = status.id;
      option.textContent = translateStatus(status.name);
      select.appendChild(option);
    });
  } catch (error) {
    console.error("Error cargando estados:", error);
  }
}

// filtrar propuestas
async function filterProposals() {
  const title = document.getElementById("title").value.trim();
  const status = document.getElementById("status").value;
  const user = JSON.parse(localStorage.getItem("user"));
  const createBy = user.id;

  const params = new URLSearchParams();
  if (title) params.append("title", title);
  if (status) params.append("status", status);
  if (createBy) params.append("createBy", createBy);

  try {
    const response = await fetch(`http://localhost:5103/api/Project?${params}`);

    if (!response.ok) {
      const errorBody = await response.json();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    const proposals = await response.json();
    renderProposalTable(proposals);
  } catch (error) {
    console.error("error:", error.message);
  }
}

// cargar tabla
function renderProposalTable(proposals) {
  const tbody = document.querySelector("table tbody");
  tbody.innerHTML = "";

  proposals.forEach((proposal) => {
    const row = document.createElement("tr");
    row.className = `${getRowBgClass(
      proposal.status
    )} cursor-pointer hover:bg-neutral-200 transition-colors`;

    // le agregamos los atributos que ya usabas en el <a>
    row.setAttribute("data-page", "proposal");
    row.setAttribute("data-id", proposal.id);
    row.setAttribute("title", "Ver Proyecto");

    row.innerHTML = `
      <td class="px-6 py-4 text-left whitespace-nowrap">${proposal.title}</td>
      <td class="px-6 py-4 text-left hidden xl:table-cell">${
        proposal.description
      }</td>
      <td class="px-6 py-4 hidden lg:table-cell">$${proposal.amount}</td>
      <td class="px-6 py-4 hidden lg:table-cell">${proposal.duration} meses</td>
      <td class="px-6 py-4">${proposal.area}</td>
      <td class="px-6 py-4">${proposal.type}</td>
      <td class="px-6 py-4">${translateStatus(proposal.status)}</i>
      </td>
    `;

    tbody.appendChild(row);
  });
}
