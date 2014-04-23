using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.repositories.interfaces.Messages;
using oikonomos.services.interfaces;

namespace oikonomos.services.tests
{
    [TestFixture]
    public class SmsSenderTests
    {
        [Test]
        public void GivenAnSmsLongerThan160Characters_WhenSendSmsesIsCalled_MultipleSmsesAreSent()
        {
            var messageRepository = MockRepository.GenerateStub<IMessageRepository>();
            var messageRecepientRepository = MockRepository.GenerateStub<IMessageRecepientRepository>();
            var personRepository = MockRepository.GenerateStub<IPersonRepository>();
            var httpPostService = MockRepository.GenerateStub<IHttpPostService>();
            httpPostService.Expect(h => h.HttpSend(string.Empty)).IgnoreArguments().Return("success").Repeat.Any();
            var sut = new SmsSender(messageRepository, messageRecepientRepository, personRepository, httpPostService);
            var currentPerson = new Person {Church = new Church {Country = "South Africa"}};

            var cellNos = new List<string> {"0826527871"};
            var longSms = "012345789012345789012345789012345789012345789012345789012345789012345789012345789012345789012345789012345789012345789012345789012345789012345789012345789012345789012345789012345789";
            sut.SendSmses(longSms, cellNos, "petermunnings", "sandton2000", currentPerson);
            httpPostService.AssertWasCalled(x => x.HttpSend(Arg<string>.Is.Anything), options => options.Repeat.Once());

        }
    }
}