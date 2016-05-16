using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Tema.Models;

namespace Tema.Controllers
{
    public class TicketController : Controller
    {
        private TicketDBContext db = new TicketDBContext();
        private ShowDBContext showDb = new ShowDBContext();

        // GET: Ticket
        public ActionResult TicketView()
        {
            return View(db.ticket.ToList());
        }

        // GET: Ticket/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketRepository ticketRepository = db.ticket.Find(id);
            if (ticketRepository == null)
            {
                return HttpNotFound();
            }
            return View(ticketRepository);
        }

        // GET: Ticket/Create
        public ActionResult Create()
        {
            return View();
        }


        public int getNumarBilete(String titlu)
        {
            int nrBileteVandute = 0;
            List<TicketRepository> ticket = new List<TicketRepository>();
            ticket = db.ticket.ToList();

            foreach (var item in ticket)
            {
                if (item.titluSpectacol.Equals(titlu))
                {
                    nrBileteVandute++;
                }
            }

            return nrBileteVandute;

        }

        //verificare daca locul este disponibil
        public bool locDisponibil([Bind(Include = "Id,titluSpectacol,rand,numar")] TicketRepository ticketRepository)
        {
            List<TicketRepository> ticket = new List<TicketRepository>();
            ticket = db.ticket.ToList();
            foreach (var item in ticket)
            {
                if (item.titluSpectacol.Equals(ticketRepository.titluSpectacol))
                {
                    if ((item.rand == ticketRepository.rand) && (item.numar == ticketRepository.numar))
                        return false;
                }
            }

            return true;

        }


        // POST: Ticket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,titluSpectacol,rand,numar")] TicketRepository ticketRepository)
        {

            ShowController show = new ShowController(); 
            int total=show.getNumarTotalBilete(ticketRepository.titluSpectacol);
            if(total==0){
                ModelState.AddModelError("", "Spectacol inexistent!");
            }
            else{
                if (total > this.getNumarBilete(ticketRepository.titluSpectacol)) {
                    if (this.locDisponibil(ticketRepository))
                    {
                        if (ModelState.IsValid)
                        {
                            if (ticketRepository.numar <= 40 && ticketRepository.rand <= 10)
                            {
                                db.ticket.Add(ticketRepository);
                                db.SaveChanges();
                                return RedirectToAction("TicketView");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Randul maxim este 10 si numarul 40!");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Locul nu este disponibil!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nu mai sunt bilete disponibile pentru acest spectacol!");
                }
            }

            return View(ticketRepository);
        }
       

        [HttpPost]
        public ActionResult TicketView(UserRepository user)
        {
           
            return View();
        }

    
        public ActionResult Export(int id)
        {
            List<TicketRepository> ticket = new List<TicketRepository>();
            ticket = db.ticket.ToList();
           
            
                Exporter ex = ExportFactory.getExporter(id);
                if (ex != null)
                {
                    ex.export(ticket);
                }
            
           
            return RedirectToAction("TicketView");
        }
     
        // GET: Ticket/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketRepository ticketRepository = db.ticket.Find(id);
            if (ticketRepository == null)
            {
                return HttpNotFound();
            }
            return View(ticketRepository);
        }

        // POST: Ticket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,titluSpectacol,rand,numar")] TicketRepository ticketRepository)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketRepository).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TicketView");
            }
            return View(ticketRepository);
        }

        // GET: Ticket/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketRepository ticketRepository = db.ticket.Find(id);
            if (ticketRepository == null)
            {
                return HttpNotFound();
            }
            return View(ticketRepository);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketRepository ticketRepository = db.ticket.Find(id);
            db.ticket.Remove(ticketRepository);
            db.SaveChanges();
            return RedirectToAction("TicketView");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
