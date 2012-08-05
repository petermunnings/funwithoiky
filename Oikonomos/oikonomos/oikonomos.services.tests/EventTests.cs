using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using oikonomos.common.DTOs;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services.tests
{
    [TestFixture]
    public class EventTests
    {
        [Test]
        public void CanGetListOfEventsThatHaveBeenAttended()
        {
            var eventRepository = MockRepository.GenerateStub<IEventRepository>();
            var eventList = new List<EventDto> {new EventDto(), new EventDto(), new EventDto()};

            eventRepository
                .Expect(e => e.GetListOfCompletedEvents(1))
                .Return(eventList);

            IEventService eventService = new EventService(eventRepository);

            var sut = eventService.GetListOfCompletedEvents(1);

            Assert.That(sut.Count(), Is.EqualTo(3));

        }
    }
}