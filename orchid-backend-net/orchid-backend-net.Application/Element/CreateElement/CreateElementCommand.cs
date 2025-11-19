using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Element.CreateElement
{
    public class CreateElementCommand : IRequest<string>, ICommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public CreateElementCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    internal class CreateElementCommandHandler(IElementRepositoty elementRepository) : IRequestHandler<CreateElementCommand, string>
    {
        public async Task<string> Handle(CreateElementCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Elements obj = new Elements()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Status = true,
                };
                elementRepository.Add(obj);
                return await elementRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Created element name : {request.Name}." : "Failed create element.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
