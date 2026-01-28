using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using orchid_backend_net.Domain.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Entities.Base;
using orchid_backend_net.Infrastructure.Persistence.Configuration;

namespace orchid_backend_net.Infrastructure.Persistence
{
    public class OrchidDbContext : DbContext, IUnitOfWork
    {
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly ILogger<OrchidDbContext>? _logger;

        public OrchidDbContext(
            DbContextOptions<OrchidDbContext> options, 
            IDomainEventDispatcher dispatcher,
            ILogger<OrchidDbContext>? logger = null) 
            : base(options)
        {
            _dispatcher = dispatcher;
            _logger = logger;
        }
        public virtual DbSet<AnalyticResults> AnalyticResults { get; set; }
        public virtual DbSet<Batches> Batches { get; set; }
        public virtual DbSet<Characteristic> Characteristics { get; set; }
        public virtual DbSet<Chemicals> Chemicals { get; set; }
        public virtual DbSet<Disease> Diseases { get; set; }
        public virtual DbSet<ExperimentLogs> ExperimentLogs { get; set; }
        public virtual DbSet<Imgs> Imgs { get; set; }
        public virtual DbSet<LabRooms> LabRooms { get; set; }
        public virtual DbSet<LogDetails> MonitoringLogDetails { get; set; }
        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<Methods> Methods { get; set; }
        public virtual DbSet<MethodStageDefinition> MethodStageDefinition { get; set; }
        public virtual DbSet<MethodStages> MethodStages { get; set; }
        public virtual DbSet<MonitoringLogs> MonitoringLogs { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Samples> Samples { get; set; }
        public virtual DbSet<SamplesRequirementsDefinition> SamplesRequirementDefinitions { get; set; }
        public virtual DbSet<SampleStage> SampleStages {  get; set; }
        public virtual DbSet<SampleStageDefinition> SampleStageDefinition { get; set; }
        public virtual DbSet<Seedlings> Seedlings { get; set; }
        public virtual DbSet<SeedlingsTraits> SeedlingsTraits { get; set; }
        public virtual DbSet<StageChemicals> StageChemicals { get; set; }
        public virtual DbSet<StageMaterials> StageMaterials { get; set; }
        public virtual DbSet<StageRequirementDefinition> StageRequirementDefinitions { get; set; }
        public virtual DbSet<TaskAssignment> TaskAssignments { get; set; }
        public virtual DbSet<TaskAttributes> TaskAttributes { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<TaskCheckList> TaskChecks { get; set; }
        public virtual DbSet<TaskCheckListItem> TaskCheckListItems { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrchidDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new ConfigUser());
            modelBuilder.ApplyConfiguration(new ConfigMethod());
            modelBuilder.ApplyConfiguration(new ConfigMethodStage());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("SaveChangesAsync started - tracking {EntryCount} entities", ChangeTracker.Entries().Count());
            
            var domainEntities = ChangeTracker
                .Entries<BaseEntity<Guid>>()
                .Where(e => e.Entity.DomainEvents.Count != 0)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            if (domainEvents.Any())
            {
                _logger?.LogDebug("Saving changes with {EventCount} domain events to dispatch", domainEvents.Count);
            }

            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                _logger?.LogDebug("SaveChangesAsync completed - {ChangesCount} changes persisted", result);

                await _dispatcher.DispatchAsync(domainEvents);
                _logger?.LogDebug("Domain events dispatched successfully");

                domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred while saving changes to database");
                throw;
            }
        }
    }
}
