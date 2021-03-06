﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using oikonomos.common.Models;
using oikonomos.data.DataAccessors;
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
                var myJWT = new JsonWebToken(authenticationToken, d);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            
            var person = PersonDataAccessor.FetchPersonFromWindowsLiveId(id);
            if(person!=null)
            {
                var pvm = PersonDataAccessor.FetchPersonViewModel(person.PersonId, person);
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
            
            var person = PersonDataAccessor.FetchPersonFromWindowsLiveId(value.liveId);
            if (person != null)
            {
                var pvm       = PersonDataAccessor.FetchPersonViewModel(person.PersonId, person);
                pvm.Firstname = value.firstname;
                pvm.Surname   = value.surname;
                pvm.CellPhone = value.cellPhone;
                pvm.Email     = value.email;
                pvm.WindowsLiveId = value.liveId;

                PersonDataAccessor.SavePerson(pvm, person);
            }

            PersonDataAccessor.SavePersonToSampleChurch(value.firstname, value.surname, value.liveId, value.cellPhone, value.email, 47);

        }
    }
}