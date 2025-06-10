import { getStatuses, saveApprovalDecision } from "../api/index.js";
import { translateStatus } from "../utils.js";
import { loadProposalData } from "./load.js";

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
    evaluateBtn.classList.add("hidden");

    await loadApprovalStatuses();
  });

  cancelBtn.addEventListener("click", async () => {
    stepsTable.classList.remove("hidden");
    reviewForm.classList.add("hidden");
    evaluateBtn.classList.remove("hidden");
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
    const statuses = await getStatuses();
    const { status: stepStatus } = JSON.parse(localStorage.getItem("step"));
    const select = document.getElementById("status-select");
    select.innerHTML = "";

    statuses.forEach((status) => {
      if (status.id === 1 && stepStatus.id !== 1) return;

      const option = document.createElement("option");
      option.value = status.id;
      option.textContent = translateStatus(status.name);
      option.selected = status.id === stepStatus.id;

      if (status.id === 1 && stepStatus.id === 1) {
        option.disabled = true;
      }

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
    await saveApprovalDecision(proposalId, reviewData);

    console.log("evaluacion guardada");
    await loadProposalData(proposalId);
    document.getElementById("observation").value = "";
  } catch (error) {
    console.error("error:", error);
    alert(error.message);
  }
}
