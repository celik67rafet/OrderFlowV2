using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlowV2.Shared.Events
{
    public record OrderShippedEvent { public Guid OrderId { get; init; } }
}
