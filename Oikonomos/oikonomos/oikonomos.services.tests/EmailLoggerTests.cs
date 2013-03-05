using System;
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
        private IEmailLogger _emailLogger;
        private IMessageRepository _messageRepository;
        private MailMessage _mailMessage;

        [SetUp]
        public void GivenAnEmailMessage()
        {
            _messageRepository = MockRepository.GenerateMock<IMessageRepository>();
            var personRepository = MockRepository.GenerateStub<IPersonRepository>();
            personRepository.Stub(p => p.FetchPersonIdsFromEmailAddress("test@test.com", 1)).Return(new[]{2});
            _emailLogger = new EmailLogger(_messageRepository, personRepository);
            _mailMessage = new MailMessage("test@test.com", "test@test.com", "subject", "body");
        }

    [Test]
        public void WhenItIsSendSuccesfully_ThenTheMessageShouldBeLoggedAsSuccesful()
        {
            _emailLogger.LogSuccess(_mailMessage, "body", 1, 1);

            var fromIds = new List<int> {2};
            _messageRepository.AssertWasCalled(m=>m.SaveMessage(1, fromIds, "subject", "body", "Email", "Success"));
        }

        [Test]
        public void WhenItIsFails_ThenTheMessageShouldBeLoggedAsFailed()
        {
            var applicationException = new ApplicationException("Test exception");
            _emailLogger.LogError(_mailMessage, "body", 1, applicationException, 1);

            var fromIds = new List<int> { 2 };
            _messageRepository.AssertWasCalled(m => m.SaveMessage(1, fromIds, "subject", "body", "Email", "Failure", "Test exception"));
        }
         
    }
}