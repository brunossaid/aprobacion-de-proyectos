const BASE_URL = "http://localhost:5103/api";

export async function getAreas() {
  const res = await fetch(`${BASE_URL}/Area`);
  if (!res.ok) throw new Error("Error al obtener areas");
  return await res.json();
}

export async function getProjectTypes() {
  const res = await fetch(`${BASE_URL}/ProjectType`);
  if (!res.ok) throw new Error("Error al obtener tipos de proyecto");
  return await res.json();
}

export async function getStatuses() {
  const response = await fetch("http://localhost:5103/api/ApprovalStatus");
  if (!response.ok) throw new Error("No se pudieron cargar los estados");
  return response.json();
}
