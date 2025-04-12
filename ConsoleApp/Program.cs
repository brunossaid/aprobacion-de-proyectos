using Application.Interfaces;
using Application.Services;
using ConsoleApp.Core;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// coneccion a la db
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=AprobacionProyectosDB;Trusted_Connection=True;TrustServerCertificate=True;"));

// servicios
services.AddScoped<IUserService, UserService>();
services.AddScoped<IProjectProposalService, ProjectProposalService>();
services.AddScoped<IAreaService, AreaService>();
services.AddScoped<IProjectTypeService, ProjectTypeService>();
services.AddScoped<IProjectApprovalStepService, ProjectApprovalStepService>();
services.AddScoped<ProposalCreationService>();
services.AddScoped<IApprovalRuleService, ApprovalRuleService>();
var serviceProvider = services.BuildServiceProvider();

// loop de login y menu principal
while (true)
{
    var userService = serviceProvider.GetRequiredService<IUserService>();

    // login
    var currentUser = await LoginMenu.Execute(userService);

    // menu principal
    await MainMenu.Execute(currentUser, serviceProvider);
}
