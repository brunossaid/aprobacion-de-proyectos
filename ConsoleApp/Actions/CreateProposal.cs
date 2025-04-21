using Application.Interfaces;
using Application.Services;
using ConsoleApp.Core;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;


namespace ConsoleApp.Actions;

public static class CreateProposal
{
    public static async Task Execute(User currentUser, IServiceProvider services)
    {
        var proposalService = services.GetRequiredService<IProjectProposalService>();
        var areaService = services.GetRequiredService<IAreaService>();
        var typeService = services.GetRequiredService<IProjectTypeService>();

        var areas = await areaService.GetAllAsync();
        var types = await typeService.GetAllAsync();

        UI.ShowTitle();

        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"Usuario actual: {currentUser.Name}");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("Crear nueva solicitud de proyecto\n");
        Console.ResetColor();

        // titulo
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write("Titulo: ");
        Console.ResetColor();
        string title = Console.ReadLine()!;
        while (string.IsNullOrWhiteSpace(title))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("El titulo no puede estar vacio. Intente nuevamente: ");
            Console.ResetColor();
            title = Console.ReadLine()!;
        }

        // descripcion
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write("Descripcion: ");
        Console.ResetColor();
        string description = Console.ReadLine()!;
        while (string.IsNullOrWhiteSpace(description))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("La descripcion no puede estar vacia. Intente nuevamente: ");
            Console.ResetColor();
            description = Console.ReadLine()!;
        }
        Console.WriteLine("");

        // area
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Area del proyecto:");
        Console.ResetColor();

        for (int i = 0; i < areas.Count; i++)
            Console.WriteLine($"{i + 1}. {areas[i].Name}");

        Area selectedArea = null!;
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Seleccione el numero correspondiente: ");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out int input) &&
                input >= 1 && input <= areas.Count)
            {
                selectedArea = areas[input - 1];
                break;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Seleccion invalida. Intente nuevamente: ");
            Console.ResetColor();
        }
        Console.WriteLine();

        // type
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Tipo de proyecto:");
        Console.ResetColor();

        for (int i = 0; i < types.Count; i++)
            Console.WriteLine($"{i + 1}. {types[i].Name}");

        ProjectType selectedType = null!;
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Seleccione el numero correspondiente: ");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out int input) &&
                input >= 1 && input <= types.Count)
            {
                selectedType = types[input - 1]; 
                break;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Selección invalida. Intente nuevamente: ");
            Console.ResetColor();
        }
        Console.WriteLine();

        // monto estimado
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write("Monto estimado: ");
        Console.ResetColor();
        decimal estimatedAmount;
        while (true)
        {
            string input = Console.ReadLine()!;
            if (!decimal.TryParse(input, out estimatedAmount) || estimatedAmount <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Por favor, ingrese un monto valido mayor a 0: ");
                Console.ResetColor();
            }
            else if (estimatedAmount > 1_000_000_000)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("El monto no puede superar los $1.000.000.000: ");
                Console.ResetColor();
            }
            else break;
        }

        // duracion estimada
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write("Duracion estimada del proyecto (en meses): ");
        Console.ResetColor();
        int estimatedDuration;
        while (true)
        {
            string input = Console.ReadLine()!;
            if (!int.TryParse(input, out estimatedDuration) || estimatedDuration <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Por favor, ingrese una cantidad valida de meses: ");
                Console.ResetColor();
            }
            else if (estimatedDuration > 1200)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("La duracion no puede superar los 1200 meses: ");
                Console.ResetColor();
            }
            else break;
        }

        // confirmacion
        Console.Clear();
        UI.ShowTitle();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Estas a punto de crear la siguiente solicitud de proyecto:\n");
        Console.ResetColor();

        Console.WriteLine($"Titulo: {title}");
        Console.WriteLine($"Descripcion: {description}");
        Console.WriteLine($"Area: {selectedArea.Name}");
        Console.WriteLine($"Tipo: {selectedType.Name}");
        Console.WriteLine($"Monto estimado: ${estimatedAmount}");
        Console.WriteLine($"Tiempo estimado: {estimatedDuration} meses");

        string? confirmation = null;
        while (confirmation != "s" && confirmation != "n")
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("\nDeseas continuar con la creacion de la solicitud? (s/n): ");
            Console.ResetColor();

            confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation != "s" && confirmation != "n")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opcion invalida. Por favor, ingrese 's' para sí o 'n' para no.");
                Console.ResetColor();
            }
        }

        if (confirmation == "s")
        {
            var proposalCreationService = services.GetRequiredService<ProposalCreationService>();

            var proposal = new ProjectProposal
            {
                Title = title,
                Description = description,
                Area = selectedArea.Id,
                Type = selectedType.Id,
                EstimatedAmount = estimatedAmount,
                EstimatedDuration = estimatedDuration,
                CreateBy = currentUser.Id,
                Status = 1 // pending
            };

            try
            {
                await proposalCreationService.CreateProposalWithStepsAsync(proposal);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nSolicitud de proyecto creada con exito.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error al guardar la solicitud:");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);

                if (ex.InnerException != null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Detalles internos: {ex.InnerException.Message}");
                }

                Console.ResetColor();
            }
        }
        else 
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nCreacion de solicitud cancelada.");
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ResetColor();
        Console.ReadKey();

    }
}
