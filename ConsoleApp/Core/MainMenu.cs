using ConsoleApp.Actions;
using Domain.Entities;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection; 

namespace ConsoleApp.Core;

public static class MainMenu
{
    public static async Task Execute(User currentUser, IServiceProvider services)
    {
        var stepService = services.GetRequiredService<IProjectApprovalStepReader>();
        while (true)
        {
            var steps = await stepService.GetAllAsync();
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
            .ToList()
            .Count();


            UI.ShowTitle();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"Usuario actual: {currentUser.Name}\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Seleciona una opcion:");
            Console.ResetColor();

            Console.WriteLine("1. Crear nueva solicitud de proyecto");
            Console.WriteLine("2. Ver mis solicitudes");
            Console.Write("3. Revisar solicitudes ");
            if (pendingSteps > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"({pendingSteps})");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine();
            }
            Console.WriteLine("4. Cambiar usuario");

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Opcion: ");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out int selected) && selected >= 1 && selected <= 4)
            {
                switch (selected)
                {
                    case 1:
                        await CreateProposal.Execute(currentUser, services);
                        break;
                    case 2:
                        await ViewProposals.Execute(currentUser, services);
                        break;
                    case 3:
                        await ReviewProposals.Execute(currentUser, services);
                        break;
                    case 4:
                        return; // volver al loginmenu
                }
            }
        }
    }



}
