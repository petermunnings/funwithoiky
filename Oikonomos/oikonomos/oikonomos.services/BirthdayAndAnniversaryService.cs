using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using oikonomos.common.Models;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class BirthdayAndAnniversaryService : IBirthdayAndAnniversaryService
    {
        private readonly IBirthdayAndAnniversaryRepository _birthdayAndAnniversaryRepository;

        public BirthdayAndAnniversaryService(IBirthdayAndAnniversaryRepository birthdayAndAniversaryRepository)
        {
            _birthdayAndAnniversaryRepository = birthdayAndAniversaryRepository;
        }

        public Stream GetBirthdayListForAMonth(string selectedRoles, int selectedMonth, int churchId)
        {
            var list = _birthdayAndAnniversaryRepository.GetBirthdayListForAMonth(selectedMonth, selectedRoles.Split(','), churchId);
            var sortedList = list.OrderBy(l => l.DateOfBirth_Value.HasValue ? l.DateOfBirth_Value.Value.Day : 0);
            return GenerateFile(sortedList);
        }

        public Stream GetBirthdayListForAMonth(int selectedMonth, int churchId)
        {
            var list = _birthdayAndAnniversaryRepository.GetBirthdayListForAMonth(selectedMonth, churchId);
            var sortedList = list.OrderBy(l => l.DateOfBirth_Value.HasValue ? l.DateOfBirth_Value.Value.Day : 0);
            return GenerateFile(sortedList);
        }

        public Stream GetBirthdayListForADateRange(DateTime startDate, DateTime endDate, int churchId)
        {
            var list = _birthdayAndAnniversaryRepository.GetBirthdayListForADateRange(startDate, endDate, churchId);
            var sortedList = list.OrderBy(l => l.DateOfBirth_Value.HasValue ? l.DateOfBirth_Value.Value.Day : 0);
            return GenerateFile(sortedList);
        }

        public Stream GetAnniversaryListForForADateRange(DateTime startDate, DateTime endDate, int churchId)
        {
            var list = _birthdayAndAnniversaryRepository.GetAnniversaryListForADateRange(startDate, endDate, churchId);
            var sortedList = list.OrderBy(l => l.DateOfBirth_Value.HasValue ? l.DateOfBirth_Value.Value.Day : 0);
            return GenerateFile(sortedList);
        }

        public Stream GetAnniversaryListForAMonth(string selectedRoles, int selectedMonth, int churchId)
        {
            var list = _birthdayAndAnniversaryRepository.GetAnniversaryListForAMonth(selectedMonth, selectedRoles.Split(','), churchId);
            var sortedList = list.OrderBy(l => l.Anniversary_Value.HasValue ? l.Anniversary_Value.Value.Day : 0);
            return GenerateFile(sortedList);
        }

        public Stream GetAnniversaryListForAMonth(int selectedMonth, int churchId)
        {
            var list = _birthdayAndAnniversaryRepository.GetAnniversaryListForAMonth(selectedMonth, churchId);
            var sortedList = list.OrderBy(l => l.Anniversary_Value.HasValue ? l.Anniversary_Value.Value.Day : 0);
            return GenerateFile(sortedList);
        }

        private static Stream GenerateFile(IEnumerable<PersonViewModel> list)
        {
            var sw = new StreamWriter(new MemoryStream());
            sw.WriteLine(@"Day, Weekday, Surname, Firstname, Member Status, HomePhone, CellPhone, Email");

            foreach (var person in list)
            {
                var day = person.DateOfBirth_Value.HasValue
                    ? person.DateOfBirth_Value.Value.Day.ToString()
                    : string.Empty;
                var weekday = person.DateOfBirth_Value.HasValue
                    ? person.DateOfBirth_Value.Value.DayOfWeek.ToString()
                    : string.Empty;

                sw.WriteLine(
                    day + ", " +
                    weekday + ", " +
                    person.Surname + ", " +
                    person.Firstname + ", " +
                    person.RoleName + ", " +
                    person.HomePhone + ", " +
                    person.CellPhone + ", " +
                    person.Email + ", "
                    );
            }

            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            return sw.BaseStream;
        }
    }
}