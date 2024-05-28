using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KangarooParty.Data;
using KangarooParty.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KangarooParty.Controllers
{
    public class PartyController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public PartyController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var parties = await dbContext.Parties
                                        .Include(c => c.Host)
                                        .Include(c => c.Attendees)
                                        .ToListAsync();
            return View(parties);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //send all kangaroos to view 
            var data = new SelectList(
                dbContext.Kangaroos.Where(c => c.HostingParty == null && c.AttendingParty == null),
                "Id",
                "Name");

            ViewData["Kangaroos"] = data.Any() ? data : null;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePartyModel template)
        {
            //find kangaroo selected in form
            var kangaroo = await dbContext.Kangaroos.FindAsync(template.HostId);
            //cannot host more than 1 party, cannot host & attend a party
            if(kangaroo != null && kangaroo.AttendingParty == null && kangaroo.HostingParty == null)
            {
                var party = new Party
                {
                    HostId = kangaroo.Id,
                };

                await dbContext.AddAsync(party);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> PartyInfo(int id)
        {
            //include all party related fields, then query specific party 
            var party = await dbContext.Parties
                .Include(c => c.Host)
                .Include(c => c.Attendees)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (party != null)
            {
                var data = new SelectList(
                dbContext.Kangaroos.Where(c => c.HostingParty == null && c.AttendingParty == null),
                "Id",
                "Name");

                ViewData["Kangaroos"] = data.Any() ? data : null;

                return View(party);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PartyInfo(Party template)
        {
            //new host kangaroo
            var kangaroo = await dbContext.Kangaroos.FindAsync(template.Host.Id);

            //current party
            var party = await dbContext.Parties
                .Include(c => c.Host)
                .FirstOrDefaultAsync(c => c.Id == template.Id);

            if (kangaroo != null && party != null)
            {
                //remove previous host
                party.HostId = kangaroo.Id;

                await dbContext.SaveChangesAsync();
                return RedirectToAction("PartyInfo", new { party.Id });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Party template)
        {
            var party = await dbContext.Parties
                .Include(c => c.Host)
                .Include(c => c.Attendees)
                .FirstOrDefaultAsync(c => c.Id == template.Id);

            if (party != null)
            {
                dbContext.Parties.Remove(party);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}

