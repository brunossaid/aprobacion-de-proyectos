import { getProjectProposalById } from "../api/index.js";
import { formatDate, getRowBgClass, translateStatus } from "../utils.js";

// cargar datos del proyecto
export async function loadProposalData(id) {
  try {
    const proposal = await getProjectProposalById(id);
    fillProposalForm(proposal);
  } catch (error) {
    console.error("Error al cargar la propuesta:", error);
    document.getElementById("main").innerHTML = `
      <div class="p-4">
        <h2 class="text-xl font-bold mb-2">Error al cargar la propuesta</h2>
        <p>Ocurrio un error inesperado. Por favor, intenta más tarde.</p>
      </div>
    `;
  }
}

// rellenar camposdel proposal
function fillProposalForm(proposal) {
  document.getElementById("title").value = proposal.title;
  document.getElementById("description").value = proposal.description;
  document.getElementById("type").value = proposal.type.name;
  document.getElementById("area").value = proposal.area.name;
  document.getElementById(
    "estimatedAmount"
  ).value = `$ ${proposal.estimatedAmount}`;
  document.getElementById("estimatedDuration").value =
    proposal.estimatedDuration;
  document.getElementById("createBy").value = proposal.createBy.name;
  document.getElementById("createAt").value = formatDate(proposal.createAt);

  const statusInput = document.getElementById("status");
  statusInput.value = translateStatus(proposal.status.name);
  statusInput.classList.add(...getRowBgClass(proposal.status.name).split(" "));

  // solo es editable cuando el status es observed
  const editBtn = document.getElementById("edit-btn");
  const editActions = editBtn.nextElementSibling;

  if (proposal.status.id === 4) {
    editBtn.classList.remove("hidden");
  } else {
    editBtn.classList.add("hidden");
    editActions.classList.add("hidden");
  }

  storeNextApprovalStepId(proposal.approvalSteps);
  fillApprovalSteps(proposal.approvalSteps);
}

// rellenar tabla de steps
function fillApprovalSteps(steps) {
  const tbody = document.querySelector("table tbody");
  tbody.innerHTML = "";

  steps.forEach((step) => {
    const row = document.createElement("tr");
    row.className = getRowBgClass(step.status.name);

    row.innerHTML = `
        <td class="px-6 py-4 text-left">Paso ${step.stepOrder}</td>
        <td class="px-6 py-4">${step.approverRole?.name || "-"}</td>
        <td class="px-6 py-4 hidden lg:table-cell">${
          step.approverUser?.name || "-"
        }</td>
        <td class="px-6 py-4 hidden lg:table-cell">${
          step.decisionDate ? formatDate(step.decisionDate) : "-"
        }</td>
        <td class="px-6 py-4 text-left hidden xl:table-cell">${
          step.observations || "-"
        }</td>
        <td class="px-6 py-4">${translateStatus(step.status?.name || "")}</td>
      `;

    tbody.appendChild(row);
  });
}

// guardar stepId a evaluar en localstorage
function storeNextApprovalStepId(steps) {
  const user = JSON.parse(localStorage.getItem("user"));
  const evaluateBtn = document.getElementById("evaluate-proposal-btn");

  const nextStep = steps.find(
    (step) =>
      (step.status.id === 1 || step.status.id === 4) &&
      step.approverRole.id === user.role.id
  );

  if (nextStep) {
    localStorage.setItem("step", JSON.stringify(nextStep));
    evaluateBtn.classList.remove("hidden");
  } else {
    localStorage.removeItem("step");
    evaluateBtn.classList.add("hidden");
  }
}
