export async function createProjectProposal(data) {
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
    throw new Error(errorMessage);
  }

  return result;
}

export async function updateProjectProposal(id, updatedData) {
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

  return await response.json();
}

export async function saveApprovalDecision(proposalId, reviewData) {
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
    const errorMessage = errorData.message || "error al guardar la evaluación.";
    throw new Error(errorMessage);
  }

  return await response.json();
}

export async function getFilteredProposals(filters = {}) {
  const { title, status, createBy, approverUserId } = filters;

  const params = new URLSearchParams();
  if (title) params.append("title", title);
  if (status) params.append("status", status);
  if (createBy) params.append("createBy", createBy);
  if (approverUserId) params.append("approverUserId", approverUserId);

  const response = await fetch(`http://localhost:5103/api/Project?${params}`);

  if (!response.ok) {
    const errorBody = await response.json();
    throw new Error(`Error ${response.status}: ${errorBody}`);
  }

  return await response.json();
}

export async function getProjectProposalById(id) {
  const response = await fetch(`http://localhost:5103/api/Project/${id}`);

  if (response.status === 404) {
    return { notFound: true };
  }

  if (!response.ok) {
    const error = await response.json();
    throw new Error(
      error.message || "Error al obtener los datos de la propuesta"
    );
  }

  return await response.json();
}
