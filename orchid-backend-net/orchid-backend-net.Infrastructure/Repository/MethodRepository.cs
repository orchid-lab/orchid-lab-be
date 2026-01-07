using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class MethodRepository(OrchidDbContext dbContext, IMapper mapper) : RepositoryBase<Methods, Methods, OrchidDbContext>(dbContext, mapper), IMethodRepository
    {
    }
}
