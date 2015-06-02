using NUnit.Framework;
using oikonomos.repositories.Messages;
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
            IEventRepository eventRepository = new EventRepository(new BirthdayAndAniversaryRepository());
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
            IEventService eventTypeService = new EventService(eventTypeRepository, emailService, new BirthdayAndAniversaryRepository());
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
            var eventDto = eventService.GetEvent(1);
            Assert.That(eventDto, Is.EqualTo(expectedEventDto));
        }

    }

}
