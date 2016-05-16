using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Tema.Models
{
    public class CsvExporter: Exporter
    {
        public void export(List<TicketRepository> ticket)
        {
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = pathDesktop + "\\bilete.csv";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            
            using (System.IO.TextWriter writer = File.CreateText(filePath))
            {
                writer.Write(string.Join(",","Titlu","Rand","Loc" ) + Environment.NewLine);
                foreach (var item in ticket)
                {
                    writer.Write(string.Join(",", item.titluSpectacol, item.rand, item.numar) + Environment.NewLine);
                }
            }
        }
    }
}