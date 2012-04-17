using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using oikonomos.web.Helpers;
using oikonomos.data;
using oikonomos.data.DataAccessors;
using oikonomos.common.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Text;
using oikonomos.common;

namespace oikonomos.web.Controllers
{
    public class ReportController : Controller
    {
        private static string[] Months={"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};

        public FileStreamResult ChurchList(bool search, string searchField, string searchString)
        {
            Person currentUser = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentUser == null)
            {
                Redirect("/Home/Index");
            }

            List<PersonListViewModel> churchList = PersonDataAccessor.FetchChurchList(currentUser, search, searchField, searchString);
            MemoryStream stream = new MemoryStream();
            CreatePeopleListDocument(currentUser, churchList, "Member").Save(stream, false);

            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=churchlist.pdf");
            return new FileStreamResult(stream, "application/pdf");
        }

        public FileStreamResult PeopleList(int roleId, string roleName)
        {
            Person currentUser = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentUser == null)
            {
                Redirect("/Home/Index");
            }

            List<PersonListViewModel> peopleList = PersonDataAccessor.FetchPeople(currentUser, roleId);
            MemoryStream stream = new MemoryStream();
            CreatePeopleListDocument(currentUser, peopleList, roleName).Save(stream, false);

            HttpContext.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}list.pdf", roleName));
            return new FileStreamResult(stream, "application/pdf");
        }

        //public FileStreamResult PastMemberList()
        //{
        //    Person currentUser = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
        //    if (currentUser == null)
        //    {
        //        Redirect("/Home/Index");
        //    }

        //    List<PersonListViewModel> visitorList = PersonDataAccessor.FetchPeople(currentUser, (int)SecurityRoles.PastMember);
        //    MemoryStream stream = new MemoryStream();
        //    CreatePeopleListDocument(currentUser, visitorList, "Past Member").Save(stream, false);

        //    HttpContext.Response.AddHeader("content-disposition", "attachment; filename=pastmemberlist.pdf");
        //    return new FileStreamResult(stream, "application/pdf");
        //}

        public FileStreamResult HomeGroupList(string id)
        {
            Person currentUser = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentUser == null)
            {
                Redirect("/Home/Index");
            }

            int groupId = int.Parse(id);
            List<PersonViewModel> peopleList = GroupDataAccessor.FetchPeopleInGroup(groupId);
            
            string hgName = GroupDataAccessor.FetchHomeGroupName(groupId);
            MemoryStream stream = new MemoryStream();
            CreateHomeGroupListDocument(hgName, peopleList).Save(stream, false);

            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=homegrouplist.pdf");
            return new FileStreamResult(stream, "application/pdf");
        }
        
        public FileStreamResult HomeGroupAttendance(string id)
        {
            Person currentUser = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentUser == null)
            {
                Redirect("/Home/Index");
            }

            int groupId = int.Parse(id);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
            List<AttendanceEventViewModel> attendees = EventDataAccessor.FetchGroupAttendance(currentUser, groupId, startDate, endDate);
            Dictionary<int, string> comments = EventDataAccessor.FetchGroupComments(currentUser, groupId);

            string hgName = GroupDataAccessor.FetchHomeGroupName(groupId);
            MemoryStream stream = new MemoryStream();
            CreateHomeGroupAttendanceDocument(hgName, attendees, comments).Save(stream, false);

            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=homegroupattendance.pdf");
            return new FileStreamResult(stream, "application/pdf");
        }

        public FileStreamResult ExportChurchData()
        {
            Person currentUser = SecurityHelper.CheckCurrentUser(Session, Response, ViewBag);
            if (currentUser == null)
            {
                Redirect("/Home/Index");
            }
            
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=churchdata.csv");
            StreamWriter sw = new StreamWriter(new MemoryStream());
            sw.WriteLine(@"Surname, Firstname, CellPhone, Email, HomePhone, WorkPhone, RoleName, Address1, Address2, Address3, Address4, Anniversary, DateOfBirth, Gender, Occupation, Skype, Twitter, Site, HeardAbout");

            List<PersonViewModel> list = PersonDataAccessor.FetchExtendedChurchList(currentUser);
            foreach (PersonViewModel person in list)
            {
                sw.WriteLine(person.Surname + ", " + 
                    person.Firstname + ", " + 
                    person.CellPhone + ", " + 
                    person.Email + ", " + 
                    person.HomePhone + ", " + 
                    person.WorkPhone + ", " + 
                    person.RoleName + ", " + 
                    person.Address1 + ", " + 
                    person.Address2 + ", " +
                    person.Address3 + ", " +
                    person.Address4 + ", " +
                    person.Anniversary + ", " +
                    person.DateOfBirth + ", " +
                    person.Gender + ", " +
                    person.Occupation + ", " +
                    person.Skype + ", " +
                    person.Twitter + ", " +
                    person.Site + ", " +
                    person.HeardAbout
                    );
            }

            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(sw.BaseStream, "text/csv");
        }

        #region Private Helper Methods
        private static MigraDoc.Rendering.PdfDocumentRenderer CreateHomeGroupListDocument(string groupName, List<PersonViewModel> membersList)
        {
            // Create new MigraDoc document
            Document document = new Document();
            document.Info.Title = groupName;
            document.Info.Author = "oikonomos";
            document.Info.Subject = "Homegroup List for " + groupName;
            Section sec = document.AddSection();
            sec.PageSetup.TopMargin = Unit.FromCentimeter(1);
            sec.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            document.DefaultPageSetup.TopMargin = Unit.FromCentimeter(1);
            document.DefaultPageSetup.LeftMargin = Unit.FromCentimeter(1);

            Paragraph p = sec.Headers.Primary.AddParagraph();
            p.AddText("Homegroup List for " + groupName);
            p.Format.Font.Size = 10.0;
            p.Format.Font.Bold = true;
            p.Format.Alignment = ParagraphAlignment.Center;

            Paragraph pf = new Paragraph();
            pf.AddText("Date: " + DateTime.Now.ToString("dd MMM yyyy"));
            pf.Format.Font.Size = 6.0;
            sec.Footers.Primary.Add(pf);

            MigraDoc.DocumentObjectModel.Tables.Table membersTable = new MigraDoc.DocumentObjectModel.Tables.Table();
            AddHeaders(membersTable, "Members");
            AddHomegroupData(document, membersList, membersTable);

            MigraDoc.DocumentObjectModel.Tables.Table visitorsTable = new MigraDoc.DocumentObjectModel.Tables.Table();

            sec.Add(membersTable);

            document.LastSection.PageSetup.TopMargin = Unit.FromMillimeter(20);

            MigraDoc.Rendering.PdfDocumentRenderer pdfRender = new MigraDoc.Rendering.PdfDocumentRenderer(false, PdfSharp.Pdf.PdfFontEmbedding.Always);
            pdfRender.Document = document; //document is where all of my info has been written to and is a MigraDoc type
            pdfRender.RenderDocument();

            return pdfRender;
        }

        private static void AddHomegroupData(Document document, List<PersonViewModel> personList, MigraDoc.DocumentObjectModel.Tables.Table table)
        {
            var style = document.Styles["Normal"];
            style.Font.Size = 8.0;
            TextMeasurement tm = new TextMeasurement(style.Font.Clone());
            int familyId = 0;
            foreach (PersonViewModel person in personList)
            {
                Row row = table.AddRow();
                Cell cell = row.Cells[0];
                if (familyId != person.FamilyId)
                {
                    cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.Surname, tm));
                }
                cell = row.Cells[1];
                cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.Firstname, tm));
                cell = row.Cells[2];
                if (familyId != person.FamilyId)
                {
                    cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.HomePhone ?? string.Empty, tm));
                }
                cell = row.Cells[3];
                cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.WorkPhone ?? string.Empty, tm));
                cell = row.Cells[4];
                cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.CellPhone ?? string.Empty, tm));
                cell = row.Cells[5];
                cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.Email ?? string.Empty, tm));
                familyId = person.FamilyId;
            }
        }
        
        private static MigraDoc.Rendering.PdfDocumentRenderer CreateHomeGroupAttendanceDocument(string groupName, List<AttendanceEventViewModel> attendees, Dictionary<int, string> comments)
        {
            // Create new MigraDoc document
            Document document = new Document();
            document.Info.Title = groupName;
            document.Info.Author = "oikonomos";
            document.Info.Subject = "Homegroup Attendance for " + groupName;
            Section sec = document.AddSection();
            sec.PageSetup.TopMargin = Unit.FromCentimeter(1);
            sec.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            document.DefaultPageSetup.TopMargin = Unit.FromCentimeter(1);
            document.DefaultPageSetup.LeftMargin = Unit.FromCentimeter(1);

            Paragraph p = sec.Headers.Primary.AddParagraph();
            p.AddText("Homegroup Attendance for " + groupName);
            p.Format.Font.Size = 10.0;
            p.Format.Font.Bold = true;
            p.Format.Alignment = ParagraphAlignment.Center;

            Paragraph pf = new Paragraph();
            pf.AddText("Date: " + DateTime.Now.ToString("dd MMM yyyy"));
            pf.Format.Font.Size = 6.0;
            sec.Footers.Primary.Add(pf);

            //Work out which distinct days there are in each month
            List<int> month1Events = new List<int>();
            List<int> month2Events = new List<int>();
            int month1 = DateTime.Now.AddMonths(-1).Month;
            int month2 = DateTime.Now.Month;
            GetAttendanceDays(attendees, month1Events, month2Events, month1, month2);

            if (month1Events.Count == 0)
            {
                month1Events.Add(1);
            }
            if (month2Events.Count == 0)
            {
                month2Events.Add(1);
            }

            MigraDoc.DocumentObjectModel.Tables.Table attendeesTable = new MigraDoc.DocumentObjectModel.Tables.Table();
            int totalColumns = AddAttendanceHeaders(month1, month2, month1Events, month2Events, attendeesTable, "Members");
            AddAttendanceData(attendees, document, month1Events, month2Events, month1, month2, attendeesTable, totalColumns, comments);
            sec.Add(attendeesTable);

            document.LastSection.PageSetup.TopMargin = Unit.FromMillimeter(20);

            MigraDoc.Rendering.PdfDocumentRenderer pdfRender = new MigraDoc.Rendering.PdfDocumentRenderer(false, PdfSharp.Pdf.PdfFontEmbedding.Always);
            pdfRender.Document = document; //document is where all of my info has been written to and is a MigraDoc type
            pdfRender.RenderDocument();

            return pdfRender;
        }

        private static void AddAttendanceData(List<AttendanceEventViewModel> membersList, 
            Document document, 
            List<int> month1Events, 
            List<int> month2Events, 
            int month1, 
            int month2, 
            MigraDoc.DocumentObjectModel.Tables.Table table, 
            int totalColumns,
            Dictionary<int, string> comments)
        {
            var style = document.Styles["Normal"];
            style.Font.Size = 8.0;
            TextMeasurement tm = new TextMeasurement(style.Font.Clone());
            int familyId = 0;
            int personId = 0;
            //Sort the List
            membersList.Sort(delegate(AttendanceEventViewModel e1, AttendanceEventViewModel e2)
            {
                int familyIdComp = e1.FamilyId.CompareTo(e2.FamilyId);
                if (familyIdComp == 0)
                {
                    int personIdComp = e1.PersonId.CompareTo(e2.PersonId);
                    if (personIdComp == 0)
                    {
                        return e1.Date.CompareTo(e2.Date);
                    }
                    else
                    {
                        return personIdComp;
                    }
                }
                return familyIdComp;
            });

            if (membersList.Count == 0)
            {
                Row emptyRow = table.AddRow();
                Cell emptyCell = emptyRow.Cells[0];
                emptyCell.AddParagraph("No attendance has been captured over the last two months");
                emptyCell.MergeRight = 4;
            }
            else
            {
                Row row = null;
                foreach (AttendanceEventViewModel attendanceEvent in membersList)
                {
                    Cell cell;
                    if (personId != attendanceEvent.PersonId)
                    {
                        row = table.AddRow();
                        cell = row.Cells[0];
                        if (familyId != attendanceEvent.FamilyId)
                        {
                            cell.AddParagraph(AdjustIfTooWideToFitIn(cell, attendanceEvent.Surname, tm));
                        }
                        cell = row.Cells[1];
                        cell.AddParagraph(AdjustIfTooWideToFitIn(cell, attendanceEvent.Firstname, tm));
                        string comment = (from c in comments where c.Key==attendanceEvent.PersonId select c.Value).FirstOrDefault();
                        if (comment != null && comment != string.Empty)
                        {
                        cell = row.Cells[totalColumns - 1];
                        cell.AddParagraph(AdjustIfTooWideToFitIn(cell, comment, tm));
                        }
                    }

                    cell = row.Cells[GetEventColumn(attendanceEvent.Date, month1, month2, month1Events, month2Events)];
                    if (attendanceEvent.Attended)
                    {
                        cell.AddParagraph("x");
                        cell.Format.Alignment = ParagraphAlignment.Center;
                    }
                    else
                    {
                        cell.AddParagraph(" ");
                        cell.Format.Alignment = ParagraphAlignment.Center;
                    }



                    familyId = attendanceEvent.FamilyId;
                    personId = attendanceEvent.PersonId;
                }
            }
        }

        private static void GetAttendanceDays(List<AttendanceEventViewModel> homegroupList, List<int> month1Events, List<int> month2Events, int month1, int month2)
        {
            foreach (AttendanceEventViewModel attendance in homegroupList)
            {
                int month = attendance.Date.Month;

                if (month == month1)
                {
                    if (!month1Events.Contains(attendance.Date.Day))
                    {
                        month1Events.Add(attendance.Date.Day);
                    }
                }
                else if (month == month2)
                {
                    if (!month2Events.Contains(attendance.Date.Day))
                    {
                        month2Events.Add(attendance.Date.Day);
                    }
                }
            }
        }

        private static int GetEventColumn(DateTime date, int month1, int month2, List<int> month1Events, List<int> month2Events)
        {
            int colCount = 2;
            foreach (int day in month1Events)
            {
                if (date == new DateTime(date.Year, month1, day))
                {
                    return colCount;
                }
                colCount++;
            }
            foreach (int day in month2Events)
            {
                if (date == new DateTime(date.Year, month2, day))
                {
                    return colCount;
                }
                colCount++;
            }
            return 2;
        }

        private static MigraDoc.Rendering.PdfDocumentRenderer CreatePeopleListDocument(Person currentUser, List<PersonListViewModel> churchList, string documentType)
        {
            // Create new MigraDoc document
            Document document = new Document();
            document.Info.Title = documentType + " List for " + currentUser.Church.Name;
            document.Info.Author = "oikonomos";
            document.Info.Subject = documentType + " List";
            Section sec = document.AddSection();
            sec.PageSetup.TopMargin = Unit.FromCentimeter(1);
            sec.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            document.DefaultPageSetup.TopMargin = Unit.FromCentimeter(1);
            document.DefaultPageSetup.LeftMargin = Unit.FromCentimeter(1);

            Paragraph p = new Paragraph();
            p.AddText(documentType + " List for " + currentUser.Church.Name);
            p.Format.Font.Size = 6.0;
            sec.Footers.Primary.Add(p);

            MigraDoc.DocumentObjectModel.Tables.Table table = new MigraDoc.DocumentObjectModel.Tables.Table();
            AddHeaders(table, documentType + "s");

            var style = document.Styles["Normal"];
            style.Font.Size = 8.0;
            TextMeasurement tm = new TextMeasurement(style.Font.Clone());
            int familyId = 0;
            foreach (PersonListViewModel person in churchList)
            {
                Row row=table.AddRow();
                Cell cell = row.Cells[0];
                if (familyId != person.FamilyId)
                {
                    cell.Format.Font.Bold = true;
                    cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.Surname, tm));
                }
                cell = row.Cells[1];
                cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.Firstname, tm));
                cell = row.Cells[2];
                if (familyId != person.FamilyId)
                {
                    cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.HomePhone ?? string.Empty, tm));
                }
                cell = row.Cells[3];
                cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.WorkPhone ?? string.Empty, tm));
                cell = row.Cells[4];
                cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.CellPhone ?? string.Empty, tm));
                cell = row.Cells[5];
                cell.AddParagraph(AdjustIfTooWideToFitIn(cell, person.Email ?? string.Empty, tm));
                familyId = person.FamilyId;
            }

            sec.Add(table);
            
            MigraDoc.Rendering.PdfDocumentRenderer pdfRender = new MigraDoc.Rendering.PdfDocumentRenderer(false, PdfSharp.Pdf.PdfFontEmbedding.Always);
            pdfRender.Document = document; //document is where all of my info has been written to and is a MigraDoc type
            pdfRender.RenderDocument();

            return pdfRender;
        }

        private static int AddAttendanceHeaders(int month1, int month2, List<int> month1Events, List<int> month2Events, MigraDoc.DocumentObjectModel.Tables.Table table, string titleMessage)
        {
            //Headers
            // Create a font
            Font font = new Font("Arial", 8);
            int totalDays = month1Events.Count + month2Events.Count;
            table.Borders.Width = 0.25;

            table.AddColumn(Unit.FromCentimeter(3));  //Firstname
            table.AddColumn(Unit.FromMillimeter(28)); //Surname
            foreach (int day in month1Events)
            {
                table.AddColumn(Unit.FromMillimeter(56/totalDays)); //Weeks in month 1
            }
            foreach (int day in month2Events)
            {
                table.AddColumn(Unit.FromMillimeter(56/totalDays)); //Weeks in month 2
            }
            table.AddColumn(Unit.FromCentimeter(2)); //Member/Visitor
            table.AddColumn(Unit.FromCentimeter(5)); //Comments

            MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Font.ApplyFont(font);
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Shading.Color = Colors.PaleGoldenrod;
            row.Format.Font.Bold = true;
            Cell cell = row.Cells[0];
            cell.AddParagraph(titleMessage);
            cell.MergeRight = 1;
            cell = row.Cells[2];
            cell.AddParagraph(Months[month1-1]);
            if (month1Events.Count > 1)
            {
                cell.MergeRight = month1Events.Count-1;
            }
            cell = row.Cells[2+month1Events.Count];
            cell.AddParagraph(Months[month2-1]);
            if (month2Events.Count > 1)
            {
                cell.MergeRight = month2Events.Count - 1;
            }
            cell = row.Cells[2+month1Events.Count+month2Events.Count];
            cell.AddParagraph(" ");

            //Add the 2nd row
            row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Font.ApplyFont(font);
            row.Shading.Color = Colors.PaleGoldenrod;
            cell = row.Cells[0];
            cell.AddParagraph("Surname");
            cell = row.Cells[1];
            cell.AddParagraph("Firstname");
            int colCount = 2;
            foreach (int day in month1Events)
            {
                cell = row.Cells[colCount];
                cell.AddParagraph(day.ToString());
                cell.Format.Alignment = ParagraphAlignment.Center;
                colCount++;
            }
            foreach (int day in month2Events)
            {
                cell = row.Cells[colCount];
                cell.AddParagraph(day.ToString());
                cell.Format.Alignment = ParagraphAlignment.Center;
                colCount++;
            }
            cell = row.Cells[colCount++];
            cell.AddParagraph("Role");
            cell = row.Cells[colCount];
            cell.AddParagraph("Comments");

            return totalDays + 3;

        }

        private static void AddHeaders(MigraDoc.DocumentObjectModel.Tables.Table table, string headingType)
        {
            //Headers
            // Create a font
            Font font = new Font("Arial", 8);

            table.Borders.Width = 0.25;

            table.AddColumn(Unit.FromCentimeter(3));
            table.AddColumn(Unit.FromMillimeter(28));
            table.AddColumn(Unit.FromMillimeter(28));
            table.AddColumn(Unit.FromMillimeter(28));
            table.AddColumn(Unit.FromMillimeter(28));
            table.AddColumn(Unit.FromCentimeter(5));

            MigraDoc.DocumentObjectModel.Tables.Row rowHeader = table.AddRow();
            rowHeader.HeadingFormat = true;
            rowHeader.Format.Font.ApplyFont(font);
            rowHeader.Shading.Color = Colors.PaleGoldenrod;
            rowHeader.Format.Font.Bold = true;
            Cell cellHeader = rowHeader.Cells[0];
            cellHeader.AddParagraph(headingType);
            cellHeader.MergeRight = 5;

            MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Font.ApplyFont(font);
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.PaleGoldenrod;
            Cell cell = row.Cells[0];
            cell.AddParagraph("Surname");
            cell = row.Cells[1];
            cell.AddParagraph("Firstname");
            cell = row.Cells[2];
            cell.AddParagraph("Home");
            cell = row.Cells[3];
            cell.AddParagraph("Work");
            cell = row.Cells[4];
            cell.AddParagraph("Cell");
            cell = row.Cells[5];
            cell.AddParagraph("Email");

        }

        private static string AdjustIfTooWideToFitIn(Cell cell, string text, TextMeasurement tm)
        {
            Column column = cell.Column;
            Unit availableWidth = column.Width - column.Table.Borders.Width - cell.Borders.Width - Unit.FromMillimeter(2);

            var tooWideWords = text.Split(" ".ToCharArray()).Distinct().Where(s => TooWide(s, availableWidth, tm));

            var adjusted = new StringBuilder(text);
            foreach (string word in tooWideWords)
            {
                var replacementWord = MakeFit(word, availableWidth, tm);
                adjusted.Replace(word, replacementWord);
            }

            return adjusted.ToString();
        }

        private static bool TooWide(string word, Unit width, TextMeasurement tm)
        {
            float f = tm.MeasureString(word, UnitType.Point).Width;
            return f > width.Point;
        }

        private static string MakeFit(string word, Unit width, TextMeasurement tm)
        {
            var adjustedWord = new StringBuilder();
            var current = string.Empty;
            foreach (char c in word)
            {
                if (TooWide(current + c, width, tm))
                {
                    adjustedWord.Append(current);
                    adjustedWord.Append(Chars.CR);
                    current = c.ToString();
                }
                else
                {
                    current += c;
                }
            }
            adjustedWord.Append(current);

            return adjustedWord.ToString();
        }
        #endregion Private Helper Methods
    }
}
