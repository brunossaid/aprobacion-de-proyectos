import { getRowBgClass, translateStatus } from "../utils.js";
import { getFilteredProposals, getStatuses } from "../api/index.js";

export async function setupProposalsForm(filterMode = "creator") {
  await setupFiltersAndInitialTable(filterMode);
  await filterProposals(filterMode);
}

async function setupFiltersAndInitialTable(filterMode) {
  await loadStatusOptions();
  await filterProposals(filterMode);

  const form = document.getElementById("project-filter");
  form.addEventListener("submit", (event) => {
    event.preventDefault();
    filterProposals(filterMode);
  });
}

// cargar estados en el select
async function loadStatusOptions() {
  try {
    const statuses = await getStatuses();
    const select = document.getElementById("status");

    select.innerHTML = `<option value="">Cualquiera</option>`;
    statuses.forEach((status) => {
      const option = document.createElement("option");
      option.value = status.id;
      option.textContent = translateStatus(status.name);
      select.appendChild(option);
    });

    const storedStatus = localStorage.getItem("statusFilterFromHome");
    if (storedStatus) {
      select.value = storedStatus;
    }
  } catch (error) {
    console.error("Error cargando estados:", error);
  }
}

// filtrar propuestas
async function filterProposals(filterMode = "creator") {
  const title = document.getElementById("title").value.trim();
  let status = document.getElementById("status").value;

  const storedStatus = localStorage.getItem("statusFilterFromHome");
  if (storedStatus) {
    status = storedStatus;
    const select = document.getElementById("status");
    if (select) {
      select.value = storedStatus;
    }
    localStorage.removeItem("statusFilterFromHome");
  }

  const user = JSON.parse(localStorage.getItem("user"));
  const filters = { title, status };

  if (filterMode === "creator") {
    filters.createBy = user.id;
  } else if (filterMode === "approver") {
    filters.approverUserId = user.id;
  }

  try {
    const proposals = await getFilteredProposals(filters);
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
    )} cursor-pointer hover:opacity-85 transition-opacity`;

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
