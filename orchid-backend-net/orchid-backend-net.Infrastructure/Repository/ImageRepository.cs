using AutoMapper;
using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class ImageRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<Imgs, Imgs, OrchidDbContext>(context, mapper), IImageRepository
    {
    }
}
