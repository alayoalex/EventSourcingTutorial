using EventSourcingTutorial;
using EventSourcingTutorial.Events;
using System.ComponentModel.DataAnnotations;

var studentDatabase = new StudentDatabase();

var studentId = Guid.Parse("d1b0b3f4-3b3b-4b3b-8b3b-3b3b3b3b3b3b");

var studentCreated = new StudentCreated
{
    StudentId = studentId,
    FullName = "Alexei Alayo Rondon",
    Email = "alexei.alayo@gmail.com",
    DateOfBirth = new DateTime(1990, 1, 1)
};

studentDatabase.Append(studentCreated);

var studentEnrolled = new StudentEnrolled
{
    StudentId = studentId,
    CourseName = "From Zero to Hero: Rest APIs in .NET"
};

studentDatabase.Append(studentEnrolled);

var studentUpdated = new StudentUpdated
{
    StudentId = studentId,
    Email = "alexei.alayo@gmail.com",
    FullName = "Nick Chapsas",
};

studentDatabase.Append(studentUpdated);

var student = studentDatabase.GetStudent(studentId);

var studentFromView = studentDatabase.GetStudentView(studentId);

Console.WriteLine();