using FluentValidation;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.ExperimentLog.StateChangeExperimentLog
{
    public class StateChangeExperimentLogValidator : AbstractValidator<StateChangeExperimentLogCommand>
    {
        private readonly IExperimentLogRepository _experimentLogRepository;
        //private readonly IStageRepository _stageRepository;
        //private readonly IMethodRepository _methodRepository;
        public StateChangeExperimentLogValidator(
            //IStageRepository stageRepository, 
            //IMethodRepository methodRepository,
            IExperimentLogRepository experimentLogRepository)
        {
            //this._stageRepository = stageRepository;
            //this._methodRepository = methodRepository;
            this._experimentLogRepository = experimentLogRepository;
            Configuration();
        }
        async Task Configuration()
        {
            RuleFor(x => x.ELID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ExperimentLog ID can't null or empty");
            //RuleFor(x => x.stageID)
            //    .NotEmpty()
            //    .NotNull()
            //    .WithMessage("Stage ID can't null or empty");
            //RuleFor(x => x.methodID)
            //    .NotEmpty()
            //    .NotNull()
            //    .WithMessage("Method ID can't null or empty");
            RuleFor(x => x.ELID)
                .MustAsync(async (id, cancellationToken) => await IsExsitExperimentLog(id, cancellationToken))
                .WithMessage(x => $"Not found ExperimentLog ID : {x.ELID}");
            //RuleFor(x => x.stageID)
            //    .MustAsync(async (id, cancellationToken) => await IsExsitStage(id, cancellationToken))
            //    .WithMessage(x => $"Not found Stage ID : {x.stageID}");
            //RuleFor(x => x.ELID)
            //    .MustAsync(async (id, cancellationToken) => await IsExsitMethod(id, cancellationToken))
            //    .WithMessage(x => $"Not found Method ID : {x.methodID}");
        
        }

        async Task<bool> IsExsitExperimentLog(string id, CancellationToken cancellationToken)
            => await _experimentLogRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
        //async Task<bool> IsExsitStage(string id, CancellationToken cancellationToken)
        //    => await _stageRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
        //async Task<bool> IsExsitMethod(string id, CancellationToken cancellationToken)
        //    => await _methodRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
    }
}
