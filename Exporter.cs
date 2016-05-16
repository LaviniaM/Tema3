using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema.Models
{
    public interface Exporter
    {
        void export(List<TicketRepository> ticket);
    }
}
