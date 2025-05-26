using Application.Interfaces;
using Application.Services;
using ConsoleApp.Core;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Actions;

public static class ReviewProposalDetail
{
    public static async Task Execute(User currentUser, IServiceProvider services, ProjectApprovalStep step)
    {
        var proposalService = services.GetRequiredService<IProjectProposalReader>();
        var approvalManager = services.GetRequiredService<ApprovalStepManager>();

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
            Console.Write("Opción: ");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out status) && status >= 1 && status <= 4)
                break;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Selección invalida. Intente nuevamente.");
            Console.ResetColor();
        }

        string? observations = null;
        if (status != 1)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Observacion (opcional): ");
            Console.ResetColor();
            observations = Console.ReadLine();
        }

        switch (status)
        {
            case 1:
                // excepcion
                break;
            case 2:
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Solicitud aprobada.");
                break;
            case 3:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Solicitud rechazada.");
                break;
            case 4:
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Solicitud observada.");
                break;
        }

        

        try
        {
            var dto = new DecisionDto
            {
                Id = step.Id,
                Status = status,
                Observation = observations,
                User = currentUser.Id
            };

            await approvalManager.DecideNextStepAsync(step.ProjectProposalId, dto);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nSolicitud revisada exitosamente.");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{ex.Message}");
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ResetColor();
        Console.ReadKey();
        Console.Clear();
    }
}
