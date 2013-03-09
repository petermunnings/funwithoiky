using System;
using System.Collections.Generic;
using System.Net.Mail;
using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IPersonRepository
    {
        Person UpdatePerson(PersonViewModel person, Person currentPerson, out bool sendWelcomeEmail, out Church church);
        bool SavePersonalDetails(PersonViewModel person, Person currentPerson, Person personToSave);
        Person FetchPerson(int personId, Person currentPerson);
        Person FetchPerson(int personId);
        IEnumerable<Person> GetPeopleWithEmailAddress(string emailAddress);
        Guid UpdatePublicId(Person person);
        void SaveWindowsLiveId(PersonViewModel person, Person personToSave);
        Person FetchPersonFromPublicId(string publicId);
        Person FetchPersonFromWindowsLiveId(string liveId);
        Person FetchPersonFromFacebookId(long facebookId);
        IEnumerable<Person> FetchPersonFromName(string fullname, string firstname, string surname, string email);
        IEnumerable<int> FetchPersonIdsFromEmailAddress(string emailAddress, int churchId);
        IEnumerable<int> FetchPersonIdsFromCellPhoneNos(IEnumerable<string> cellPhoneNos, int churchId);
    }
}