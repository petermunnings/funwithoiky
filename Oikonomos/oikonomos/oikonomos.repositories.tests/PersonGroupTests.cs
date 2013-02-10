﻿using NUnit.Framework;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories.tests
{
    [TestFixture]
    public class PersonGroupTests : TestBase
    {
        private Person _currentPerson;

        [SetUp]
        public void Setup()
        {
            _currentPerson = new Person {Firstname = "Tester", ChurchId = 1};
        }

        [Test]
        public void CanUpdatePrimaryGroup()
        {
            IPersonGroupRepository personGroupRepository = new PersonGroupRepository();
            personGroupRepository.SavePrimaryGroup(46,8, _currentPerson);
            var primaryGroup = personGroupRepository.GetPrimaryGroup(46, _currentPerson);
            Assert.That(primaryGroup.GroupId, Is.EqualTo(8));
        }

        [Test]
        public void IfGroupIdDoesntExistUpdatePrimaryGroupDoesNothing()
        {
            IPersonGroupRepository personGroupRepository = new PersonGroupRepository();
            personGroupRepository.SavePrimaryGroup(46, 1, _currentPerson);
            var primaryGroup = personGroupRepository.GetPrimaryGroup(46, _currentPerson);
            Assert.That(primaryGroup.GroupId, Is.EqualTo(8));
        }

        [Test]
        public void IfPrimaryGroupDoesntExistReturnsNull()
        {
            IPersonGroupRepository personGroupRepository = new PersonGroupRepository();
            var primaryGroup = personGroupRepository.GetPrimaryGroup(2, _currentPerson);
            Assert.That(primaryGroup, Is.Null);
        }
    }
}
