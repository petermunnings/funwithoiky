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
        public void ShouldGetTheRightNumberOfComments()
        {
            var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString);
            ICommentRepository commentsRepo = new CommentRepository(context);
            var currentPerson = new Person {PersonId = 256};
            var personRole1 = new PersonRole {PersonId = 256, RoleId = 1};
            var personRole2 = new PersonRole { PersonId = 256, RoleId = 2 };

            currentPerson.PersonRoles.Add(personRole1);
            currentPerson.PersonRoles.Add(personRole2);

            var sut = commentsRepo.GetListOfComments(currentPerson, 256);

            Assert.That(sut.Count(), Is.EqualTo(1));

        }
         
    }
}