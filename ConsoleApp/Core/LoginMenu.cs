using Application.Interfaces;
using Domain.Entities;

namespace ConsoleApp.Core;

public static class LoginMenu
{
    public static async Task<User> Execute(IUserService userService)
    {
        while (true)
        {
            UI.ShowTitle();

            var users = await userService.GetAllAsync();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Seleccionar usuario: ");
            Console.ResetColor();

            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {users[i].Name}"); 
            }

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Opcion: ");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out int selected) && selected >= 1 && selected <= users.Count)
            {
                return users[selected - 1]; 
            }
        }
    }
}
