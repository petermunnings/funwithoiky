using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using oikonomos.data;
using oikonomos.data.DataAccessors;
using oikonomos.repositories;
using oikonomos.repositories.interfaces;
using oikonomos.services;
using oikonomos.services.interfaces;
using oikonomos.web.Helpers;

namespace oikonomos.web.ApiControllers
{
    public class Person
    {
        public string liveId    { get; set; }
        public string firstname { get; set; }
        public string surname   { get; set; }
        public string cellPhone { get; set; }
        public string email     { get; set; }
    }
    
    public class PersonController : ApiController
    {
        private readonly IPersonService _personService;
        private readonly IPersonRepository _personRepository;

        public PersonController()
        {
            var permissionRepository = new PermissionRepository();
            var churchRepository = new ChurchRepository();
            _personRepository = new PersonRepository(permissionRepository, churchRepository);
            _personService = new PersonService(
                _personRepository,
                new PersonGroupRepository(),
                permissionRepository,
                new PersonRoleRepository(),
                new PersonOptionalFieldRepository(),
                new RelationshipRepository(_personRepository),
                new ChurchMatcherRepository(), 
                new GroupRepository(), 
                new FamilyRepository(),
                new EmailService(new PasswordService(_personRepository, churchRepository, new UsernamePasswordRepository(permissionRepository)), new GroupRepository()),
                    new AddressRepository()
                );
        }

        private const string ClientSecret = "LWunfzibyFCzKqnFmnrEgUs6zHrglVZS";
        
        public Person Get(string id)
        {
            var authenticationToken = Request.Headers.GetValues("authenticationToken").FirstOrDefault();

            if (authenticationToken == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var d = new Dictionary<int, string> { { 0, ClientSecret } };
            try
            {
                var myJwt = new JsonWebToken(authenticationToken, d);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            
            var person = _personRepository.FetchPersonFromWindowsLiveId(id);
            if(person!=null)
            {
                var pvm = _personService.FetchPersonViewModel(person.PersonId, person);
                return new Person { liveId = id, firstname = pvm.Firstname, surname = pvm.Surname, cellPhone = pvm.CellPhone, email = pvm.Email };
            }

            return new Person { liveId = id, firstname = "", surname = "", cellPhone = "", email = "" };
            
        }

        public void Post([FromBody]Person value)
        {
            var authenticationToken = Request.Headers.GetValues("authenticationToken").FirstOrDefault();

            if (authenticationToken == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var d = new Dictionary<int, string> { { 0, ClientSecret } };
            try
            {
                var myJWT = new JsonWebToken(authenticationToken, d);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            
            var person = _personRepository.FetchPersonFromWindowsLiveId(value.liveId);
            if (person != null)
            {
                var pvm       = _personService.FetchPersonViewModel(person.PersonId, person);
                pvm.Firstname = value.firstname;
                pvm.Surname   = value.surname;
                pvm.CellPhone = value.cellPhone;
                pvm.Email     = value.email;
                pvm.WindowsLiveId = value.liveId;

                _personService.Save(pvm, person);
            }

            _personService.SavePersonToSampleChurch(value.firstname, value.surname, value.liveId, value.cellPhone, value.email, 47);

        }
    }
}