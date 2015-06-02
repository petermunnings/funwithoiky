using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using oikonomos.repositories;
using oikonomos.repositories.Messages;
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

            var permissionRepository = new PermissionRepository();
            var personRepository = new PersonRepository(permissionRepository, new ChurchRepository());
            var usernamePasswordRepository = new UsernamePasswordRepository(permissionRepository);
            var groupRepository = new GroupRepository();
            var messageRepository = new MessageRepository();
            var messageRecepientRepository = new MessageRecepientRepository();
            var messageAttachmentRepository = new MessageAttachmentRepository();
            var emailSender = new EmailSender(messageRepository, messageRecepientRepository, messageAttachmentRepository, personRepository);
            var churchEmailTemplatesRepository = new ChurchEmailTemplatesRepository();
            var emailContentRepository = new EmailContentRepository();
            var emailContentService = new EmailContentService(emailContentRepository);
            var emailService = new EmailService(usernamePasswordRepository, personRepository, groupRepository, emailSender, emailContentService, churchEmailTemplatesRepository);
            IEventService eventService = new EventService(eventRepository, emailService, new BirthdayAndAniversaryRepository());

            var sut = eventService.GetPersonEventsForGroup(1, currentPerson);

            Assert.That(sut.Count(), Is.EqualTo(3));

        }
    }
}