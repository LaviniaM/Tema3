using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tema.Models
{
    public class ExportFactory
    {
        public static Exporter getExporter(int id){
            if (id==1)
            {
               return new CsvExporter();

            }
            else
            {
                if (id==2)
                {
                  return  new JsonExporter();
                }
            }
            return null;
    }
    }
}