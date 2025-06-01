using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class UpdateProposalService
    {
        private readonly IProjectProposalWriter _proposalWriter;
        private readonly IProjectProposalReader _proposalReader;

        public UpdateProposalService(IProjectProposalReader proposalReader, IProjectProposalWriter proposalWriter)
        {
            _proposalWriter = proposalWriter;
            _proposalReader = proposalReader;
        }

        public async Task<ProjectProposal> UpdateProposalAsync(Guid projectId, UpdateProjectProposalDto updateDto)
        {
            var project = await _proposalReader.GetByIdAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException("Proyecto no encontrado");

            if (project.Status != 4)
                throw new InvalidOperationException("Solo se puede modificar un proyecto observado.");

            if (!string.IsNullOrWhiteSpace(updateDto.Title))
            {
                var newTitle = updateDto.Title.Trim();
                bool titleExists = await _proposalReader.TitleExistsAsync(newTitle, projectId);
                if (titleExists)
                    throw new InvalidOperationException("Ya existe un proyecto con ese titulo.");
                project.Title = newTitle;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Description))
                project.Description = updateDto.Description.Trim();

            if (updateDto.Duration.HasValue)
            {
                if (updateDto.Duration.Value <= 0)
                    throw new InvalidOperationException("La duracion estimada debe ser mayor a cero.");

                project.EstimatedDuration = updateDto.Duration.Value;
            }

            await _proposalWriter.UpdateAsync(project);
            return project;
        }
    }
}
