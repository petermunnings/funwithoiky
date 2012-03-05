using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
            Person currentPerson = CreateSysAdmin();
            int groupId = 1;
            DateTime dateTime = new DateTime(2011, 06, 01);
            List<AttendanceEventViewModel> attendanceList= EventDataAccessor.FetchGroupAttendance(currentPerson, groupId, dateTime);
            Assert.AreEqual(13, attendanceList.Count);
            int attended = 0;
            int didNotAttend = 0;
            foreach (AttendanceEventViewModel a in attendanceList)
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
            Person currentPerson = Person.CreatePerson(256, 1, "Peter", 52, DateTime.Now, DateTime.Now);
            currentPerson.PersonRoles.Add(PersonRole.CreatePersonRole(4, 256, DateTime.Now, DateTime.Now));
            return currentPerson;
        }
    }
}
