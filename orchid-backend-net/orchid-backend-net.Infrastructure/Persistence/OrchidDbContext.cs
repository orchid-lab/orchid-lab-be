
using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Infrastructure.Persistence.Configuration;

namespace orchid_backend_net.Infrastructure.Persistence
{
    public class OrchidDbContext(DbContextOptions<OrchidDbContext> options) : DbContext(options), IUnitOfWork
    {
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<TaskAssignment> TaskAssignments { get; set; }
        public virtual DbSet<TaskAttributes> TaskAttributes { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<MonitoringLogs> MonitoringLogs { get; set; }
        public virtual DbSet<LogDetails> MonitoringLogDetails { get; set; }
        public virtual DbSet<Imgs> Imgs { get; set; }
        public virtual DbSet<AnalyticResults> AnalyticResults { get; set; }
        public virtual DbSet<SamplesRequirements> SamplesRequirements { get; set; }
        public virtual DbSet<ExperimentLogs> ExperimentLogs { get; set; }
        public virtual DbSet<Seedlings> Seedlings { get; set; }
        public virtual DbSet<SeedlingsTraits> SeedlingsTraits { get; set; }
        public virtual DbSet<Characteristic> Characteristics { get; set; }
        public virtual DbSet<StageChemicals> StageChemicals { get; set; }
        public virtual DbSet<StageMaterials> StageMaterials { get; set; }
        public virtual DbSet<Stages> Stages { get; set; }
        public virtual DbSet<Methods> Methods { get; set; }
        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<Chemicals> Chemicals { get; set; }
        public virtual DbSet<Samples> Samples { get; set; }
        public virtual DbSet<Hybridzations> Hybridzations { get; set; }
        public virtual DbSet<LabRooms> LabRooms { get; set; }
        public virtual DbSet<Batches> Batches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrchidDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new ConfigUser());
        }
    }
}
