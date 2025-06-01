namespace ConsoleApp.Core;

public static class UI
{
    public static void ShowTitle()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=======================================");
        Console.ResetColor();
        Console.WriteLine("        APROBACION DE PROYECTOS       ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=======================================\n");
        Console.ResetColor();
    }
}
