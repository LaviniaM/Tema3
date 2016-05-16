using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Tema.Models
{
    public class JsonExporter: Exporter
    {
        public void export(List<TicketRepository> ticket)
        {
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = pathDesktop + "\\bilete.json";
             string json="";
           // string output = jss.Serialize(ticket);

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
                json = JsonConvert.SerializeObject(ticket, Formatting.Indented);
      
                System.IO.File.WriteAllText(filePath, json);
        

        }
    }
}