using System.Configuration;
using System.Linq;
using NUnit.Framework;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories.tests
{
    [TestFixture]
    public class CommentTests
    {
        [Test]
        [Explicit("Just used to test db - don't know how to Mock out EF")]
        public void ShouldGetTheRightNumberOfComments()
        {
            var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString);
            ICommentRepository commentsRepo = new CommentRepository(context);
            var currentPerson = new Person {PersonId = 256, RoleId = 1};

            var sut = commentsRepo.GetListOfComments(currentPerson, 256);

            Assert.That(sut.Count(), Is.EqualTo(1));

        }
         
    }
}