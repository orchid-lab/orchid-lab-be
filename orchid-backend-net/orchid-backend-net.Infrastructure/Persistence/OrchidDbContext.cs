using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Infrastructure.Persistence.Configuration;

namespace orchid_backend_net.Infrastructure.Persistence
{
    public class OrchidDbContext(DbContextOptions<OrchidDbContext> options) : DbContext(options), IUnitOfWork
    {
        public virtual DbSet<Characteristics> Characteristic { get; set; }
        public virtual DbSet<ElementInStage> ElementInStage { get; set; }
        public virtual DbSet<ExperimentLogs> ExperimentLogs { get; set; }
        public virtual DbSet<Hybridizations> Hybridization { get; set; }
        public virtual DbSet<Imgs> Imgs { get; set; }
        public virtual DbSet<InfectedSamples> InfectedSamples { get; set; }
        public virtual DbSet<Linkeds> Linkeds { get; set; }
        public virtual DbSet<Referents> Referents { get; set; }
        public virtual DbSet<ReportAttributes> ReportAttributes { get; set; }
        public virtual DbSet<Stages> Stage { get; set; }
        public virtual DbSet<TasksAssign> TaskAssigns { get; set; }
        public virtual DbSet<TaskAttributes> TaskAttributes { get; set; }
        public virtual DbSet<TissueCultureBatches> TissueCultureBatches { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<TaskTemplates> TaskTemplates { get; set; }
        public virtual DbSet<TaskTemplateDetails> TaskTemplateDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrchidDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new ConfigCharacteristic());
            modelBuilder.ApplyConfiguration(new ConfigElementStage());
            modelBuilder.ApplyConfiguration(new ConfigUser());
            modelBuilder.ApplyConfiguration(new ConfigTissueCultureBatch());
            modelBuilder.ApplyConfiguration(new ConfigTaskAttribute());
            modelBuilder.ApplyConfiguration(new ConfigTaskAssign());
            modelBuilder.ApplyConfiguration(new ConfigStage());
            modelBuilder.ApplyConfiguration(new ConfigExperimentLog());
            modelBuilder.ApplyConfiguration(new ConfigReferent());
            modelBuilder.ApplyConfiguration(new ConfigReportAttribute());
            modelBuilder.ApplyConfiguration(new ConfigLinked());
            modelBuilder.ApplyConfiguration(new ConfigInfectedSample());
            modelBuilder.ApplyConfiguration(new ConfigImg());
            modelBuilder.ApplyConfiguration(new ConfigHybridization());
            modelBuilder.ApplyConfiguration(new ConfigTaskTemplates());
            modelBuilder.ApplyConfiguration(new ConfigTaskTemplateDetails());
        }
        private static void ConfigureModel(ModelBuilder modelBuilder)
        {
            #region User
            modelBuilder.Entity<Users>().HasData(
                //admin
                new Users
                {
                    ID = new Guid().ToString(),
                    Email = "nquangthanh88@gmail.com",
                    Name = "Thanh",
                    PhoneNumber = "0786156757",
                    RoleID = 1,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = new Guid().ToString(),
                    Email = "admin@gmail.com",
                    Name = "admin",
                    PhoneNumber = "0786156757",
                    RoleID = 1,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                //researcher
                new Users
                {
                    ID = "researcherjfhdbgvsjdrbgailrg",
                    Email = "researcher@gmail.com",
                    Name = "ResearcherName",
                    PhoneNumber = "0786156757",
                    RoleID = 2,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = "researchernbjwruiegsdkjniwuef",
                    Email = "researcher1@gmail.com",
                    Name = "ResearcherName2",
                    PhoneNumber = "0786156757",
                    RoleID = 2,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = "researcherapwmlkfgblasirgasrg",
                    Email = "researcher2@gmail.com",
                    Name = "ResearcherName2",
                    PhoneNumber = "0786156757",
                    RoleID = 2,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = new Guid().ToString(),
                    Email = "researcher3@gmail.com",
                    Name = "ResearcherName3",
                    PhoneNumber = "0786156757",
                    RoleID = 2,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = new Guid().ToString(),
                    Email = "researcher4@gmail.com",
                    Name = "ResearcherName4",
                    PhoneNumber = "0786156757",
                    RoleID = 2,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = new Guid().ToString(),
                    Email = "researcher5@gmail.com",
                    Name = "ResearcherName5",
                    PhoneNumber = "0786156757",
                    RoleID = 2,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                //technician
                new Users
                {
                    ID = "technicianiasrvsdftnxdfg",
                    Email = "technician@gmail.com",
                    Name = "TechnicianName",
                    PhoneNumber = "0786156757",
                    RoleID = 3,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = "technicianskrgwaefxkbvkserugh",
                    Email = "technician1@gmail.com",
                    Name = "TechnicianName1",
                    PhoneNumber = "0786156757",
                    RoleID = 3,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = "techniciansslgfiawefasdcaerhab",
                    Email = "technician2@gmail.com",
                    Name = "TechnicianName2",
                    PhoneNumber = "0786156757",
                    RoleID = 3,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = "technicianasdcsrgsdfcvsjdtrh",
                    Email = "technician3@gmail.com",
                    Name = "TechnicianName3",
                    PhoneNumber = "0786156757",
                    RoleID = 3,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = "techniciantaserfgzcxvarsghn",
                    Email = "technician4@gmail.com",
                    Name = "TechnicianName4",
                    PhoneNumber = "0786156757",
                    RoleID = 3,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = "techniciandfhbsthbmdiftnqfxt4r",
                    Email = "technician5@gmail.com",
                    Name = "TechnicianName5",
                    PhoneNumber = "0786156757",
                    RoleID = 3,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = "technicianisdvfkasrugsdfvkzsdr",
                    Email = "technician6@gmail.com",
                    Name = "TechnicianName6",
                    PhoneNumber = "0786156757",
                    RoleID = 3,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = new Guid().ToString(),
                    Email = "technician7@gmail.com",
                    Name = "TechnicianName7",
                    PhoneNumber = "0786156757",
                    RoleID = 3,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = new Guid().ToString(),
                    Email = "technician8@gmail.com",
                    Name = "TechnicianName8",
                    PhoneNumber = "0786156757",
                    RoleID = 3,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = new Guid().ToString(),
                    Email = "technician9@gmail.com",
                    Name = "TechnicianName9",
                    PhoneNumber = "0786156757",
                    RoleID = 3,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                },
                new Users
                {
                    ID = new Guid().ToString(),
                    Email = "technician10@gmail.com",
                    Name = "TechnicianName10",
                    PhoneNumber = "0786156757",
                    RoleID = 3,
                    Create_date = DateTime.Now,
                    Status = true,
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                }
                );
            #endregion

            #region Seedling
            modelBuilder.Entity<Seedlings>().HasData(
                new Seedlings
                {
                    ID = "seedlingskrgnkdxfnbsrthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-1",
                    ScientificName = "seedling-1",
                    Parent1 = null,
                    Parent2 = null,
                },
                new Seedlings
                {
                    ID = "seedlingskrgnkdxf2nbsrthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-2",
                    ScientificName = "seedling-2",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfn4bsrthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-3",
                    ScientificName = "seedling-3",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnb1srthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-4",
                    ScientificName = "seedling-4",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnbs25rthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-5",
                    ScientificName = "seedling-5",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedling2skrgnkdxfnbsrthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2025-02-25"),
                    LocalName = "seedling-a",
                    ScientificName = "seedling-a",
                    Parent1 = "seedlingskrgnkdxfnbs25rthg",
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfn6bsrthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2025-04-24"),
                    LocalName = "seedling-6",
                    ScientificName = "seedling-6",
                    Parent1 = "seedling2skrgnkdxfnbsrthg",
                    Parent2 = "seedlingskrgnkdxfnbs25rthg",
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfn3bsrthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2025-04-25"),
                    LocalName = "seedling-7",
                    ScientificName = "seedling-7",
                    Parent1 = "seedlingskrgnkdxfn3bsrthg",
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnb78srthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2025-01-24"),
                    LocalName = "seedling-8",
                    ScientificName = "seedling-8",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnbs4rthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-9",
                    ScientificName = "seedling-9",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnbsrasdfthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-10",
                    ScientificName = "seedling-10",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnbsa2rthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-11",
                    ScientificName = "seedling-11",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnb3bsrthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-12",
                    ScientificName = "seedling-12",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnbsr53thg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-13",
                    ScientificName = "seedling-13",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnbsrbxthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-14",
                    ScientificName = "seedling-14",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnbsrth3g",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-15",
                    ScientificName = "seedling-15",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnbsrthjg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-16",
                    ScientificName = "seedling-16",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnbsr1cthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-17",
                    ScientificName = "seedling-17",
                    Parent1 = null,
                    Parent2 = null,
                }, new Seedlings
                {
                    ID = "seedlingskrgnkdxfnb42srthg",
                    Create_date = DateTime.UtcNow,
                    Dob = DateOnly.Parse("2024-08-24"),
                    LocalName = "seedling-18",
                    ScientificName = "seedling-18",
                    Parent1 = null,
                    Parent2 = null,
                }
                );
            #endregion

            #region SeedlingAttribute
            modelBuilder.Entity<SeedlingAttributes>().HasData(
                new SeedlingAttributes
                {
                    ID = "seedattibute1dfbseth",
                    Name = "Cao",
                    Description = "Chiều cao của mẫu",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibute1sfg3vas",
                    Name = "Rộng",
                    Description = "Chiều rộng của mẫu",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibute1dffsbcfhnt",
                    Name = "Đen",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibute1d4fbseth",
                    Name = "Xanh lá",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattib23fbtute1dfbseth",
                    Name = "Đỏ",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibvbsdrtgsute1dfbseth",
                    Name = "Vàng",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seed5attibute1d2fbseth",
                    Name = "Trắng",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibutebhr1dfbseth",
                    Name = "Đốm",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibute1dfbsgfn2eth",
                    Name = "Lá",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seed5df5attibute1dfbseth",
                    Name = "Độ dài rễ",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibutdfgh643e1dfbseth",
                    Name = "Độ dài thân",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibute142d6gffbseth",
                    Name = "Hoa màu đỏ",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibute24bda1dfbseth",
                    Name = "Hoa màu trắng",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibute1dfkbjtbseth",
                    Name = "Hoa màu xanh ngọc",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedatt1isbuxte1dfbs3eth",
                    Name = "Hoa màu xanh biếc",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibut2e1adfbswbseth",
                    Name = "Hoa màu vàng",
                    Description = "",
                    Status = true,
                },
                new SeedlingAttributes
                {
                    ID = "seedattibute1d2asdcfdryjfbseth",
                    Name = "Hoa màu tím",
                    Description = "",
                    Status = true,
                }
                );
            #endregion

            #region Characteristic
            modelBuilder.Entity<Characteristics>().HasData(
                new Characteristics
                {
                    ID = Guid.NewGuid().ToString(),
                    SeedlingID = "seedlingskrgnkdxfnbsrthg",
                    SeedlingAttributeID = "seedattibute1d2asdcfdryjfbseth",
                    Value = 1,
                    Status = true,
                },
                new Characteristics
                {
                    ID = Guid.NewGuid().ToString(),
                    SeedlingID = "seedlingskrgnkdxfnbsrthg",
                    SeedlingAttributeID = "seed5attibute1d2fbseth",
                    Value = 1,
                    Status = true,
                },
                new Characteristics
                {
                    ID = Guid.NewGuid().ToString(),
                    SeedlingID = "seedlingskrgnkdxfnbsrthg",
                    SeedlingAttributeID = "seedattibute1sfg3vas",
                    Value = 53,
                    Status = true,
                },
                new Characteristics
                {
                    ID = Guid.NewGuid().ToString(),
                    SeedlingID = "seedlingskrgnkdxf2nbsrthg",
                    SeedlingAttributeID = "seedattibute1sfg3vas",
                    Value = 53,
                    Status = true,
                },
                new Characteristics
                {
                    ID = Guid.NewGuid().ToString(),
                    SeedlingID = "seedlingskrgnkdxf2nbsrthg",
                    SeedlingAttributeID = "seed5attibute1d2fbseth",
                    Value = 53,
                    Status = true,
                },
                new Characteristics
                {
                    ID = Guid.NewGuid().ToString(),
                    SeedlingID = "seedlingskrgnkdxf2nbsrthg",
                    SeedlingAttributeID = "seedattibute24bda1dfbseth",
                    Value = 53,
                    Status = true,
                },
                new Characteristics
                {
                    ID = Guid.NewGuid().ToString(),
                    SeedlingID = "seedlingskrgnkdxfn4bsrthg",
                    SeedlingAttributeID = "seedattibute24bda1dfbseth",
                    Value = 53,
                    Status = true,
                }
                );
            #endregion

            #region Task
            modelBuilder.Entity<Tasks>().HasData(
                new Tasks
                {
                    ID = "taskjsfbgksudrybgsdfg",
                    Researcher = "researcherapwmlkfgblasirgasrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbg1ksudrybgsdfg",
                    Researcher = "researchernbjwruiegsdkjniwuef",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 1
                },
                new Tasks
                {
                    ID = "taskjsfb4gksudrybgsdfg",
                    Researcher = "researcherjfhdbgvsjdrbgailrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 2
                },
                new Tasks
                {
                    ID = "t3askjsfbgksudrybg5sdfg",
                    Researcher = "researcherapwmlkfgblasirgasrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 3
                },
                new Tasks
                {
                    ID = "taskjsfbgksudry6bgsdfg",
                    Researcher = "researchernbjwruiegsdkjniwuef",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 4
                },
                new Tasks
                {
                    ID = "taskjsfbgksudry7bgsdfg",
                    Researcher = "researcherjfhdbgvsjdrbgailrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 5
                },
                new Tasks
                {
                    ID = "taskjsfbgksudrybg2sdfg",
                    Researcher = "researchernbjwruiegsdkjniwuef",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 6
                },
                new Tasks
                {
                    ID = "taskjsfbgksu2drybgsdfg",
                    Researcher = "researcherapwmlkfgblasirgasrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksudrybgsd76fg",
                    Researcher = "researchernbjwruiegsdkjniwuef",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksudryb2cgsdfg",
                    Researcher = "researcherjfhdbgvsjdrbgailrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksudrybgs1sxdfg",
                    Researcher = "researcherapwmlkfgblasirgasrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksudryb4gbgsdfg",
                    Researcher = "researchernbjwruiegsdkjniwuef",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksudryb32sgsdfg",
                    Researcher = "researcherjfhdbgvsjdrbgailrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksud325drybgsdfg",
                    Researcher = "researcherapwmlkfgblasirgasrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksudrybgs13adfg",
                    Researcher = "researcherjfhdbgvsjdrbgailrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksudr5124avhjaybgsdfg",
                    Researcher = "researcherjfhdbgvsjdrbgailrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksudryb5cvg24tsdfsdfg",
                    Researcher = "researchernbjwruiegsdkjniwuef",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksudr5dsfyb5cvgsdfg",
                    Researcher = "researchernbjwruiegsdkjniwuef",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgks31radfudryb5cvgsdfg",
                    Researcher = "researcherapwmlkfgblasirgasrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksgbsf23udryb5cvgsdfg",
                    Researcher = "researcherapwmlkfgblasirgasrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksud3baryb5cvgsdfg",
                    Researcher = "researchernbjwruiegsdkjniwuef",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                },
                new Tasks
                {
                    ID = "taskjsfbgksASudryb5cvgsdfg",
                    Researcher = "researcherjfhdbgvsjdrbgailrg",
                    Name = "Sterilization Task",
                    Description ="Task for sterilizing samples",
                    Start_date = DateTime.Parse("2025-07-17T16:30:27.555262Z"),
                    End_date = DateTime.Parse("2025-07-22T16:30:27.555262Z"),
                    Create_at = DateTime.Parse("2025-07-20T16:30:27.555262Z"),
                    Status = 0
                }
                );
            #endregion

            #region TaskAssign
            modelBuilder.Entity<TasksAssign>().HasData(
                new TasksAssign
                {

                }
                );
            #endregion

            #region TaskAttribute
            modelBuilder.Entity<TaskAttributes>().HasData(
                new TaskAttributes
                {

                }
                );
            #endregion

            #region Method
            modelBuilder.Entity<Methods>().HasData(
                new Methods
                {

                }
                );
            #endregion

            #region Stage
            modelBuilder.Entity<Stages>().HasData(
                new Stages
                {

                }
                );
            #endregion

            #region Element
            modelBuilder.Entity<Elements>().HasData(
                new Elements
                {

                }
                );
            #endregion

            #region ElementIsStage
            modelBuilder.Entity<ElementInStage>().HasData(
                new ElementInStage
                {

                }
                );
            #endregion

            #region ExperimentLog
            modelBuilder.Entity<ExperimentLogs>().HasData(
                new ExperimentLogs
                {

                }
                );
            #endregion

            #region Linked
            modelBuilder.Entity<Linkeds>().HasData(
                new Linkeds
                {

                }
                );
            #endregion

            #region LabRoom
            modelBuilder.Entity<LabRooms>().HasData(
                new LabRooms
                {

                }
                );
            #endregion

            #region TissueCulcureBatch
            modelBuilder.Entity<TissueCultureBatches>().HasData(
                new TissueCultureBatches
                {

                }
                );
            #endregion

            #region Diseases
            modelBuilder.Entity<Diseases>().HasData(
                
                );
            #endregion
        }
    }
}
