using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Seedling
{
    public class SeedlingDTO : IMapFrom<Seedlings>
    {
        public string ID { get; set; }
        public string LocalName { get; set; }
        public string ScientificName { get; set; }
        public string Description { get; set; }
        public string? Parent1 { get; set; }
        public string? Parent2 { get; set; }
        public DateOnly DoB { get; set; }
        public List<CharacteristicsDTO> Characteristics { get; set; } = [];
        public string? Create_by { get; set; }
        public DateTime? Create_date { get; set; }
        public string? Update_by { get; set; }
        public DateTime? Update_date { get; set; }
        public string? Delete_by { get; set; }
        public DateTime? Delete_date { get; set; }

        public static SeedlingDTO Create(string id, string name, string description, 
            string? mother, string? father, DateOnly doB, 
            string localName,List<CharacteristicsDTO> characteristics, string createBy, DateTime createDate, 
            string? updateBy, DateTime? updateDate = null, 
            string? deleteBy = null, DateTime? deleteDate = null)
        {
            return new SeedlingDTO
            {
                ID = id,
                ScientificName = name,
                LocalName = localName,
                Description = description,
                Parent2 = mother,
                Parent1 = father,
                DoB = doB,
                Characteristics = characteristics,
                Create_by = createBy,
                Create_date = createDate,
                Update_by = updateBy,
                Update_date = updateDate,
                Delete_by = deleteBy,
                Delete_date = deleteDate
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Seedlings, SeedlingDTO>()
                .ForMember(dest => dest.Characteristics, opt => opt.MapFrom(src => src.Characteristics))
                .ReverseMap();
        }
    }
}
