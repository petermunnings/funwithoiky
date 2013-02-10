using System;
using System.Linq;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class PersonOptionalFieldRepository : RepositoryBase, IPersonOptionalFieldRepository
    {
        public void SaveContactInformation(PersonViewModel person, Person personToSave)
        {
            personToSave.Family.HomePhone = person.HomePhone;
            UpdatePersonOptionalField(personToSave, OptionalFields.CellPhone, person.CellPhone);
            UpdatePersonOptionalField(personToSave, OptionalFields.Skype, person.Skype);
            UpdatePersonOptionalField(personToSave, OptionalFields.Twitter, person.Twitter);
            UpdatePersonOptionalField(personToSave, OptionalFields.WorkPhone, person.WorkPhone);
            UpdatePersonOptionalField(personToSave, OptionalFields.HeardAbout, person.HeardAbout);
            UpdatePersonOptionalField(personToSave, OptionalFields.Gender, person.Gender);
            Context.SaveChanges();
        }

        private void UpdatePersonOptionalField(Person person, OptionalFields optionalField, string value)
        {
            var personOptionalField = person.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)optionalField);
            if (personOptionalField == null)
            {
                personOptionalField = new PersonOptionalField
                {
                    OptionalFieldId = (int)optionalField,
                    Created = DateTime.Now,
                    Changed = DateTime.Now
                };
                person.PersonOptionalFields.Add(personOptionalField);
            }
            if (personOptionalField.Value != (value ?? string.Empty))
                personOptionalField.Changed = DateTime.Now;
            personOptionalField.Value = value ?? string.Empty;

        }
    }
}