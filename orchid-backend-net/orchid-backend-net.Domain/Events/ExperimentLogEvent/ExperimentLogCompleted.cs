using orchid_backend_net.Domain.Events.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Domain.Events.ExperimentLogEvent
{
    public record ExperimentLogCompleted(string ExperimentLogId)
        : DomainEvent;
}
