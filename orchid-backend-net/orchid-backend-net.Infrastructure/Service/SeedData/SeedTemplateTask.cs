using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Common.Const;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedTemplateTask
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (await context.Set<Tasks>().AnyAsync(t => t.StageId != null))
                return;

            var stages = await context.Set<MethodStageDefinition>().ToListAsync();
            var materials = await context.Set<Materials>().ToListAsync();
            var chemicals = await context.Set<Chemicals>().ToListAsync();

            int S(string name) => stages.First(x => x.Name.StartsWith(name)).ID;
            int M(string name) => materials.First(x => x.Name == name).ID;
            int C(string name) => chemicals.First(x => x.Name == name).ID;

            var tasks = new List<Tasks>
        {
            // =========================
            // SAMPLING
            // =========================
            new Tasks
            {
                Name = "Chuẩn bị và cắt mẫu",
                Description = "Cắt mô ban đầu (đỉnh sinh trưởng, mắt ngủ, lát mô) sẵn sàng cho khử trùng.",
                StageId = S(StageNames.SAMPLING),
                Status = Domain.Common.Enum.TaskStatus.Template,
                TaskAttributes =
                {
                    new() { MaterialId = M(MaterialNames.FORCEPS), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.SCALPEL), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.BLADE), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.TRAY), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.GLOVES), Unit = Unit.MATERIAL_UNIT, Value = 1 }
                },
                CreatedBy = "Hệ thống",
            },

            // =========================
            // STERILIZATION
            // =========================
            new Tasks
            {
                Name = "Rửa sơ bộ mẫu",
                Description = "Rửa mẫu dưới vòi nước và xà phòng để loại bỏ tạp chất ban đầu.",
                StageId = S(StageNames.STERILIZATION),
                Status = Domain.Common.Enum.TaskStatus.Template,
                TaskAttributes =
                {
                    new() { MaterialId = M("Vòi nước"), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M("Bồn nước"), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M("Xà phòng"), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M("Cọ rửa chai"), Unit = Unit.MATERIAL_UNIT, Value = 1 }
                },
                CreatedBy = "Hệ thống",

            },

            new Tasks
            {
                Name = "Khử trùng hóa học mẫu",
                Description = "Xử lý mẫu bằng dung dịch khử trùng trước khi cấy.",
                StageId = S(StageNames.STERILIZATION),
                Status = Domain.Common.Enum.TaskStatus.Template,
                TaskAttributes =
                {
                    new() { ChemicalId = C(ChemicalNames.ETHANOL), Unit = Unit.CHEMICAL_UNIT, Value = 70 },
                    new() { ChemicalId = C(ChemicalNames.NAOCL), Unit = Unit.CHEMICAL_UNIT, Value = 1 },
                    new() { ChemicalId = C(ChemicalNames.HGCL2), Unit = Unit.CHEMICAL_UNIT, Value = 0.1m },
                    new() { ChemicalId = C(ChemicalNames.TWEEN20), Unit = Unit.CHEMICAL_UNIT, Value = 2 },
                    new() { ChemicalId = C(ChemicalNames.DISTILLED_WATER), Unit = Unit.CHEMICAL_UNIT, Value = 500 }
                },
                CreatedBy = "Hệ thống",

            },

            // =========================
            // INITIATION
            // =========================
            new Tasks
            {
                Name = "Chuẩn bị môi trường nuôi cấy khởi động",
                Description = "Pha môi trường dinh dưỡng và điều chỉnh pH cho nuôi cấy khởi động.",
                StageId = S(StageNames.INITIATION),
                Status = Domain.Common.Enum.TaskStatus.Template,
                TaskAttributes =
                {
                    new() { ChemicalId = C(ChemicalNames.BAP), Unit = Unit.CHEMICAL_UNIT, Value = 1 },
                    new() { ChemicalId = C(ChemicalNames.KINETIN), Unit = Unit.CHEMICAL_UNIT, Value = 0.5m },
                    new() { ChemicalId = C(ChemicalNames.D24), Unit = Unit.CHEMICAL_UNIT, Value = 0.2m }
                },
                CreatedBy = "Hệ thống",

            },

            new Tasks
            {
                Name = "Khử trùng môi trường nuôi cấy",
                Description = "Khử trùng môi trường bằng autoclave trước khi cấy.",
                StageId = S(StageNames.INITIATION),
                Status = Domain.Common.Enum.TaskStatus.Template,
                TaskAttributes =
                {
                    new() { MaterialId = M(MaterialNames.AUTOCLAVE), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.CULTURE_BOTTLE), Unit = Unit.MATERIAL_UNIT, Value = 10 }
                },
                CreatedBy = "Hệ thống",

            },

            new Tasks
            {
                Name = "Cấy mẫu khởi động",
                Description = "Cấy mẫu đã khử trùng vào môi trường khởi động trong điều kiện vô trùng.",
                StageId = S(StageNames.INITIATION),
                Status = Domain.Common.Enum.TaskStatus.Template,
                TaskAttributes =
                {
                    new() { MaterialId = M(MaterialNames.LAMINAR), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.FORCEPS), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.SCALPEL), Unit = Unit.MATERIAL_UNIT, Value = 1 }
                },
                CreatedBy = "Hệ thống",

            },

            // =========================
            // MULTIPLICATION
            // =========================
            new Tasks
            {
                Name = "Chuẩn bị môi trường nhân nhanh",
                Description = "Chuẩn bị môi trường dinh dưỡng cho giai đoạn nhân nhanh.",
                StageId = S(StageNames.MULTIPLICATION),
                Status = Domain.Common.Enum.TaskStatus.Template,
                TaskAttributes =
                {
                    new() { ChemicalId = C(ChemicalNames.BAP), Unit = Unit.CHEMICAL_UNIT, Value = 1 },
                    new() { ChemicalId = C(ChemicalNames.KINETIN), Unit = Unit.CHEMICAL_UNIT, Value = 0.5m }
                },
                CreatedBy = "Hệ thống",

            },

            new Tasks
            {
                Name = "Cấy chuyển nhân nhanh",
                Description = "Chia cắt chồi và cấy chuyển để tăng số lượng cây con.",
                StageId = S(StageNames.MULTIPLICATION),
                Status = Domain.Common.Enum.TaskStatus.Template,
                TaskAttributes =
                {
                    new() { MaterialId = M(MaterialNames.LAMINAR), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.CULTURE_BOTTLE), Unit = Unit.MATERIAL_UNIT, Value = 10 }
                },
                CreatedBy = "Hệ thống",

            },

            // =========================
            // ROOTING
            // =========================
            new Tasks
            {
                Name = "Chuẩn bị môi trường tạo rễ",
                Description = "Chuẩn bị môi trường kích thích hình thành rễ.",
                StageId = S(StageNames.ROOTING),
                Status = Domain.Common.Enum.TaskStatus.Template,
                TaskAttributes =
                {
                    new() { ChemicalId = C(ChemicalNames.NAA), Unit = Unit.CHEMICAL_UNIT, Value = 0.5m },
                    new() { ChemicalId = C(ChemicalNames.IBA), Unit = Unit.CHEMICAL_UNIT, Value = 0.5m }
                },
                CreatedBy = "Hệ thống",

            },

            new Tasks
            {
                Name = "Cấy chuyển tạo rễ",
                Description = "Chuyển chồi sang môi trường tạo rễ.",
                StageId = S(StageNames.ROOTING),
                Status = Domain.Common.Enum.TaskStatus.Template,
                TaskAttributes =
                {
                    new() { MaterialId = M(MaterialNames.LAMINAR), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.CULTURE_BOTTLE), Unit = Unit.MATERIAL_UNIT, Value = 10 }
                },
                CreatedBy = "Hệ thống",

            },

            // =========================
            // ACCLIMATIZATION
            // =========================
            new Tasks
            {
                Name = "Đưa cây ra giá thể",
                Description = "Chuyển cây con ra giá thể để thích nghi môi trường tự nhiên.",
                StageId = S(StageNames.ACCLIMATIZATION),
                Status = Domain.Common.Enum.TaskStatus.Template,
                TaskAttributes =
                {
                    new() { MaterialId = M(MaterialNames.SEED_TRAY), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.SUBSTRATE), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.SPRAYER), Unit = Unit.MATERIAL_UNIT, Value = 1 },
                    new() { MaterialId = M(MaterialNames.GLOVES), Unit = Unit.MATERIAL_UNIT, Value = 1 }
                },
                CreatedBy = "Hệ thống",

            }
        };

            await context.Set<Tasks>().AddRangeAsync(tasks);
            await context.SaveChangesAsync();

            // =========================
            // ATTACH CHECKLIST TO EACH TEMPLATE TASK
            // =========================

            // Attach checklist to each template task
            var allTasks = await context.Set<Tasks>()
                .Where(t => t.StageId != null)
                .ToListAsync();

            foreach (var t in allTasks)
            {
                var checklist = new TaskCheckList
                {
                    TaskId = t.ID
                };

                var items = new List<TaskCheckListItem>();

                // Stage-based default checklist definitions
                var stageName = stages.First(s => s.ID == t.StageId!).Name;
                if (stageName.StartsWith(StageNames.SAMPLING))
                {
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Chuẩn bị dụng cụ", IsRequired = true, Order = 1 });
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Cắt mẫu đúng vị trí", IsRequired = true, Order = 2 });
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Ghi chú kích thước mẫu", IsRequired = false, Order = 3, ExpectedUnit = "mm" });
                }
                else if (stageName.StartsWith(StageNames.STERILIZATION))
                {
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Rửa sơ bộ", IsRequired = true, Order = 1 });
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Ngâm EtOH 70%", IsRequired = true, Order = 2, ExpectedUnit = "%", ExpectedMinValue = 70, ExpectedMaxValue = 70 });
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Ngâm NaOCl", IsRequired = true, Order = 3, ExpectedUnit = "%", ExpectedMinValue = 0.5m, ExpectedMaxValue = 1.5m });
                }
                else if (stageName.StartsWith(StageNames.INITIATION))
                {
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Pha môi trường đúng công thức", IsRequired = true, Order = 1 });
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Điều chỉnh pH", IsRequired = true, Order = 2, ExpectedUnit = "pH", ExpectedMinValue = 5.6m, ExpectedMaxValue = 5.8m });
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Khử trùng autoclave", IsRequired = true, Order = 3 });
                }
                else if (stageName.StartsWith(StageNames.MULTIPLICATION))
                {
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Chia cắt chồi đúng kỹ thuật", IsRequired = true, Order = 1 });
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Số lượng chai đúng kế hoạch", IsRequired = false, Order = 2 });
                }
                else if (stageName.StartsWith(StageNames.ROOTING))
                {
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Chuẩn bị chất kích rễ", IsRequired = true, Order = 1 });
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Cấy chuyển sang môi trường tạo rễ", IsRequired = true, Order = 2 });
                }
                else if (stageName.StartsWith(StageNames.ACCLIMATIZATION))
                {
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Chuẩn bị giá thể", IsRequired = true, Order = 1 });
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Chuyển cây ra giá thể", IsRequired = true, Order = 2 });
                    items.Add(new TaskCheckListItem { TaskCheckListId = checklist.ID, Name = "Phun ẩm ban đầu", IsRequired = false, Order = 3 });
                }

                await context.Set<TaskCheckList>().AddAsync(checklist);
                if (items.Count > 0)
                {
                    await context.Set<TaskCheckListItem>().AddRangeAsync(items);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
