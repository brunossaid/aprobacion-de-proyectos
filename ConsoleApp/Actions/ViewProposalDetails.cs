using Domain.Entities;
using ConsoleApp.Core;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Actions;

public static class ViewProposalDetail
{
    public static async Task Execute(ProjectProposal proposal, IServiceProvider services, User currentUser)
    {
        var stepService = services.GetRequiredService<IProjectApprovalStepService>();
        var steps = await stepService.GetAllAsync();
        var proposalSteps = steps
            .Where(s => s.ProjectProposalId == proposal.Id)
            .OrderBy(s => s.StepOrder)
            .ToList();


        UI.ShowTitle();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"Usuario actual: {currentUser.Name}");
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Detalles de la solicitud de proyecto");
        Console.ResetColor();

        Console.Write("Estado: ");
        switch (proposal.Status)
        {
            case 1:
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Pendiente");
                break;
            case 2:
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Aprobada");
                break;
            case 3:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Rechazada");
                break;
        }
        Console.ResetColor();

        Console.WriteLine($"Titulo: {proposal.Title}");
        Console.WriteLine($"Descripcion: {proposal.Description}");
        Console.WriteLine($"Area: {proposal.AreaNavigation?.Name}");
        Console.WriteLine($"Tipo: {proposal.TypeNavigation?.Name}");
        Console.WriteLine($"Monto estimado: $ {proposal.EstimatedAmount}");
        Console.WriteLine($"Duración estimada: {proposal.EstimatedDuration} meses\n");

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Pasos de aprobacion:");
        Console.ResetColor();
        foreach (var step in proposalSteps){
            Console.Write($"Paso {step.StepOrder} ({step.ApproverRoleNavigation.Name}) ");
            switch (step.Status)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Pendiente");
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Aprobado");
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Rechazado");
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Observado");
                    break;
            }

            if (!string.IsNullOrWhiteSpace(step.Observations))
            {
                Console.ResetColor();
                Console.WriteLine($"Observacion: {step.Observations}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("No se agrego ninguna observacion.");
            }
            Console.WriteLine("");

            Console.ResetColor();
        }
        
        

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nPresione cualquier tecla para volver...");
        Console.ResetColor();
        Console.ReadKey();
    }
}
