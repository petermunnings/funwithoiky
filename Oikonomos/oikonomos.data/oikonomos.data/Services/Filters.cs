using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oikonomos.data.Services
{
    public class Filters
    {
        public static IQueryable<Person> ApplyNameSearch(string searchString, IQueryable<Person> people)
        {
            string completeName = searchString;
            string[] names = completeName.Split(' ');
            switch (names.Length)
            {
                case 1:
                    {
                        people = (from p in people
                                  where p.Firstname.Contains(completeName)
                                  || p.Family.FamilyName.Contains(completeName)
                                  select p);
                        break;
                    }
                case 2:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        people = (from p in people
                                  where (p.Firstname.Contains(name0) && p.Family.FamilyName.Contains(name1))
                                  || (p.Firstname.Contains(completeName))
                                  || (p.Family.FamilyName.Contains(completeName))
                                  select p);
                        break;
                    }
                case 3:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        string name2 = names[2];
                        people = (from p in people
                                  where (p.Firstname.Contains(name0 + " " + name1) && p.Family.FamilyName.Contains(name2))
                                  || (p.Firstname.Contains(name0) && p.Family.FamilyName.Contains(name1 + " " + name2))
                                  || (p.Firstname.Contains(completeName))
                                  || (p.Family.FamilyName.Contains(completeName))
                                  select p);
                        break;
                    }
                case 4:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        string name2 = names[2];
                        string name3 = names[3];
                        people = (from p in people
                                  where (p.Firstname.Contains(name0 + " " + name1) && p.Family.FamilyName.Contains(name2 + " " + name3))
                                  || (p.Firstname.Contains(name0) && p.Family.FamilyName.Contains(names[1] + " " + name2 + " " + name3))
                                  || (p.Firstname.Contains(name0 + " " + name1 + " " + name2) && p.Family.FamilyName.Contains(name3))
                                  || (p.Firstname.Contains(completeName))
                                  || (p.Family.FamilyName.Contains(completeName))
                                  select p);
                        break;
                    }
                case 5:
                case 6:
                case 7:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        string name2 = names[2];
                        string name3 = names[3];
                        string name4 = names[4];
                        people = (from p in people
                                  where (p.Firstname.Contains(name0) && p.Family.FamilyName.Contains(name1 + " " + name2 + " " + name3 + " " + name4))
                                  || (p.Firstname.Contains(name0 + " " + name1) && p.Family.FamilyName.Contains(name2 + " " + name3 + " " + name4))
                                  || (p.Firstname.Contains(name0 + " " + name1 + " " + name2) && p.Family.FamilyName.Contains(name3 + " " + name4))
                                  || (p.Firstname.Contains(name0 + " " + name1 + " " + name2 + " " + name3) && p.Family.FamilyName.Contains(name4))
                                  || (p.Firstname.Contains(completeName))
                                  || (p.Family.FamilyName.Contains(completeName))
                                  select p);
                        break;
                    }
            }
            return people;
        }

        public static IQueryable<Group> ApplyNameSearchGroupLeader(string searchString, IQueryable<Group> groups)
        {
            string completeName = searchString;
            string[] names = completeName.Split(' ');
            switch (names.Length)
            {
                case 1:
                    {
                        groups = (from g in groups
                                  where g.Leader.Firstname.Contains(completeName)
                                  || g.Leader.Family.FamilyName.Contains(completeName)
                                  select g);
                        break;
                    }
                case 2:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        groups = (from g in groups
                                  where (g.Leader.Firstname.Contains(name0) && g.Leader.Family.FamilyName.Contains(name1))
                                  || (g.Leader.Firstname.Contains(completeName))
                                  || (g.Leader.Family.FamilyName.Contains(completeName))
                                  select g);
                        break;
                    }
                case 3:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        string name2 = names[2];
                        groups = (from g in groups
                                  where (g.Leader.Firstname.Contains(name0 + " " + name1) && g.Leader.Family.FamilyName.Contains(name2))
                                  || (g.Leader.Firstname.Contains(name0) && g.Leader.Family.FamilyName.Contains(name1 + " " + name2))
                                  || (g.Leader.Firstname.Contains(completeName))
                                  || (g.Leader.Family.FamilyName.Contains(completeName))
                                  select g);
                        break;
                    }
                case 4:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        string name2 = names[2];
                        string name3 = names[3];
                        groups = (from g in groups
                                  where (g.Leader.Firstname.Contains(name0 + " " + name1) && g.Leader.Family.FamilyName.Contains(name2 + " " + name3))
                                  || (g.Leader.Firstname.Contains(name0) && g.Leader.Family.FamilyName.Contains(names[1] + " " + name2 + " " + name3))
                                  || (g.Leader.Firstname.Contains(name0 + " " + name1 + " " + name2) && g.Leader.Family.FamilyName.Contains(name3))
                                  || (g.Leader.Firstname.Contains(completeName))
                                  || (g.Leader.Family.FamilyName.Contains(completeName))
                                  select g);
                        break;
                    }
                case 5:
                case 6:
                case 7:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        string name2 = names[2];
                        string name3 = names[3];
                        string name4 = names[4];
                        groups = (from g in groups
                                  where (g.Leader.Firstname.Contains(name0) && g.Leader.Family.FamilyName.Contains(name1 + " " + name2 + " " + name3 + " " + name4))
                                  || (g.Leader.Firstname.Contains(name0 + " " + name1) && g.Leader.Family.FamilyName.Contains(name2 + " " + name3 + " " + name4))
                                  || (g.Leader.Firstname.Contains(name0 + " " + name1 + " " + name2) && g.Leader.Family.FamilyName.Contains(name3 + " " + name4))
                                  || (g.Leader.Firstname.Contains(name0 + " " + name1 + " " + name2 + " " + name3) && g.Leader.Family.FamilyName.Contains(name4))
                                  || (g.Leader.Firstname.Contains(completeName))
                                  || (g.Leader.Family.FamilyName.Contains(completeName))
                                  select g);
                        break;
                    }
            }
            return groups;
        }

        public static IQueryable<Group> ApplyNameSearchGroupAdministrator(string searchString, IQueryable<Group> groups)
        {
            string completeName = searchString;
            string[] names = completeName.Split(' ');
            switch (names.Length)
            {
                case 1:
                    {
                        groups = (from g in groups
                                  where g.Administrator.Firstname.Contains(completeName)
                                  || g.Administrator.Family.FamilyName.Contains(completeName)
                                  select g);
                        break;
                    }
                case 2:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        groups = (from g in groups
                                  where (g.Administrator.Firstname.Contains(name0) && g.Administrator.Family.FamilyName.Contains(name1))
                                  || (g.Administrator.Firstname.Contains(completeName))
                                  || (g.Administrator.Family.FamilyName.Contains(completeName))
                                  select g);
                        break;
                    }
                case 3:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        string name2 = names[2];
                        groups = (from g in groups
                                  where (g.Administrator.Firstname.Contains(name0 + " " + name1) && g.Administrator.Family.FamilyName.Contains(name2))
                                  || (g.Administrator.Firstname.Contains(name0) && g.Administrator.Family.FamilyName.Contains(name1 + " " + name2))
                                  || (g.Administrator.Firstname.Contains(completeName))
                                  || (g.Administrator.Family.FamilyName.Contains(completeName))
                                  select g);
                        break;
                    }
                case 4:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        string name2 = names[2];
                        string name3 = names[3];
                        groups = (from g in groups
                                  where (g.Administrator.Firstname.Contains(name0 + " " + name1) && g.Administrator.Family.FamilyName.Contains(name2 + " " + name3))
                                  || (g.Administrator.Firstname.Contains(name0) && g.Administrator.Family.FamilyName.Contains(names[1] + " " + name2 + " " + name3))
                                  || (g.Administrator.Firstname.Contains(name0 + " " + name1 + " " + name2) && g.Administrator.Family.FamilyName.Contains(name3))
                                  || (g.Administrator.Firstname.Contains(completeName))
                                  || (g.Administrator.Family.FamilyName.Contains(completeName))
                                  select g);
                        break;
                    }
                case 5:
                case 6:
                case 7:
                    {
                        string name0 = names[0];
                        string name1 = names[1];
                        string name2 = names[2];
                        string name3 = names[3];
                        string name4 = names[4];
                        groups = (from g in groups
                                  where (g.Administrator.Firstname.Contains(name0) && g.Administrator.Family.FamilyName.Contains(name1 + " " + name2 + " " + name3 + " " + name4))
                                  || (g.Administrator.Firstname.Contains(name0 + " " + name1) && g.Administrator.Family.FamilyName.Contains(name2 + " " + name3 + " " + name4))
                                  || (g.Administrator.Firstname.Contains(name0 + " " + name1 + " " + name2) && g.Administrator.Family.FamilyName.Contains(name3 + " " + name4))
                                  || (g.Administrator.Firstname.Contains(name0 + " " + name1 + " " + name2 + " " + name3) && g.Administrator.Family.FamilyName.Contains(name4))
                                  || (g.Administrator.Firstname.Contains(completeName))
                                  || (g.Administrator.Family.FamilyName.Contains(completeName))
                                  select g);
                        break;
                    }
            }
            return groups;
        }

    }
}
