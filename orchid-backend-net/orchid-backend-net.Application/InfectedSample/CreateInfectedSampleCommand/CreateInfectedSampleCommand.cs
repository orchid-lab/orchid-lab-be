using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.InfectedSample.CreateInfectedSampleCommand;

public class CreateInfectedSampleCommand : IRequest<bool>, ICommand
{
    public required string SampleID { get; set; }
    public required string DiseaseID { get; set; }
}

internal class CreateInfectedSampleCommandHandler(IInfectedSampleRepository infectedSampleRepository) : IRequestHandler<CreateInfectedSampleCommand, bool>
{
    public async Task<bool> Handle(CreateInfectedSampleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var infectedSample = new Domain.Entities.InfectedSamples()
            {
                SampleID = request.SampleID,
                DiseaseID = request.DiseaseID,
            };
            infectedSampleRepository.Add(infectedSample);
            return await infectedSampleRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }
}
