using EventSourcingTutorial.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingTutorial
{
    public class StudentDatabase
    {
        private readonly Dictionary<Guid, SortedList<DateTime, Event>> _studentEvents = new();
        private readonly Dictionary<Guid, Student> _students = new();

        public void Append(Event @event)
        {
            var stream = _studentEvents!.GetValueOrDefault(@event.StreamId, null);
            if (stream == null) 
            {
                _studentEvents[@event.StreamId] = new SortedList<DateTime, Event>();
            }
            @event.CreatedAtUtc = DateTime.UtcNow;
            _studentEvents[@event.StreamId].Add(@event.CreatedAtUtc, @event);

            _students[@event.StreamId] = GetStudent(@event.StreamId);
        }

        public Student? GetStudentView(Guid studentId)
        {
            return _students!.GetValueOrDefault(studentId, null);
        }

        public Student GetStudent(Guid studentId)
        {
            if (! _studentEvents.ContainsKey(studentId))
            {
                return null;
            }

            var student = new Student();
            var studentEvents = _studentEvents[studentId];
            foreach (var studentEvent in studentEvents)
            {
                student.Apply(studentEvent.Value);
            }
            return student;
        }
    }
}
