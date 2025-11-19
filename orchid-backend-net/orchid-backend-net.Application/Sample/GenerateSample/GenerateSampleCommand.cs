using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Linkeds.CreateLinkedsCommand;
using orchid_backend_net.Application.Sample.CreateSample;

namespace orchid_backend_net.Application.Sample.GenerateSample
{
    public class GenerateSampleCommand(string experimentLogName, string experimentLogId, string stageId, int numberOfSample) : IRequest<Unit>, ICommand
    {
        public string Name { get; set; } = experimentLogName;
        public string ExperimentLogID { get; set; } = experimentLogId;
        public string StageID { get; set; } = stageId;
        public int NumberOfSample { get; set; } = numberOfSample;
    }

    internal class GenerateSampleCommandHandler(ISender sender) : IRequestHandler<GenerateSampleCommand, Unit>
    {
        public async Task<Unit> Handle(GenerateSampleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                for (int i = 0; i < request.NumberOfSample; i++)
                {
                    var sampleId = await sender.Send(
                        new CreateSampleCommand($"Mẫu thí nghiệm số {i + 1} của {request.Name}", ""),
                        cancellationToken);

                    await sender.Send(new CreateLinkedsCommand()
                    {
                        ExperimentLogID = request.ExperimentLogID,
                        StageID = request.StageID,
                        SampleID = sampleId
                    }, cancellationToken);
                }
                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
