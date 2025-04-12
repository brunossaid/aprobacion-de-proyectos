using Domain.Entities;
using ConsoleApp.Core;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;

namespace ConsoleApp.Actions;

public static class ReviewProposals
{
    public static async Task Execute(User currentUser, IServiceProvider services)
    {
        var stepService = services.GetRequiredService<IProjectApprovalStepService>();
        var proposalService = services.GetRequiredService<IProjectProposalService>();

        var steps = await stepService.GetAllAsync();
        var proposals = await proposalService.GetAllAsync();

        // filtrar steps pendientes, coincidan con el role del usuario actual y que los anteriores esten aprobados
        var pendingSteps = steps
            .Where(step =>
                step.ApproverRoleId == currentUser.Role &&          
                step.Status != 2 && 
                step.Status != 3 &&                                
                steps                                              
                    .Where(previousStep =>
                        previousStep.ProjectProposalId == step.ProjectProposalId &&  
                        previousStep.StepOrder < step.StepOrder)                     
                    .All(previousStep => previousStep.Status == 2)) 
            .ToList();

        if (!pendingSteps.Any())
        {
            UI.ShowTitle();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("No hay solicitudes pendientes para revisar.");
            Console.ResetColor();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            return;
        }

        while (true)
        {
            UI.ShowTitle();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"Usuario actual: {currentUser.Name}");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\nSelecciona la solicitud a revisar:");
            Console.ResetColor();

            for (int i = 0; i < pendingSteps.Count; i++)
            {
                var step = pendingSteps[i];
                var proposal = proposals.FirstOrDefault(p => p.Id == step.ProjectProposalId);
                if (proposal != null)
                {
                    Console.Write($"{i + 1}. {proposal.Title} ($ {proposal.EstimatedAmount}) ");
                    switch (step.Status)
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Pendiente");
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Observado");
                            break;
                    }
                    Console.ResetColor();
                }
            }
            Console.WriteLine("0. Volver al menu principal");

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("\nIngrese el numero de la solicitud: ");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out int selection) &&
                selection >= 1 && selection <= pendingSteps.Count)
            {
                var selectedStep = pendingSteps[selection - 1];
                await ReviewProposalDetail.Execute(currentUser, services, selectedStep);
                return; 
            }
            else if (selection == 0)
            {
                return;
            }
        }
    }
}
