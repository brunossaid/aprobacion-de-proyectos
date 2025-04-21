using Application.Interfaces;
using ConsoleApp.Core;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Actions;

public static class ReviewProposalDetail
{
    public static async Task Execute(User currentUser, IServiceProvider services, ProjectApprovalStep step)
    {
        var proposalService = services.GetRequiredService<IProjectProposalService>();
        var stepService = services.GetRequiredService<IProjectApprovalStepService>();
        var proposal = await proposalService.GetByIdAsync(step.ProjectProposalId);

        if (proposal == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No se pudo encontrar la solicitud vinculada.");
            Console.ResetColor();
            Console.ReadKey();
            return;
        }

        UI.ShowTitle();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Revision de solicitud de proyecto");
        Console.ResetColor();

        Console.WriteLine($"Titulo: {proposal.Title}");
        Console.WriteLine($"Descripción: {proposal.Description}");
        Console.WriteLine($"Area: {proposal.AreaNavigation.Name}");
        Console.WriteLine($"Tipo: {proposal.TypeNavigation.Name}");
        Console.WriteLine($"Monto estimado: $ {proposal.EstimatedAmount}");
        Console.WriteLine($"Duración estimada: {proposal.EstimatedDuration} meses\n");

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Selecciona un nuevo estado:");
        Console.ResetColor();
        Console.WriteLine("1. Pendiente");
        Console.WriteLine("2. Aprobado");
        Console.WriteLine("3. Rechazado");
        Console.WriteLine("4. Observado");

        int status;
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Opcion: ");
            Console.Write("");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out status) && status >= 1 && status <= 4)
                break;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Seleccion invalida. Intente nuevamente.");
            Console.ResetColor();
        }

        string? observations = null;
        switch (status)
        {
            case 1:
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("La solicitud sigue pendiente.");
                break;
            case 2:
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Solicitud Aprobada.");
                
                var allSteps = await stepService.GetAllAsync();
                var relatedSteps = allSteps.Where(s => s.ProjectProposalId == step.ProjectProposalId).ToList();

                bool allApproved = relatedSteps.All(s => s.Id == step.Id || s.Status == 2);
                if (allApproved)
                {
                    proposal.Status = 2; 
                    await proposalService.UpdateAsync(proposal);
                }

                break;
            case 3:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Solicitud rechazada.");

                if (proposal != null)
                {
                    proposal.Status = 3;
                    await proposalService.UpdateAsync(proposal);
                }
                break;
            case 4:
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Solicitud observada.");
                break;
        }


        if (status != 1)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Observacion (opcional): ");
            Console.ResetColor();
            observations = Console.ReadLine();
        }

        step.Status = status;
        step.Observations = observations;
        step.DecisionDate = DateTime.Now;
        step.ApproverUserId = currentUser.Id;

        await stepService.UpdateAsync(step);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nSolicitud revisada exitosamente.");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }
}
