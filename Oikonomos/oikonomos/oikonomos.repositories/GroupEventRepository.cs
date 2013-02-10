using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.data.DataAccessors;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class GroupEventRepository : RepositoryBase, IGroupEventRepository
    {
        private readonly IPersonRepository _personRepository;

        public GroupEventRepository(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public void Save(Person currentPerson, HomeGroupEventViewModel hgEvent)
        {
            var didAttend = 0;
            var didNotAttend = 0;
            foreach (var personEvents in hgEvent.Events)
            {
                var person = _personRepository.FetchPerson(personEvents.PersonId);
                foreach (var personEvent in personEvents.Events)
                {
                    var pe = EventDataAccessor.SavePersonEvent(Context, personEvents, currentPerson, personEvent);
                    EventDataAccessor.CheckToSeeIfEventAlreadyExists(personEvents, Context, personEvent, pe);

                    if (!person.HasPermission(Permissions.IncludeInGroupAttendanceStats)) continue;
                    if (personEvent.Name == EventNames.DidNotAttendGroup)
                        didNotAttend++;
                    if (personEvent.Name == EventNames.AttendedGroup)
                        didAttend++;
                }
            }

            //Add the attended and did not attend group events
            EventDataAccessor.AddAttendanceEvents(hgEvent, Context, didAttend, "Members attended", currentPerson);
            EventDataAccessor.AddAttendanceEvents(hgEvent, Context, didNotAttend, "Members did not attend", currentPerson);

            Context.SaveChanges();
        }
    }
}