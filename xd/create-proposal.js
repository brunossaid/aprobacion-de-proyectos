export async function setupCreateProposalForm() {
  await loadSelectOptions();
  setupSubmitHandler();
}

// cargar areas y tipos
async function loadSelectOptions() {
  try {
    const [typesRes, areasRes] = await Promise.all([
      fetch("http://localhost:5103/api/ProjectType"),
      fetch("http://localhost:5103/api/Area"),
    ]);

    const [types, areas] = await Promise.all([
      typesRes.json(),
      areasRes.json(),
    ]);

    fillSelect("type", types);
    fillSelect("area", areas);
  } catch (error) {
    console.error("Error al cargar tipos o areas", error);
  }
}

// llenar selects
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

// crear solicitud
function setupSubmitHandler() {
  const form = document.getElementById("create-proposal-form");

  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const data = {
      title: form.name.value,
      description: form.description.value,
      estimatedAmount: parseFloat(form.estimatedAmount.value) || 0,
      estimatedDuration: parseInt(form.estimatedDuration.value) || 0,
      area: parseInt(form.area.value) || 0,
      projectType: parseInt(form.type.value) || 0,
      createBy: localStorage.getItem("user") || "0",
    };

    try {
      const res = await fetch("http://localhost:5103/api/Project", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      const result = await res.json();

      if (!res.ok) {
        const errorMessage = result.message || "Error desconocido del servidor";
        alert("Error: " + errorMessage);
        return;
      }

      alert("Solicitud creada con éxito");
      console.log("solicitud creada");
      displayCreatedProposal(result);
      form.reset();
    } catch (error) {
      console.error("error en la solicitud", error);
      alert("Error al conectar con el servidor");
    }
  });
}

// mostrar solicitud creada
function displayCreatedProposal(proposal) {
  // Mostrar la sección si está oculta
  const createdSection = document.getElementById("created-proposal");

  if (createdSection.classList.contains("hidden")) {
    createdSection.classList.remove("hidden");
    createdSection.classList.add("opacity-0");

    // Forzar reflow para que Tailwind aplique transición
    void createdSection.offsetWidth;

    createdSection.classList.remove("opacity-0");
    createdSection.classList.add("opacity-100");

    // Scroll suave hacia la sección
    createdSection.scrollIntoView({ behavior: "smooth" });
  }

  // Mostrar los datos en los inputs correspondientes
  const fields = {
    name: proposal.title,
    description: proposal.description,
    type: proposal.type.name,
    area: proposal.area.name,
    estimatedAmount: `$ ${proposal.estimatedAmount.toFixed(2)}`,
    estimatedDuration: `${proposal.estimatedDuration} meses`,
    createBy: proposal.createBy.name,
  };

  for (const [key, value] of Object.entries(fields)) {
    const input = document.getElementById(`new-${key}`);
    if (input) {
      input.value = value;
    } else {
      console.warn(`Elemento con id "new-${key}" no encontrado`);
    }
  }
}
