using System.Collections.Generic;
using System.Net.Mail;
using NUnit.Framework;
using Rhino.Mocks;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services.tests
{
    [TestFixture]
    public class EmailLoggerTests
    {
        [Test]
        public void GivenAnEmailMessage_WhenItIsSendSuccesfully_ThenTheMessageShouldBeLoggedAsSuccesful()
        {
            var messageRepository = MockRepository.GenerateMock<IMessageRepository>();
            var personRepository  = MockRepository.GenerateStub<IPersonRepository>();
            personRepository.Stub(p => p.FetchPersonIdFromEmailAddress("test@test.com")).Return(2);
            IEmailLogger emailLogger = new EmailLogger(messageRepository, personRepository);
            var mailMessage = new MailMessage("test@test.com", "test@test.com", "subject", "body");
            emailLogger.LogSuccess(mailMessage, 1);

            var fromIds = new List<int> {2};
            messageRepository.AssertWasCalled(m=>m.SaveMessage(1, fromIds, "subject", "body", "Email", "Success"));
        }
         
    }
}