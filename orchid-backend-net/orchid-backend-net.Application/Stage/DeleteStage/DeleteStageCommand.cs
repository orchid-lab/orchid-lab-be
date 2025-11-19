using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;
using System.Text.Json.Serialization;

namespace orchid_backend_net.Application.Stage.DeleteStage
{
    public class DeleteStageCommand(string methodId) : IRequest, ICommand
    {
        public string MethodID { get; set; } = methodId;
    }

    internal class DeleteStageCommandHandler(IStageRepository stageRepository, IElementInStageRepository elementInStageRepository, 
        IReferentRepository referentRepository) : IRequestHandler<DeleteStageCommand>
    {
        public async Task Handle(DeleteStageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var stage = await stageRepository.FindAsync(x => x.MethodID.Equals(request.MethodID) && x.Status == true, cancellationToken);
                if (stage == null)
                    throw new NotFoundException($"Not found stage with MethodID :{request.MethodID}");
                stage.Status = false;
                stageRepository.Update(stage);

                //Deactivate all elements in this stage
                var elementInStages = await elementInStageRepository.FindAllAsync(x => x.StageID.Equals(stage.ID) && x.Status, cancellationToken);
                foreach (var elementInStage in elementInStages)
                {
                    elementInStage.Status = false;
                    elementInStageRepository.Update(elementInStage);
                }
                
                //Deactivate all referents in this stage
                var referents = await referentRepository.FindAllAsync(x => x.StageID.Equals(stage.ID) && x.Status, cancellationToken);
                foreach (var referent in referents)
                {
                    referent.Status = false;
                    referentRepository.Update(referent);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
