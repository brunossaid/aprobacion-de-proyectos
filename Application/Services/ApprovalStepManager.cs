using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class ApprovalStepManager
{
    private readonly IProjectProposalService _proposalService;
    private readonly IProjectApprovalStepService _stepService;
    private readonly IUserService _userService;

    public ApprovalStepManager(
        IProjectProposalService proposalService,
        IProjectApprovalStepService stepService,
        IUserService userService)
    {
        _proposalService = proposalService;
        _stepService = stepService;
        _userService = userService;
    }

    public async Task<ProjectProposal> DecideNextStepAsync(Guid projectId, DecisionDto dto)
    {
        var project = await _proposalService.GetByIdAsync(projectId);
        if (project == null)
            throw new KeyNotFoundException("Proyecto no encontrado");

        if (project.Status == 3)
            throw new InvalidOperationException("El proyecto ya fue aprobado");
        if (project.Status == 2)
            throw new InvalidOperationException("El proyecto ya fue rechazado");

        var step = await _stepService.GetByIdAsync(dto.Id);
        if (step == null)
            throw new KeyNotFoundException("Paso de aprobacion no encontrado");

        if (step.ProjectProposalId != projectId)
            throw new InvalidOperationException("El paso no pertenece al proyecto indicado");

        if (step.Status != 1 && step.Status != 4)
            throw new InvalidOperationException("Este paso ya fue evaluado");

        var user = await _userService.GetByIdAsync(dto.User);
        if (user == null)
            throw new KeyNotFoundException("Usuario no encontrado");

        if (user.Role != step.StepOrder)
            throw new InvalidOperationException("El usuario no tiene permisos para aprobar este paso");

        step.Status = dto.Status;
        step.Observations = dto.Observation;
        step.DecisionDate = DateTime.UtcNow;
        step.ApproverUserId = dto.User;
        await _stepService.UpdateAsync(step);

        // modificar status del proyecto si corresponde
        switch (dto.Status)
        {
            case 1:
                throw new InvalidOperationException("No se puede asignar estado pendiente.");
            case 2:
                var steps = await _stepService.GetStepsByProjectIdAsync(projectId);
                if (steps.All(s => s.Status == 2))
                {
                    project.Status = 2;
                    await _proposalService.UpdateAsync(project);
                }
                project.Status = 1;
                await _proposalService.UpdateAsync(project);
                break;
            case 3:
                project.Status = 3;
                await _proposalService.UpdateAsync(project);
                break;
            case 4:
                project.Status = 4;
                await _proposalService.UpdateAsync(project);
                break;
            default:
                throw new InvalidOperationException("Estado invalido.");
        }        

        return project;
    }
}
