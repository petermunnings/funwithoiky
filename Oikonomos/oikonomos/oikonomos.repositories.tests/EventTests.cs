using NUnit.Framework;
using Rhino.Mocks;
using oikonomos.common.DTOs;
using oikonomos.repositories.interfaces;
using oikonomos.services;
using oikonomos.services.interfaces;

namespace oikonomos.repositories.tests
{
    [TestFixture]
    public class EventTests : TestBase
    {
        [Test]
        public void CanGetCreatedItem()
        {
            IEventRepository eventRepository = new EventRepository(Context);
            const string testName            = "Test Event";
            
            var newEventDto = new EventDto
                                      {
                                          ChurchId = 1,
                                          Name = testName
                                      };
            
            var eventId     = eventRepository.SaveItem(newEventDto);
            var eventItem   = eventRepository.GetItem(eventId);

            Assert.That(eventItem.Name, Is.EqualTo(testName));

            eventRepository.DeleteItem(eventId);
        }

        [Test]
        public void CanCreateANewEvent()
        {
            var eventTypeRepository = MockRepository.GenerateStub<IEventRepository>();

            IEventService eventTypeService = new EventService(eventTypeRepository);
            var newEvent = new EventDto();
            eventTypeRepository
                .Expect(e => e.SaveItem(newEvent))
                .Return(1);
            var result = eventTypeService.CreateEvent(newEvent);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CanGetCreatedEvent()
        {
            var eventRepository = MockRepository.GenerateStub<IEventRepository>();
            var expectedEventDto = new EventDto
            {
                EventId = 1
            };
            eventRepository
                .Expect(et => et.GetItem(1))
                .Return(expectedEventDto);

            IEventService eventService = new EventService(eventRepository);
            var eventDto = eventService.GetEvent(1);
            Assert.That(eventDto, Is.EqualTo(expectedEventDto));
        }

    }

}
