using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Lib.Web.Mvc.JQuery.JqGrid
{
    public class JqGridModelBinder : IModelBinder
    {
        #region IModelBinder Members
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");
            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext");

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(bindingContext.ModelType);

            if (controllerContext.RequestContext.HttpContext.Request.ContentType.Contains("application/json"))
                return serializer.ReadObject(controllerContext.HttpContext.Request.InputStream);
            else
            {
                string jsonString = controllerContext.HttpContext.Request[bindingContext.ModelName];
                if (String.IsNullOrEmpty(jsonString))
                    return null;
                else
                    return serializer.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(jsonString)));
            }
        }
        #endregion
    }
}
