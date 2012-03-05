using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;

namespace oikonomos.common.Models
{
    public class TableGenerationModel
    {
        public string Sql { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CommandType { get; set; }
        public List<SelectListItem> CommandTypeOptions { get; set; }
        public DataTable Results { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}