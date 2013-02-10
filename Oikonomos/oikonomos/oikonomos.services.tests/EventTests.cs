using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services.tests
{
    [TestFixture]
    public class EventTests
    {
        [Test]
        public void CanGetListOfPersonEventsForAGroup()
        {
            var eventRepository = MockRepository.GenerateStub<IEventRepository>();
            var eventList = new List<PersonEventDto> { new PersonEventDto(), new PersonEventDto(), new PersonEventDto() };
            var currentPerson = new Person {Permissions = new List<int> {57}};
            eventRepository
                .Expect(e => e.GetPersonEventsForGroup(1, currentPerson))
                .Return(eventList);

            IEventService eventService = new EventService(eventRepository);

            var sut = eventService.GetPersonEventsForGroup(1, currentPerson);

            Assert.That(sut.Count(), Is.EqualTo(3));

        }
    }
}