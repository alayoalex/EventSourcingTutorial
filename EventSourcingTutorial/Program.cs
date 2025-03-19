using EventSourcingTutorial;
using EventSourcingTutorial.Events;
using System.ComponentModel.DataAnnotations;

var studentDatabase = new StudentDatabase();

var studentId = Guid.Parse("d1b0b3f4-3b3b-4b3b-8b3b-3b3b3b3b3b3b");

var studentCreated = new StudentCreated
{
    StudentId = studentId,
    FullName = "Yunior Alayo Rondon",
    Email = "yunior.alayo@gmail.com",
    DateOfBirth = new DateTime(1986, 1, 1)
};

await studentDatabase.AppendAsync(studentCreated);

var studentEnrolled = new StudentEnrolled
{
    StudentId = studentId,
    CourseName = "From Zero to Hero: Rest APIs in .NET"
};

await studentDatabase.AppendAsync(studentEnrolled);

var studentUpdated = new StudentUpdated
{
    StudentId = studentId,
    Email = "yuninho2005@gmail.com",
    FullName = "Yuninho Pernambucano",
};

await studentDatabase.AppendAsync(studentUpdated);

var student = await studentDatabase.GetStudentAsync(studentId);

//var studentFromView = studentDatabase.GetStudentView(studentId);

Console.WriteLine();