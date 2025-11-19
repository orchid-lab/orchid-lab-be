using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.CreateSample
{
    public class CreateSampleCommand(string name, string? description) : IRequest<string>, ICommand
    {
        public string Name { get; set; } = name;
        public string? Description { get; set; } = description;
    }

    internal class CreateSampleCommandHandler(ISampleRepository sampleRepository) : IRequestHandler<CreateSampleCommand, string>
    {
        public async Task<string> Handle(CreateSampleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Samples createdSample = new()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Dob = DateOnly.FromDateTime(DateTime.UtcNow),
                    Status = 0
                };
                sampleRepository.Add(createdSample);
                return createdSample.ID;
            }
            catch (Exception ex) 
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
