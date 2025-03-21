﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingTutorial.Events
{
    public class StudentEnrolled : Event
    {
        public required Guid StudentId { get; init; }
        public required string CourseName { get; init; }
        public override Guid StreamId => StudentId;
    }
}
