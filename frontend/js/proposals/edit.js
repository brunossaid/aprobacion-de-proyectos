import { updateProjectProposal } from "../api/index.js";
import { loadProposalData } from "./load.js";

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
    await updateProjectProposal(id, updatedData);

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

// campos editables y sus valores originales
const editableFields = ["title", "description", "estimatedDuration"];
const originalValues = {};

// modo edicion
let editMode = false;

// clases de los inputs editables
const editableInputClasses = [
  "border-stone-400",
  "dark:border-stone-600",
  "hover:border-stone-600",
  "dark:hover:border-stone-300",
  "focus:border-teal-800",
];
