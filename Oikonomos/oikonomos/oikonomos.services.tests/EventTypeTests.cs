using NUnit.Framework;
using Rhino.Mocks;
using oikonomos.common.DTOs;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services.tests
{
    [TestFixture]
    public class EventTypeTests
    {
        [Test]
        public void CanCreateANewEventType()
        {
            var eventTypeRepository = MockRepository.GenerateStub<IEventTypeRepository>();
            IEventTypeService eventTypeService = new EventTypeService(eventTypeRepository);
            var newEventType                   = new EventTypeDto();
            var result                         = eventTypeService.Save(newEventType);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CanGetCreatedEventType()
        {
            var eventTypeRepository = MockRepository.GenerateStub<IEventTypeRepository>();
            var eventTypeItem = new EventTypeDto
                                    {
                                        EventTypeId = 1
                                    };
            eventTypeRepository
                .Expect(et => et.GetItem(1))
                .Return(eventTypeItem);

            IEventTypeService eventTypeService = new EventTypeService(eventTypeRepository);
            var newEventType                   = new EventTypeDto();
            var eventTypeId                    = eventTypeService.Save(newEventType);
            var savedEvent                     = eventTypeService.GetItem(eventTypeId);
            Assert.That(savedEvent.EventTypeId, Is.EqualTo(1));
        }
    }
}
