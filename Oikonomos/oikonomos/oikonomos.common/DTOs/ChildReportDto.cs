using System;

namespace oikonomos.common.DTOs
{
    public class ChildReportDto
    {
        public int PersonId { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string CellNo { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public string FatherEmail { get; set; }
        public string FatherCell { get; set; }
        public string MotherEmail { get; set; }
        public string MotherCell { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string GroupName { get; set; }
        public string HomePhone { get; set; }

        public int Age
        {
            get
            {
                if (!DateOfBirth.HasValue) return 0;
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}