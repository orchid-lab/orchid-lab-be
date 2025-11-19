using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.DeleteSeedling
{
    public class DeleteSeedlingCommand : IRequest<string>
    {
        public string ID { get; set; }
        public DeleteSeedlingCommand(string id)
        {
            ID = id;
        }

        public DeleteSeedlingCommand() { }
    }

    internal class DeleteSeedlingCommandHandler(ISeedlingRepository seedlingRepository, ICurrentUserService currentUserService) : IRequestHandler<DeleteSeedlingCommand, string>
    {
        public async Task<string> Handle(DeleteSeedlingCommand request, CancellationToken cancellationToken)
        {
            var seedling = await seedlingRepository.FindAsync(x => x.ID.Equals(request.ID), cancellationToken);
            seedling.Delete_date = DateTime.UtcNow;
            seedling.Delete_by = currentUserService.UserName;
            seedlingRepository.Update(seedling);
            return await seedlingRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Deleted Seedling with ID: {seedling.ID}" : "Failed to deleted Seedling.";
        }
    }

}
