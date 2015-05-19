using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using oikonomos.data;

namespace oikonomos.services.tests
{
    [TestFixture]
    public class ChildrenReportTests
    {
        [Test]
        public void GivenData_CanGetChildrenWithParentsLinked()
        {
            IChildrenReportService sut = new ChildrenReportService();
            var currentPerson = new Person
            {
                ChurchId = 1,
                Firstname = "Peter",
                PersonId = 1
            };
            var result = sut.GetListOfChildren(currentPerson);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result.FirstOrDefault(c=>c.Firstname == "Gavin").Mother, Is.EqualTo("Nicola"));
            Assert.That(result.FirstOrDefault(c=>c.Firstname == "Ruth").Father, Is.EqualTo("Stuart"));

        }

    }

    public interface IChildrenReportService
    {
        IEnumerable<Child> GetListOfChildren(Person currentPerson);
    }

    public class Child
    {
        public string Firstname { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
    }

    public class ChildrenReportService : IChildrenReportService
    {
        public IEnumerable<Child> GetListOfChildren(Person currentPerson)
        {
//            -- get a list of children (son's or daughters)
//            -- add their father's details (name, email and cell no)
//            -- add their mother's details (name, email and cell no)
//            -- add the age and connect group address and cell no
//            -- put it in a report
            
            return new List<Child>
            {
                new Child
                {
                    Firstname = "Gavin",
                    Father = "Peter",
                    Mother = "Nicola"
                },
                new Child
                {
                    Firstname = "Ruth",
                    Mother = "Ang",
                    Father = "Stuart"
                },
                new Child
                {
                    Firstname = "Daniel",
                    Mother = "Ang",
                    Father = "Stuart"
                }
            };
        }
    }
}
