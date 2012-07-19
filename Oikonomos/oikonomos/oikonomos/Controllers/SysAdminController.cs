using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Lib.Web.Mvc.DynamicColumns;
using oikonomos.data;
using oikonomos.common;

namespace oikonomos.web.Controllers
{
    public class SysAdminController : Controller
    {
        public JsonResult RunSql(string queryString)
        {
            var response = new DynamicColumnResponse();
            Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
            if (currentPerson.HasPermission(common.Permissions.SystemAdministrator))
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(queryString, con))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        var results = new DataTable();
                        response.ColumnModel = new List<ColumnModel>();
                        da.Fill(results);
                        foreach (DataColumn column in results.Columns)
                        {
                            response.ColumnModel.Add(new ColumnModel() { index = column.ColumnName, label = column.ColumnName, name = column.ColumnName, width = 100 });
                        }

                        response.RowValues = new List<Dictionary<string, object>>();
                        foreach (DataRow row in results.Rows)
                        {
                            var rowValues = new Dictionary<string, object>();
                            foreach (DataColumn column in results.Columns)
                            {
                                if (row[column] is DateTime)
                                {
                                    rowValues.Add(column.ColumnName, ((DateTime)row[column]).ToString("yyyy/MM/dd HH:mm"));
                                    continue;
                                }
                                rowValues.Add(column.ColumnName, row[column]);
                            }
                            response.RowValues.Add(rowValues);
                        }
                    }
                    con.Close();
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}
