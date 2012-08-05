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
            eventTypeRepository
                .Expect(e => e.SaveItem(newEventType))
                .Return(1);
            var result                         = eventTypeService.CreateEventType(newEventType);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CanGetCreatedEventType()
        {
            var eventTypeRepository = MockRepository.GenerateStub<IEventTypeRepository>();
            var expectedEventTypeDto = new EventTypeDto
                                    {
                                        EventTypeId = 1
                                    };
            eventTypeRepository
                .Expect(et => et.GetItem(1))
                .Return(expectedEventTypeDto);

            IEventTypeService eventTypeService = new EventTypeService(eventTypeRepository);
            var eventTypeDto                   = eventTypeService.GetEventType(1);
            Assert.That(eventTypeDto, Is.EqualTo(expectedEventTypeDto));
        }
    }
}
