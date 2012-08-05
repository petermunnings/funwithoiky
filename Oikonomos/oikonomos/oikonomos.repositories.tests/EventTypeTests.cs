using System.Configuration;
using NUnit.Framework;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories.tests
{
    [TestFixture]
    public class EventTypeTests
    {
        [Test]
        public void CanGetCreatedItem()
        {
            var context                              = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString);
            IEventTypeRepository eventTypeRepository = new EventTypeRepository(context);
            const string testName                    = "Test Event Type";
            
            var newEventTypeDto = new EventTypeDto
                                      {
                                          ChurchId = 1,
                                          Name     = testName
                                      };
            
            var eventTypeId = eventTypeRepository.SaveItem(newEventTypeDto);
            var eventType   = eventTypeRepository.GetItem(eventTypeId);

            Assert.That(eventType.Name, Is.EqualTo(testName));

            eventTypeRepository.DeleteItem(eventTypeId);
        }

    }

}
