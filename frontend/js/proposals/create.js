import {
  createProjectProposal,
  getAreas,
  getProjectTypes,
} from "../api/index.js";
import { loadPage } from "../navigation.js";
import { showAlert } from "../utils.js";

export async function setupCreateProposalForm() {
  await loadSelectOptions();
  setupSubmitHandler();
}

async function loadSelectOptions() {
  try {
    const [types, areas] = await Promise.all([getProjectTypes(), getAreas()]);

    fillSelect("type", types);
    fillSelect("area", areas);
  } catch (error) {
    console.error("Error al cargar tipos o areas", error);
  }
}

function fillSelect(id, options) {
  const select = document.getElementById(id);
  select.innerHTML = "";

  options.forEach((option) => {
    const opt = document.createElement("option");
    opt.value = option.id;
    opt.textContent = option.name;
    select.appendChild(opt);
  });
}

// configura el submit
function setupSubmitHandler() {
  const form = document.getElementById("create-proposal-form");
  form.removeEventListener("submit", handleSubmit);
  form.addEventListener("submit", handleSubmit);
}

// crear solicitud
async function handleSubmit(e) {
  e.preventDefault();

  const form = e.target;
  const user = JSON.parse(localStorage.getItem("user"));

  const data = {
    title: form.title.value,
    description: form.description.value,
    amount: parseFloat(form.estimatedAmount.value),
    duration: parseInt(form.estimatedDuration.value),
    area: parseInt(form.area.value),
    type: parseInt(form.type.value),
    user: user.id,
  };

  try {
    const result = await createProjectProposal(data);

    showAlert("Solicitud de proyecto creada con exito", "success");
    console.log("solicitud creada");
    displayCreatedProposal(result);
    form.reset();
  } catch (error) {
    console.error("error en la solicitud", error);
    showAlert(error, "error");
  }
}

function displayCreatedProposal(proposal) {
  const createdSection = document.getElementById("created-proposal");

  if (createdSection.classList.contains("hidden")) {
    createdSection.classList.remove("hidden");
    createdSection.classList.add("opacity-0");

    void createdSection.offsetWidth;

    createdSection.classList.remove("opacity-0");
    createdSection.classList.add("opacity-100");

    createdSection.scrollIntoView({ behavior: "smooth" });
  }

  const fields = {
    title: proposal.title,
    description: proposal.description,
    type: proposal.type.name,
    area: proposal.area.name,
    estimatedAmount: `$ ${proposal.amount.toFixed(2)}`,
    estimatedDuration: `${proposal.duration} meses`,
    createBy: proposal.user.name,
  };

  const viewBtn = document.getElementById("view-proposal-btn");
  if (viewBtn) {
    viewBtn.onclick = () => {
      loadPage("proposal", proposal.id);
    };
  }

  for (const [key, value] of Object.entries(fields)) {
    const input = document.getElementById(`new-${key}`);
    if (input) {
      input.value = value;
    } else {
      console.warn(`Elemento con id "new-${key}" no encontrado`);
    }
  }
}
