using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.SqlClient;
using NUnit.Framework;

namespace oikonomos.repositories.tests
{
    [TestFixture]
    public class MessageRepositoryTests : TestBase
    {
        [SetUp]
        public void SetupDatabase()
        {
            using (var con = new SqlConnection(_sqlConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("DELETE FROM MessageRecepient",con))
                {
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "DELETE FROM Message";
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        [Test]
        public void GivenAMessage_WhenSaveMessage_ThenMessageShouldBeSaved()
        {
            var messageRepository = new MessageRepository();
            messageRepository.SaveMessage(1, new List<int>{1,3}, "test subject", "test body", "Email", "Success");
            Assert.That(DatabaseIsInExpectedState());
        }

        private bool DatabaseIsInExpectedState()
        {
            int messageRecepients;
            int messages;

            using (var con = new SqlConnection(_sqlConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM MessageRecepient Where STATUS = 'Success'", con))
                {
                    messageRecepients = (int) cmd.ExecuteScalar();
                    cmd.CommandText = "SELECT COUNT(*) FROM Message";
                    messages = (int)cmd.ExecuteScalar();
                }
                con.Close();
            }

            return messageRecepients == 2 && messages == 1;
        }
    }
}