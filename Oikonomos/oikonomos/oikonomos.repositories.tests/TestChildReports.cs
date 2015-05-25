using System.Collections.Generic;
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
            var results = childReportRepo.GetListOfChildrenForAChurch(personRunningReport);

        }
        
    }
}
