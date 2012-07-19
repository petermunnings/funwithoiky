using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using oikonomos.data.DataAccessors;
using oikonomos.data;
using oikonomos.common.Models;

namespace oikonomos.test
{
    [TestClass]
    public class EventTest
    {
        [TestMethod]
        public void FetchGroupAttendance()
        {
            var currentPerson  = CreateSysAdmin();
            const int groupId  = 1;
            var dateTime       = new DateTime(2011, 06, 01);
            var attendanceList = EventDataAccessor.FetchGroupAttendance(currentPerson, groupId, dateTime);
            
            Assert.AreEqual(13, attendanceList.Count);
            var attended       = 0;
            var didNotAttend   = 0;
            foreach (var a in attendanceList)
            {
                if (a.Attended)
                    attended++;
                else
                    didNotAttend++;
            }
            Assert.AreEqual(6, attended);
            Assert.AreEqual(7, didNotAttend);
        }

        private static Person CreateSysAdmin()
        {
            var currentPerson = Person.CreatePerson(256, "Peter", 52, DateTime.Now, DateTime.Now);
            currentPerson.PersonRoles.Add(PersonRole.CreatePersonRole(4, 256, DateTime.Now, DateTime.Now));
            return currentPerson;
        }
    }
}
