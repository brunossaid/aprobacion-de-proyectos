using Domain.Entities;
using ConsoleApp.Core;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ConsoleApp.Actions;

public static class ViewProposals
{
    public static async Task Execute(User currentUser, IServiceProvider services)
    {
        while (true)
        {
            UI.ShowTitle();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"Usuario actual: {currentUser.Name}");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\nMis solicitudes de proyecto");
            Console.ResetColor();

            var proposalService = services.GetRequiredService<IProjectProposalService>();
            var proposals = await proposalService.GetAllAsync();
            var userProposals = proposals.Where(p => p.CreateBy == currentUser.Id).ToList();


            for(int i = 0; i < userProposals.Count; i++)
            {
                var proposal = userProposals[i];
                Console.Write($"{i + 1}. {proposal.Title} - ");
                switch(proposal.Status){
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
            }
            Console.WriteLine("0. Volver al menu principal");


            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Opcion: ");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out int selected))
            {
                if (selected == 0)
                {
                    return; // vuelve al menú principal
                }
                else if (selected >= 1 && selected <= userProposals.Count)
                {
                    var selectedProposal = userProposals[selected - 1];
                    await ViewProposalDetail.Execute(selectedProposal, services, currentUser);
                }
            }
        }
    }
}
