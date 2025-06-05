import { formatDate, getRowBgClass, translateStatus } from "../utils.js";

// cargar datos del proyecto
export async function loadProposalData(id) {
  try {
    const response = await fetch(`http://localhost:5103/api/Project/${id}`);

    if (response.status === 404) {
      document.getElementById("main").innerHTML = `
        <div class="p-4">
          <h2 class="text-2xl font-bold mb-2">Propuesta no encontrada</h2>
          <p>La solicitud con ID <strong>${id}</strong> no existe.</p>
        </div>
      `;
      return;
    }

    if (!response.ok) {
      throw new Error("Error al obtener los datos de la propuesta");
    }

    const proposal = await response.json();
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

// campos editables y sus valores originales
const editableFields = ["title", "description", "estimatedDuration"];
const originalValues = {};
let editMode = false;

// botones de edicion de proyecto
export function setupEditHandlers() {
  const form = document.getElementById("edit-proposal-form");
  const editBtn = document.getElementById("edit-btn");
  const saveBtn = document.getElementById("save-btn");
  const cancelBtn = document.getElementById("cancel-btn");
  const editActions = editBtn.nextElementSibling;

  editBtn.addEventListener("click", () => enableEditMode(editActions));
  cancelBtn.addEventListener("click", () => cancelEdit(editActions));
  saveBtn.addEventListener("click", () => form.requestSubmit());

  form.addEventListener("submit", saveEditedProposal);
}

// activar modo edicion
function enableEditMode(editActions) {
  editMode = true;
  toggleEditButtons(true, editActions);

  editableFields.forEach((id) => {
    const el = document.getElementById(id);
    originalValues[id] = el.value;
    if (id === "estimatedAmount") {
      el.value = el.value.replace(/[^0-9.]/g, "");
    }
    el.removeAttribute("readonly");
    el.classList.remove("border-transparent", "focus:outline-none");
    el.classList.add(...editableInputClasses);
  });
}

// cancelar edicion
function cancelEdit(editActions) {
  editMode = false;
  toggleEditButtons(false, editActions);

  editableFields.forEach((id) => {
    const el = document.getElementById(id);
    el.value = originalValues[id];
    el.setAttribute("readonly", true);
    el.classList.remove(...editableInputClasses);
    el.classList.add("border-transparent", "focus:outline-none");
  });
}

// mostrar/ocultar botones
function toggleEditButtons(editing, editActions) {
  document.getElementById("edit-btn").style.display = editing
    ? "none"
    : "inline-block";
  editActions.classList.toggle("hidden", !editing);
}

// guardar
async function saveEditedProposal(e) {
  e.preventDefault();

  const title = document.getElementById("title");
  const description = document.getElementById("description");
  const duration = document.getElementById("estimatedDuration");

  const updatedData = {
    title: title.value.trim() || originalValues["title"],
    description: description.value.trim() || originalValues["description"],
    duration: parseInt(duration.value, 10),
  };

  const state = JSON.parse(localStorage.getItem("lastPage"));
  const id = state?.id;

  try {
    const response = await fetch(`http://localhost:5103/api/Project/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(updatedData),
    });

    if (!response.ok) {
      const errorData = await response.json();
      const errorMessage = errorData.message || "Error.";
      throw new Error(errorMessage);
    }

    console.log("solicitud actualizada");

    editableFields.forEach((fieldId) => {
      const field = document.getElementById(fieldId);
      field.setAttribute("readonly", true);
      field.classList.remove(...editableInputClasses);
      field.classList.add("border-transparent", "focus:outline-none");
    });

    toggleEditButtons(
      false,
      document.getElementById("edit-btn").nextElementSibling
    );
    editMode = false;

    await loadProposalData(id);
  } catch (err) {
    console.error("error:", err);
    alert(err.message);
  }
}

// clases de los inputs editables
const editableInputClasses = [
  "border-stone-400",
  "dark:border-stone-600",
  "hover:border-stone-600",
  "dark:hover:border-stone-300",
  "focus:border-teal-800",
];

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

// botones de evaluacion de proyecto
export function setupEvaluateHandler() {
  const evaluateBtn = document.getElementById("evaluate-proposal-btn");
  const saveBtn = document.getElementById("save-review-btn");
  const cancelBtn = document.getElementById("cancel-review-btn");
  const stepsTable = document.getElementById("steps-table");
  const reviewForm = document.getElementById("review-project-form");

  evaluateBtn.addEventListener("click", async () => {
    stepsTable.classList.add("hidden");
    reviewForm.classList.remove("hidden");

    await loadApprovalStatuses();
  });

  cancelBtn.addEventListener("click", async () => {
    stepsTable.classList.remove("hidden");
    reviewForm.classList.add("hidden");
  });

  saveBtn.addEventListener("click", async () => {
    stepsTable.classList.remove("hidden");
    reviewForm.classList.add("hidden");

    saveApprovalStep();
  });
}

// select de status
async function loadApprovalStatuses() {
  try {
    const response = await fetch("http://localhost:5103/api/ApprovalStatus");
    if (!response.ok) throw new Error("No se pudieron cargar los estados");

    const statuses = await response.json();

    const select = document.getElementById("status-select");
    select.innerHTML = "";

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

// guardar evaluacion de step
async function saveApprovalStep() {
  const state = JSON.parse(localStorage.getItem("lastPage"));
  const user = JSON.parse(localStorage.getItem("user"));
  const step = JSON.parse(localStorage.getItem("step"));
  const proposalId = state?.id;

  const statusId = parseInt(document.getElementById("status-select").value, 10);
  const observation = document.getElementById("observation").value.trim();

  const reviewData = {
    id: step.id,
    user: user.id,
    status: statusId,
    observation: observation,
  };

  try {
    const response = await fetch(
      `http://localhost:5103/api/Project/${proposalId}/decision`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(reviewData),
      }
    );

    if (!response.ok) {
      const errorData = await response.json();
      const errorMessage =
        errorData.message || "error al guardar la evaluacion.";
      throw new Error(errorMessage);
    }

    console.log("evaluacion guardada");
    await loadProposalData(proposalId);
  } catch (error) {
    console.error("error:", error);
    alert(error.message);
  }
}

// guardar stepId a evaluar en localstorage
function storeNextApprovalStepId(steps) {
  const evaluateBtn = document.getElementById("evaluate-proposal-btn");

  const nextStep = steps.find(
    (step) => step.status.id === 1 || step.status.id === 4
  );
  if (nextStep) {
    localStorage.setItem("step", JSON.stringify(nextStep));
    evaluateBtn.classList.remove("hidden");
  } else {
    localStorage.removeItem("step");
    evaluateBtn.classList.add("hidden");
  }
}
