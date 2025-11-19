using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using System.Text.Json.Serialization;

namespace orchid_backend_net.Application.Stage.CreateStage
{
    //fix this shit 

    public class CreateStageCommand(string name, string description, int dateOfProcessing, int step, List<string> elementInStages, List<CreateReferentInStage> referents) : IRequest, ICommand
    {
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public int DateOfProcessing { get; set; } = dateOfProcessing;
        public int Step { get; set; } = step;
        public List<string> ElementInStages { get; set; } = elementInStages;
        public List<CreateReferentInStage> Referents { get; set; } = referents;

        [JsonIgnore]
        public string? MethodID { get; set; }
    }

    public class CreateReferentInStage(string name, string unit, decimal valueFrom, decimal valueTo)
    {
        public string Name { get; set; } = name;
        public string Unit { get; set; } = unit;
        public decimal ValueFrom { get; set; } = valueFrom;
        public decimal ValueTo { get; set; } = valueTo;
    }

    internal class CreateStageCommandHandler(IStageRepository stageRepository, IElementInStageRepository elementInStageRepository, IReferentRepository referentRepository) : IRequestHandler<CreateStageCommand>
    {
        public async Task Handle(CreateStageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //this shit cannot be init in the constructor bro
                //and cannot be init in the validator
                //must be in the logic handler
                if(request.MethodID == null)
                {
                    throw new ArgumentNullException(nameof(request.MethodID), "MethodID must be set before creating a stage.");
                }
                //1. Create the stage entity
                Stages stage = new()
                {
                    MethodID = request.MethodID, // Assuming MethodID is set before calling this handler
                    Name = request.Name,
                    Description = request.Description,
                    DateOfProcessing = request.DateOfProcessing,
                    Step = request.Step,
                    Status = true
                };
                stageRepository.Add(stage);

                //2. Attach elements to the stage
                foreach (var element in request.ElementInStages)
                {
                    ElementInStage elementInStage = new()
                    {
                        StageID = stage.ID,
                        ElementID = element, // Assuming element is a string representing the ID of the element
                        Status = true
                    };
                    elementInStageRepository.Add(elementInStage);
                }

                //3. Create and attach referents to the stage
                foreach (var referent in request.Referents)
                {
                    Referents referentEntity = new()
                    {
                        StageID = stage.ID,
                        Name = referent.Name,
                        MeasurementUnit = referent.Unit,
                        ValueFrom = referent.ValueFrom,
                        ValueTo = referent.ValueTo,
                        Status = true
                    };
                    referentRepository.Add(referentEntity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating stage", ex);
            }
        }
    }
}
