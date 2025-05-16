using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User => Set<User>();
        public DbSet<ApproverRole> ApproverRole => Set<ApproverRole>();
        public DbSet<ApprovalStatus> ApprovalStatus => Set<ApprovalStatus>();
        public DbSet<Area> Area => Set<Area>();
        public DbSet<ProjectProposal> ProjectProposal => Set<ProjectProposal>();
        public DbSet<ProjectType> ProjectType => Set<ProjectType>();
        public DbSet<ProjectApprovalStep> ProjectApprovalStep => Set<ProjectApprovalStep>();
        public DbSet<ApprovalRule> ApprovalRule => Set<ApprovalRule>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=AprobacionProyectosDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // datos iniciales
            modelBuilder.Entity<ApproverRole>().HasData(
                new ApproverRole { Id = 1, Name = "Líder de Área" },
                new ApproverRole { Id = 2, Name = "Gerente" },
                new ApproverRole { Id = 3, Name = "Director" },
                new ApproverRole { Id = 4, Name = "Comité Técnico" }
            );

            modelBuilder.Entity<ApprovalStatus>().HasData(
                new ApprovalStatus { Id = 1, Name = "Pending" },
                new ApprovalStatus { Id = 2, Name = "Approved" },
                new ApprovalStatus { Id = 3, Name = "Rejected" },
                new ApprovalStatus { Id = 4, Name = "Observed" }
            );

            modelBuilder.Entity<ProjectType>().HasData(
                new ProjectType { Id = 1, Name = "Mejora de Procesos" },
                new ProjectType { Id = 2, Name = "Innovación y Desarrollo" },
                new ProjectType { Id = 3, Name = "Infraestructura" },
                new ProjectType { Id = 4, Name = "Capacitación Interna" }
            );

            modelBuilder.Entity<Area>().HasData(
                new Area { Id = 1, Name = "Finanzas" },
                new Area { Id = 2, Name = "Tecnología" },
                new Area { Id = 3, Name = "Recursos Humanos" },
                new Area { Id = 4, Name = "Operaciones" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "José Ferreyra", Email = "jferreyra@unaj.com", Role = 2 },
                new User { Id = 2, Name = "Ana Lucero", Email = "alucero@unaj.com", Role = 1 },
                new User { Id = 3, Name = "Gonzalo Molinas", Email = "gmolinas@unaj.com", Role = 2 },
                new User { Id = 4, Name = "Lucas Olivera", Email = "lolivera@unaj.com", Role = 3 },
                new User { Id = 5, Name = "Danilo Fagundez", Email = "dfagundez@unaj.com", Role = 4 },
                new User { Id = 6, Name = "Gabriel Galli", Email = "ggalli@unaj.com", Role = 4 }
            );

            modelBuilder.Entity<ApprovalRule>().HasData(
                new ApprovalRule { Id = 1, MinAmount = 0, MaxAmount = 10000, Area = null, Type = null, StepOrder = 1, ApproverRoleId = 1 },
                new ApprovalRule { Id = 2, MinAmount = 5000, MaxAmount = 20000, Area = null, Type = null, StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 3, MinAmount = 0, MaxAmount = 20000, Area = 2, Type = 2, StepOrder = 1, ApproverRoleId = 2 },
                new ApprovalRule { Id = 4, MinAmount = 20000, MaxAmount = 0, Area = null, Type = null, StepOrder = 3, ApproverRoleId = 3 },
                new ApprovalRule { Id = 5, MinAmount = 5000, MaxAmount = 0, Area = 1, Type = 1, StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 6, MinAmount = 0, MaxAmount = 10000, Area = null, Type = 2, StepOrder = 1, ApproverRoleId = 1 },
                new ApprovalRule { Id = 7, MinAmount = 0, MaxAmount = 10000, Area = 2, Type = 2, StepOrder = 1, ApproverRoleId = 4 },
                new ApprovalRule { Id = 8, MinAmount = 10000, MaxAmount = 30000, Area = 2, Type = null, StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 9, MinAmount = 30000, MaxAmount = 0, Area = 3, Type = null, StepOrder = 2, ApproverRoleId = 3 },
                new ApprovalRule { Id = 10, MinAmount = 0, MaxAmount = 50000, Area = null, Type = 4, StepOrder = 1, ApproverRoleId = 1 }
            );

            // relaciones
            // ApprovalRule
            modelBuilder.Entity<ApprovalRule>()
                .HasOne(a => a.AreaNavigation)
                .WithMany()
                .HasForeignKey(a => a.Area)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovalRule>()
                .HasOne(a => a.TypeNavigation)
                .WithMany()
                .HasForeignKey(a => a.Type)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovalRule>()
                .HasOne(a => a.ApproverRoleNavigation)
                .WithMany()
                .HasForeignKey(a => a.ApproverRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProjectProposal
            modelBuilder.Entity<ProjectProposal>()
                .HasOne(p => p.AreaNavigation)
                .WithMany()
                .HasForeignKey(p => p.Area)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectProposal>()
                .HasOne(p => p.TypeNavigation)
                .WithMany()
                .HasForeignKey(p => p.Type)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectProposal>()
                .HasOne(p => p.StatusNavigation)
                .WithMany()
                .HasForeignKey(p => p.Status)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectProposal>()
                .HasOne(p => p.CreateByNavigation)
                .WithMany()
                .HasForeignKey(p => p.CreateBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectProposal>()
            .Property(p => p.CreateAt)
            .HasDefaultValueSql("GETDATE()");

            // ProjectApprovalStep
            modelBuilder.Entity<ProjectApprovalStep>()
                .HasOne(p => p.ProjectProposalNavigation)
                .WithMany()
                .HasForeignKey(p => p.ProjectProposalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectApprovalStep>()
                .HasOne(p => p.ApproverUserNavigation)
                .WithMany()
                .HasForeignKey(p => p.ApproverUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectApprovalStep>()
                .HasOne(p => p.ApproverRoleNavigation)
                .WithMany()
                .HasForeignKey(p => p.ApproverRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectApprovalStep>()
                .HasOne(p => p.StatusNavigation)
                .WithMany()
                .HasForeignKey(p => p.Status)
                .OnDelete(DeleteBehavior.Restrict);

            // User
            modelBuilder.Entity<User>()
                .HasOne(u => u.RoleNavigation)
                .WithMany()
                .HasForeignKey(u => u.Role)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
