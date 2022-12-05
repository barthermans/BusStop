using System.Text;

namespace BusStop.Tests
{
    public class ApplicationsTests
    {
        private Application _application;

        [SetUp]
        public void Setup()
        {
            _application = new Application( null, null);
        }


        [Test]
        public void InvalidServiceRecords()
        {
            var input = new string[]
            {
                "Company 10:00 10:01", // invalid company
                "Posh 10:02 11:03",    // trip duration too long
                "Posh 10:05 10:04",    // arrival time < departure time
                "Posh 10:06 10:06",    // arrival time == departure time
                "Posh  10:07 10:08",   // extra space
                "Posh 10:0 10:10",     // departure time too short
                "Posh 10:11 10:120",   // arrival time too long
                "Posh abcde 10:14",    // invalid departure time
                "Posh 10:15 abcde"     // invalid arrival time
            };

            var output = _application.Process(input);
            var expectedOutput = "";

            Assert.That(output, Is.EqualTo(expectedOutput));
        }


        [Test]
        public void ExampleFromAssignment()
        {
            var input = new string[]
            {
                "Posh 10:15 11:10",
                "Posh 10:10 11:00",
                "Grotty 10:10 11:00",
                "Grotty 16:30 18:45",
                "Posh 12:05 12:30",
                "Grotty 12:30 13:25",
                "Grotty 12:45 13:25",
                "Posh 17:25 18:01"
            };

            var output = _application.Process(input);
            var expectedOutput = @"Posh 10:10 11:00
Posh 10:15 11:10
Posh 12:05 12:30
Posh 17:25 18:01

Grotty 12:45 13:25";

            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void MultipleInefficientServices()
        {
            var input = new string[]
            {
                "Posh 10:00 10:30",
                "Posh 10:00 10:15",
                "Posh 10:00 10:25",
                "Posh 10:00 10:10"
            };

            var output = _application.Process(input);
            var expectedOutput = @"Posh 10:00 10:10";

            Assert.That(output, Is.EqualTo(expectedOutput));
        }
    }
}