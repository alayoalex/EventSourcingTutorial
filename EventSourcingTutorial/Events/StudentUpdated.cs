using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingTutorial.Events
{
    public class StudentUpdated : Event
    {
        public required Guid StudentId { get; init; }
        public required string FullName { get; init; }
        public required string Email    { get; init; }
        public override Guid StreamId => StudentId;
    }
}
