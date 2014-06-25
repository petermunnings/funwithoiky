using System.Linq;
using NUnit.Framework;
using oikonomos.repositories.Messages;
using oikonomos.services;

namespace oikonomos.repositories.tests
{
    [TestFixture]
    public class PersonDataAccessorTests : TestBase
    {
        [Test]
        public void CanSavePersonToSampleChurch()
        {
            var noPeopleInSampleChurch = _context.PersonChurches.Count(pc => pc.ChurchId == 6);
            var permissionRepository = new PermissionRepository();
            var churchRepository = new ChurchRepository();
            var personRepository = new PersonRepository(permissionRepository, churchRepository);
            var emailSender = new EmailSender(new MessageRepository(), new MessageRecepientRepository(), new MessageAttachmentRepository(), personRepository);
            var emailService = new EmailService(new UsernamePasswordRepository(permissionRepository), personRepository, new GroupRepository(), emailSender, new EmailContentService(new EmailContentRepository()));
            var uploadPhotoRepository = new PhotoRepository();
            var personService = new PersonService(
                personRepository,
                new PersonGroupRepository(personRepository),
                permissionRepository,
                new PersonRoleRepository(),
                new PersonOptionalFieldRepository(),
                new RelationshipRepository(personRepository),
                new ChurchMatcherRepository(),
                new GroupRepository(),
                new FamilyRepository(uploadPhotoRepository),
                emailService,
                new AddressRepository(),
                uploadPhotoRepository
                );
            personService.SavePersonToSampleChurch("test1", "test1", "liveId1", "", "", 47);

            var updatedNoPeopleInSampleChurch = _context.PersonChurches.Count(pc => pc.ChurchId == 6);
            Assert.That(updatedNoPeopleInSampleChurch, Is.EqualTo(noPeopleInSampleChurch + 1));

        }
    }
}