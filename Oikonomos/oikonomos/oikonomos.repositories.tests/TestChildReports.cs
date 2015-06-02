using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using NUnit.Framework;
using oikonomos.data;

namespace oikonomos.repositories.tests
{
    [TestFixture]
    public class TestChildReports : TestBase
    {
        [Test]
        public void GivenAPerson_WhenGettingListOfChildren_ShouldGetChildrenForThatChurch()
        {
            var childReportRepo = new ChildrenReportsRepository();
            var personRunningReport = new Person
            {
                ChurchId = 1,
                PersonId = 1,
                Permissions = new List<int>{4, 40}
            };
            var roles = new List<int> {1, 2, 3, 4, 5};
            var results = childReportRepo.GetListOfChildrenForAChurch(personRunningReport, roles);

        }
        
    }
}
