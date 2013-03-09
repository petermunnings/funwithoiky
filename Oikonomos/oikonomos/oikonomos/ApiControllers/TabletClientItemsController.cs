using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using oikonomos.common.Models;
using oikonomos.data.DataAccessors;
using oikonomos.repositories;
using oikonomos.repositories.interfaces;
using oikonomos.services;
using oikonomos.services.interfaces;
using oikonomos.web.Helpers;
using oikonomos.web.Models.Api;

namespace oikonomos.web.ApiControllers
{
    public class TabletClientItemsController : ApiController
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonService _personService;
        private const string ClientSecret = "LWunfzibyFCzKqnFmnrEgUs6zHrglVZS";

        public TabletClientItemsController()
        {
            var permissionRepository = new PermissionRepository();
            var churchRepository = new ChurchRepository();
            _personRepository = new PersonRepository(permissionRepository, churchRepository);
            var personGroupRepository = new PersonGroupRepository(_personRepository);
            var relationshipRepository = new RelationshipRepository(_personRepository);
            var usernamePasswordRepository = new UsernamePasswordRepository(permissionRepository);
            var emailSender = new EmailSender(new MessageRepository(), _personRepository);
            var groupRepository = new GroupRepository();
            var emailContentService = new EmailContentService(new EmailContentRepository());
            var emailService = new EmailService(
                usernamePasswordRepository,
                _personRepository,
                groupRepository,
                emailSender,
                emailContentService
                );

            _personService = new PersonService(_personRepository, 
                personGroupRepository, 
                permissionRepository, 
                new PersonRoleRepository(), 
                new PersonOptionalFieldRepository(), 
                relationshipRepository, 
                new ChurchMatcherRepository(), 
                new GroupRepository(), 
                new FamilyRepository(), 
                emailService,
                new AddressRepository());
        }

        public IEnumerable<Item> Get()
        {
            var authenticationToken = Request.Headers.GetValues("authenticationToken").FirstOrDefault();

            if (authenticationToken == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var d = new Dictionary<int, string> {{0, ClientSecret}};
            try
            {
                var myJWT = new JsonWebToken(authenticationToken, d);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var group1 = new Group { key = "1people", title = "People", subtitle = "Contact information for people in my church", description = "Click here to access contact information", backgroundImage = "images/people.png" };
            var group2 = new Group { key = "2events", title = "Events", subtitle = "Upcoming events in my church", description = "Click here to see a list of upcoming events", backgroundImage = "images/Calendar.png" };
            var group3 = new Group { key = "3myDetails", title = "My Details", subtitle = "Update my own details", description = "Click here update personal details", backgroundImage = "images/MyDetails.png" };

            var liveId = Request.Headers.GetValues("liveId").FirstOrDefault();
            var currentPerson = _personRepository.FetchPersonFromWindowsLiveId(liveId);
            if(currentPerson==null)
            {
                return new[] {
                    new Item { group = group1, title = "", subtitle = "Visitor", description = "", content = "", backgroundImage = "images/unknown.jpg" },
                    new Item { group = group3, title = "", subtitle = "Visitor", description = "", content = "", backgroundImage = "images/unknown.jpg" }
                };
            }

            var items = new List<Item>();

            if(currentPerson.ChurchId==6) //Sample church people
            {
                var currentPersonVm = _personService.FetchPersonViewModel(currentPerson.PersonId, currentPerson);
                items.Add(GetSamplePerson("Joe Bloggs", group1));
                items.Add(GetSamplePerson("John Doe", group1));
                CreatePersonItem(currentPersonVm, items, group1);
                items.Add(GetSampleEvent("Great Event", "You really should be there", "event1.jpg", "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", group2));
                items.Add(GetSampleEvent("Launch of Windows 8", "26th October 2012", "event2.jpg", "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?", group2));
                items.Add(GetSampleEvent("Another Great Event", "Make sure you do not miss out", "event3.jpg", "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.", group2));
                CreatePersonItem(currentPersonVm, items, group3, "images/MyDetails.png");

                return items;
            }

            var people = PersonDataAccessor.FetchChurchListForTablet(currentPerson);
            
            foreach(var person in people)
            {
                CreatePersonItem(person, items, group1);
            }

            items.Add(GetSampleEvent("Great Event", "You really should be there", "event1.jpg", "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", group2));
            items.Add(GetSampleEvent("Launch of Windows 8", "26th October 2012", "event2.jpg", "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?", group2));
            items.Add(GetSampleEvent("Another Great Event", "Make sure you do not miss out", "event3.jpg", "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.", group2));
                
            var currentPersonViewModel = _personService.FetchPersonViewModel(currentPerson.PersonId, currentPerson);
            CreatePersonItem(currentPersonViewModel, items, group3);
            return items;
        }

        private Item GetSamplePerson(string title, Group group)
        {
            return new Item
                       {
                           backgroundImage = "images/unknown.jpg",
                           content         = CreateContent("(000) 5555555", "sample@sample.com"),
                           title           = title,
                           subtitle        = "Member",
                           description     = "",
                           group           = group
                       };
        }

        private Item GetSampleEvent(string eventTitle, string eventSubTitle, string imageUrl, string content, Group group)
        {
            return new Item
                       {
                           backgroundImage = string.Format("https://www.oikonomos.co.za/Content/images/sampleEvents/{0}", imageUrl),
                           title           = eventTitle,
                           subtitle        = eventSubTitle,
                           description     = "",
                           content         = content,
                           group           = group
                       };
        }

        private string CreateContent(string cellPhone, string email)
        {
            return string.Format("<table><tr><td>Cell</td><td>{0}</td></tr><tr><td>Email</td><td><a href='mailto:{1}'>{2}</a></td></tr></table>", cellPhone, email, email);
        }

        private void CreatePersonItem(PersonViewModel person, ICollection<Item> items, Group group, string backgroundImage)
        {
            items.Add(new Item
            {
                backgroundImage = backgroundImage,
                content         = CreateContent(person.CellPhone, person.Email),
                title           = person.FullName,
                subtitle        = person.RoleName,
                description     = "",
                group           = group
            });
        }

        private void CreatePersonItem(PersonViewModel person, ICollection<Item> items, Group group )
        {
            var backgroundImage = "images/unknown.jpg";
            if (!string.IsNullOrEmpty(person.FacebookId))
            {
                backgroundImage = string.Format("https://graph.facebook.com/{0}/picture", person.FacebookId);
            }

            CreatePersonItem(person, items, group, backgroundImage);

        }
    }
}