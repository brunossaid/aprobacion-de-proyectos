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
  const res = await fetch(`${BASE_URL}/ApprovalStatus`);
  if (!res.ok) throw new Error("Error al cargar estados");
  return res.json();
}

export async function getUsers() {
  const res = await fetch(`${BASE_URL}/User`);
  if (!res.ok) throw new Error("Error al cargar usuarios");
  return await res.json();
}
