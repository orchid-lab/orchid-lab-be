using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Stage.CreateStage;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace orchid_backend_net.Application.Stage.UpdateStage
{
    public class UpdateStageCommand(string? name, string? description, int? dateOfProcessing,
        int? step, List<string>? elementInStages, List<UpdateReferentInStage>? referents) : IRequest, ICommand
    {
        public string? Name { get; set; } = name;
        public string? Description { get; set; } = description;
        public int? DateOfProcessing { get; set; } = dateOfProcessing;
        public int? Step { get; set; } = step;
        public List<string>? ElementInStages { get; set; } = elementInStages;
        public List<UpdateReferentInStage>? Referents { get; set; } = referents;

        [JsonIgnore]
        public string? MethodID { get; set; }
    }

    public class UpdateReferentInStage(string? name, string? unit, decimal? valueFrom, decimal? valueTo)
    {
        public string? Name { get; set; } = name;
        public string? Unit { get; set; } = unit;
        public decimal? ValueFrom { get; set; } = valueFrom;
        public decimal? ValueTo { get; set; } = valueTo;
    }

    internal class UpdateStageCommandHandler(IStageRepository stageRepository, IElementInStageRepository elementInStageRepository,
        IReferentRepository referentRepository, IElementRepositoty elementRepositoty) : IRequestHandler<UpdateStageCommand>
    {
        public async Task Handle(UpdateStageCommand request, CancellationToken cancellationToken)
        {
            //0.Check method is set before updating a stage
            if (request.MethodID == null)
            {
                throw new ArgumentNullException(nameof(request.MethodID), "MethodID must be set before updating a stage.");
            }
            //1. Find the existing stage
            var stage = await stageRepository.FindAsync(x => x.MethodID.Equals(request.MethodID) && x.Status == true, cancellationToken) 
                ?? throw new ArgumentException($"Stage with MethodID {request.MethodID} does not exist.");
            //2. Update the stage properties
            stage.Name = request.Name ?? stage.Name;
            stage.Description = request.Description ?? stage.Description;
            stage.DateOfProcessing = request.DateOfProcessing ?? stage.DateOfProcessing;
            stage.Step = request.Step ?? stage.Step;
            //3. Update elements in the stage
            //If the element is exist but not contain in the stage, add it
            if (request.ElementInStages != null && request.ElementInStages.Count > 0)
            {
                foreach (var element in request.ElementInStages)
                {
                    var elementCheck = await elementRepositoty.AnyAsync(x => x.ID.Equals(element), cancellationToken);
                    var existingElement = await elementInStageRepository.FindAsync(x => x.StageID.Equals(stage.ID) && x.ElementID.Equals(element), cancellationToken);
                    if (!elementCheck)
                    {
                        throw new ArgumentException("Element does not exist");
                    }
                    if (existingElement is null)
                    {
                        ElementInStage elementInStage = new()
                        {
                            StageID = stage.ID,
                            ElementID = element, // Assuming element is a string representing the ID of the element
                            Status = true
                        };
                        elementInStageRepository.Add(elementInStage);
                    }
                }
            }
            //4. Update referents in the stage
            if (request.Referents != null && request.Referents.Count > 0)
            {
                foreach (var referent in request.Referents)
                {
                    // Check if the referent already exists in the stage
                    var existingReferent = await referentRepository.FindAsync(x => x.Name.Equals(referent.Name) && x.StageID.Equals(stage.ID), cancellationToken);
                    if (existingReferent is null)
                    {
                        if (string.IsNullOrEmpty(referent.Name) || referent.Unit == null || referent.ValueFrom == null || referent.ValueTo == null)
                        {
                            throw new ArgumentException("Referent properties cannot be null or empty.");
                        }
                        // If not, create a new referent
                        Referents newReferent = new()
                        {
                            Name = referent.Name,
                            MeasurementUnit = (string)referent.Unit,
                            ValueFrom = (decimal)referent.ValueFrom,
                            ValueTo = (decimal)referent.ValueTo,
                            StageID = stage.ID
                        };
                        referentRepository.Add(newReferent);
                    }
                    else
                    {
                        // If it exists, update its properties
                        existingReferent.Name = referent.Name ?? existingReferent.Name;
                        existingReferent.MeasurementUnit = referent.Unit ?? existingReferent.MeasurementUnit;
                        existingReferent.ValueFrom = referent.ValueFrom ?? existingReferent.ValueFrom;
                        existingReferent.ValueTo = referent.ValueTo ?? existingReferent.ValueTo;
                    }
                }
            }
        }
    }
}
