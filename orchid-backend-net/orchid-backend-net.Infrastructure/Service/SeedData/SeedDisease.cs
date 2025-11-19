using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedDisease
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!context.Set<Diseases>().Any())
            {
                var diseases = new List<Diseases>
                {
                    new Diseases
                    {
                        ID = "disease_anthracnose",
                        Name = "Anthracnose",
                        InfectedRate = 50,
                        Description = "Description",
                        Status = true,
                        Solution = ""
                    },
                    new Diseases
                    {
                        ID = "disease_bacterial_wilt",
                        Name = "Bacterial Wilt",
                        InfectedRate = 50,
                        Description = "Description",
                        Status = true,
                        Solution = ""
                    },
                    new Diseases
                    {
                        ID = "disease_blackrot",
                        Name = "Blackrot",
                        InfectedRate = 50,
                        Description = "Description",
                        Status = true,
                        Solution = ""
                    },
                    new Diseases
                    {
                        ID = "disease_brownspots",
                        Name = "Brownspots",
                        InfectedRate = 50,
                        Description = "Description",
                        Status = true,
                        Solution = ""
                    },
                    new Diseases
                    {
                        ID = "disease_mold_bac",
                        Name = "Mold Bacterial",
                        InfectedRate = 50,
                        Description = "Description",
                        Status = true,
                        Solution = ""
                    },
                    new Diseases
                    {
                        ID = "disease_mold_fungus",
                        Name = "Mold Fungus",
                        InfectedRate = 50,
                        Description = "Description",
                        Status = true,
                        Solution = ""
                    },
                    new Diseases
                    {
                        ID = "disease_soft_rot",
                        Name = "Soft Rot",
                        InfectedRate = 50,
                        Description = "Description",
                        Status = true,
                        Solution = ""
                    },
                    new Diseases
                    {
                        ID = "disease_stemrot",
                        Name = "Stem Rot",
                        InfectedRate = 50,
                        Description = "Description",
                        Status = true,
                        Solution = ""
                    },
                    new Diseases
                    {
                        ID = "disease_withered_yellow_root",
                        Name = "Withered Yellow Root",
                        InfectedRate = 50,
                        Description = "Description",
                        Status = true,
                        Solution = ""
                    },
                    new Diseases
                    {
                        ID = "oxidation",
                        Name = "Oxidation",
                        InfectedRate = 50,
                        Description = "Description",
                        Status = true,
                        Solution = ""
                    },
                    new Diseases
                    {
                        ID = "virus",
                        Name = "Virus",
                        InfectedRate = 50,
                        Description = "Description",
                        Status = true,
                        Solution = ""
                    },
                };
                await context.Set<Diseases>().AddRangeAsync(diseases);
                await context.SaveChangesAsync();
            }
        }
    }
}
