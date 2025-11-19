using AutoMapper;

namespace orchid_backend_net.Application.Common.Mappings
{
    internal interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}
