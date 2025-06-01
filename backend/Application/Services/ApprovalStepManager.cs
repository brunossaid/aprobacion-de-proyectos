using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class ApprovalStepManager
{
    private readonly IProjectProposalReader _proposalReader;
    private readonly IProjectProposalWriter _proposalWriter;
    private readonly IProjectApprovalStepReader _stepReader;
    private readonly IProjectApprovalStepWriter _stepWriter;
    private readonly IUserService _userService;

    public ApprovalStepManager(
        IProjectProposalReader proposalReader,
        IProjectProposalWriter proposalWriter,
        IProjectApprovalStepReader stepReader,
        IProjectApprovalStepWriter stepWriter,
        IUserService userService)
    {
        _proposalReader = proposalReader;
        _proposalWriter = proposalWriter;
        _stepReader = stepReader;
        _stepWriter = stepWriter;
        _userService = userService;
    }

    public async Task<ProjectProposal> DecideNextStepAsync(Guid projectId, DecisionDto dto)
    {
        var project = await _proposalReader.GetByIdAsync(projectId);
        if (project == null)
            throw new KeyNotFoundException("Proyecto no encontrado");

        if (project.Status == 2)
            throw new InvalidOperationException("El proyecto ya fue aprobado");
        if (project.Status == 3)
            throw new InvalidOperationException("El proyecto ya fue rechazado");

        var step = await _stepReader.GetByIdAsync(dto.Id);
        if (step == null)
            throw new KeyNotFoundException("Paso de aprobacion no encontrado");

        if (step.ProjectProposalId != projectId)
            throw new InvalidOperationException("El paso no pertenece al proyecto indicado");

        if (step.Status != 1 && step.Status != 4)
            throw new InvalidOperationException("Este paso ya fue evaluado");

        var steps = await _stepReader.GetStepsByProjectIdAsync(projectId);
        var priorStepsIncomplete = steps.Any(s =>
            s.StepOrder < step.StepOrder &&
            s.Status != 2);
        if (priorStepsIncomplete)
            throw new InvalidOperationException("No se puede evaluar este paso hasta que se aprueben todos los pasos anteriores.");

        var user = await _userService.GetByIdAsync(dto.User);
        if (user == null)
            throw new KeyNotFoundException("Usuario no encontrado");

        if (user.Role != step.ApproverRoleId)
            throw new InvalidOperationException("El usuario no tiene permisos para aprobar este paso");

        step.Status = dto.Status;
        step.Observations = dto.Observation;
        step.DecisionDate = DateTime.UtcNow;
        step.ApproverUserId = dto.User;
        await _stepWriter.UpdateAsync(step);

        // modificar estado del proyecto si corresponde
        switch (dto.Status)
        {
            case 1:
                throw new InvalidOperationException("No se puede asignar estado pendiente.");
            case 2:
                var updatedSteps = await _stepReader.GetStepsByProjectIdAsync(projectId);
                if (updatedSteps.All(s => s.Status == 2))
                {
                    project.Status = 2;
                }
                else
                {
                    project.Status = 1;
                }
                await _proposalWriter.UpdateAsync(project);
                break;
            case 3:
                project.Status = 3;
                await _proposalWriter.UpdateAsync(project);
                break;
            case 4:
                project.Status = 4;
                await _proposalWriter.UpdateAsync(project);
                break;
            default:
                throw new InvalidOperationException("Estado invalido.");
        }

        return project;
    }

    public async Task<List<ProjectApprovalStep>> GetPendingStepsForUserAsync(int userId)
    {
        var user = await _userService.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("Usuario no encontrado");

        var allSteps = await _stepReader.GetAllAsync();

        var pendingSteps = allSteps
            .Where(step =>
                step.ApproverRoleId == user.Role &&
                (step.Status == 1 || step.Status == 4) &&
                allSteps
                    .Where(prev =>
                        prev.ProjectProposalId == step.ProjectProposalId &&
                        prev.StepOrder < step.StepOrder)
                    .All(prev => prev.Status == 2))
            .ToList();

        return pendingSteps;
    }

    public async Task<List<ProjectApprovalStep>> GetStepsForProposalAsync(Guid proposalId)
    {
        var steps = await _stepReader.GetStepsByProjectIdAsync(proposalId);
        return steps.OrderBy(s => s.StepOrder).ToList();
    }
}
