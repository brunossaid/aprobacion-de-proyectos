import { formatDate, getRowBgClass, translateStatus } from "./utils.js";

// cargar datos del proyecto
export async function loadProposalData(id) {
  try {
    const response = await fetch(`http://localhost:5103/api/Project/${id}`);

    if (response.status === 404) {
      document.getElementById("main").innerHTML = `
        <div class="p-4">
          <h2 class="text-xl font-bold mb-2">Propuesta no encontrada</h2>
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

// rellenar campos
function fillProposalForm(proposal) {
  document.getElementById("name").value = proposal.title;
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
}

// campos editables y sus valores originales
const editableFields = ["name", "description", "estimatedDuration"];
const originalValues = {};
let editMode = false;

// cargar eventos de edicion
export function setupEditHandlers() {
  const form = document.getElementById("edit-proposal-form");
  const editBtn = document.getElementById("edit-btn");
  const saveBtn = document.getElementById("save-btn");
  const cancelBtn = document.getElementById("cancel-btn");
  const editActions = editBtn.nextElementSibling;

  editBtn.addEventListener("click", () => enableEditMode(editActions));
  cancelBtn.addEventListener("click", () => cancelEdit(editActions));
  saveBtn.addEventListener("click", () => form.requestSubmit());

  form.addEventListener("submit", handleFormSubmit);
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
    el.classList.remove("border-transparent");
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
    el.classList.add("border-transparent");
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
async function handleFormSubmit(e) {
  e.preventDefault();

  const titleEl = document.getElementById("name");
  const descriptionEl = document.getElementById("description");
  const durationEl = document.getElementById("estimatedDuration");

  const updatedData = {
    title: titleEl.value.trim() || originalValues["name"],
    description: descriptionEl.value.trim() || originalValues["description"],
    duration: parseInt(durationEl.value, 10),
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
      field.classList.add("border-transparent");
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
