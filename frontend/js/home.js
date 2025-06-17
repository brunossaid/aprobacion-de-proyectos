import { getFilteredProposals } from "./api/index.js";
import { getRowBgClass, translateStatus } from "./utils.js";

export async function setupHome() {
  const user = JSON.parse(localStorage.getItem("user"));

  document.getElementById("user-name").textContent = `Bienvenido, ${user.name}`;
  document.getElementById("user-role").textContent = user.role.name;
  document.getElementById("date").textContent = new Date().toLocaleDateString();

  try {
    const ownProposals = await getFilteredProposals({ createBy: user.id });

    const summary = {
      pending: 0,
      approved: 0,
      rejected: 0,
      observed: 0,
    };

    ownProposals.forEach((p) => {
      const status = p.status.toLowerCase();
      if (status && summary.hasOwnProperty(status)) {
        summary[status]++;
      }
    });

    const proposalsToReview = await getFilteredProposals({
      approverUserId: user.id,
    });

    updateProposalSummary("pending-proposals", summary.pending, "Pending");
    updateProposalSummary("approved-proposals", summary.approved, "Approved");
    updateProposalSummary("rejected-proposals", summary.rejected, "Rejected");
    updateProposalSummary("observed-proposals", summary.observed, "Observed");

    document.getElementById(
      "proposals-to-review"
    ).textContent = `${proposalsToReview.length} a Revisar`;
  } catch (err) {
    console.error("Error cargando resumen:", err);
  }
}

// setear los divs con el estado y el color
function updateProposalSummary(id, count, status) {
  const statusText =
    count === 1
      ? `${count} ${translateStatus(status)}`
      : `${count} ${translateStatus(status)}s`;
  document.getElementById(id).textContent = statusText;
  const div = document.querySelector(`#${id}`).parentElement;
  const bgClass = getRowBgClass(status);
  div.classList.add(...bgClass.split(" "));
  if (count === 0) {
    div.style.display = "none";
  }
}
