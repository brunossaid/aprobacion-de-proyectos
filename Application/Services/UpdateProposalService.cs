using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class UpdateProposalService
    {
        private readonly IProjectProposalService _proposalService;

        public UpdateProposalService(IProjectProposalService proposalService)
        {
            _proposalService = proposalService;
        }

        public async Task<ProjectProposal> UpdateProposalAsync(Guid projectId, UpdateProjectProposalDto updateDto)
        {
            var project = await _proposalService.GetByIdAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException("Proyecto no encontrado");

            if (project.Status != 4)
                throw new InvalidOperationException("Solo se puede modificar un proyecto observado.");

            if (!string.IsNullOrWhiteSpace(updateDto.Title))
            {
                var newTitle = updateDto.Title.Trim();
                bool titleExists = await _proposalService.TitleExistsAsync(newTitle, projectId);
                if (titleExists)
                    throw new InvalidOperationException("Ya existe un proyecto con ese título.");
                project.Title = newTitle;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Description))
                project.Description = updateDto.Description.Trim();

            if (updateDto.EstimatedDuration.HasValue)
            {
                if (updateDto.EstimatedDuration.Value <= 0)
                    throw new InvalidOperationException("La duracion estimada debe ser mayor a cero.");

                project.EstimatedDuration = updateDto.EstimatedDuration.Value;
            }

            await _proposalService.UpdateAsync(project);
            return project;
        }
    }
}
