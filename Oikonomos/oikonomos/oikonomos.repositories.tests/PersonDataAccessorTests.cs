using System.Linq;
using NUnit.Framework;
using oikonomos.services;

namespace oikonomos.repositories.tests
{
    [TestFixture]
    public class PersonDataAccessorTests : TestBase
    {
        [Test]
        public void CanSavePersonToSampleChurch()
        {
            var noPeopleInSampleChurch = Context.PersonChurches.Count(pc => pc.ChurchId == 6);
            var permissionRepository = new PermissionRepository();
            var churchRepository = new ChurchRepository();
            var personRepository = new PersonRepository(permissionRepository, churchRepository);
            var personService = new PersonService(
                personRepository,
                new PersonGroupRepository(personRepository),
                permissionRepository,
                new PersonRoleRepository(),
                new PersonOptionalFieldRepository(),
                new RelationshipRepository(personRepository),
                new ChurchMatcherRepository(),
                new GroupRepository(),
                new FamilyRepository(),
                new EmailService(new PasswordService(personRepository, churchRepository, new UsernamePasswordRepository(permissionRepository)), new GroupRepository()),
                new AddressRepository()
                );
            personService.SavePersonToSampleChurch("test1", "test1", "liveId1", "", "", 47);

            var updatedNoPeopleInSampleChurch = Context.PersonChurches.Count(pc => pc.ChurchId == 6);
            Assert.That(updatedNoPeopleInSampleChurch, Is.EqualTo(noPeopleInSampleChurch + 1));

        }
    }
}