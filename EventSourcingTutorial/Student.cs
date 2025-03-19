using EventSourcingTutorial.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingTutorial
{
    public class Student
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public List<string> EnrolledCourses { get; set; } = new();
        public DateTime DateOfBirth { get; set; }

        private void Apply(StudentCreated created)
        {
            Id = created.StudentId;
            FullName = created.FullName;
            Email = created.Email;
            DateOfBirth = created.DateOfBirth;
        }

        private void Apply(StudentUpdated updated)
        {
            FullName = updated.FullName;
            Email = updated.Email;
        }

        private void Apply(StudentEnrolled enrolled)
        {
            EnrolledCourses.Add(enrolled.CourseName);
        }

        private void Apply(StudentUnEnrolled unEnrolled)
        {
            if (EnrolledCourses.Contains(unEnrolled.CourseName))
                EnrolledCourses.Remove(unEnrolled.CourseName);
        }

        public void Apply(Event @event)
        {
            switch (@event)
            {
                case StudentCreated created:
                    Apply(created);
                    break;
                case StudentEnrolled enrolled:
                    Apply(enrolled);
                    break;
                case StudentUnEnrolled unEnrolled:
                    Apply(unEnrolled);
                    break;
            }
        }
    }
}
