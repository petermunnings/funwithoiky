using NUnit.Framework;
using oikonomos.data.DataAccessors;

namespace oikonomos.repositories.tests
{
    [TestFixture]
    public class PersonDataAccessorTests
    {
        [Test]
        public void CanSavePersonToSampleChurch()
        {
            PersonDataAccessor.SavePersonToSampleChurch("test1", "test1", "liveId1", "", "", 47);
        }
         
    }
}