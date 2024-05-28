using System;
using System.Collections.Generic;
using System.IO;
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
    public class KangarooController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public KangarooController(ApplicationDbContext DbContext)
        {
            dbContext = DbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var kangaroos = await dbContext.Kangaroos.Include(c => c.HostingParty).ToListAsync();
            return View(kangaroos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateKangarooModel template)
        {
            if(template.Name != null)
            {
                var kangaroo = new Kangaroo
                {
                    Name = template.Name,

                };

                await dbContext.AddAsync(kangaroo);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> KangarooInfo(int id)
        {
            //include all kangaroo related fields, then query specific kangaroo 
            var kangaroo = await dbContext.Kangaroos
                .Include(c => c.HostingParty)
                .Include(c => c.AttendingParty)
                .Include(c => c.AttendingParty != null ? c.AttendingParty.Host : null)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (kangaroo != null)
            {
                //compile list of available parties to join if kangaroo is not a host
                if(kangaroo.HostingParty == null)
                {
                    var data = new SelectList(
                        dbContext.Parties.Where(c => c.Id != kangaroo.AttendingPartyId)
                        .Select(c => new Kangaroo
                        {
                            Id = c.Id,
                            Name = c.Host.Name + "'s Party",
                        }), "Id", "Name");

                    ViewData["Parties"] = data.Any() ? data : null;
                }

                return View(kangaroo);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> KangarooInfo(Kangaroo template)
        {
            //include all kangaroo related fields, then query specific kangaroo 
            var kangaroo = await dbContext.Kangaroos
                .Include(c => c.AttendingParty)
                .FirstOrDefaultAsync(c => c.Id == template.Id);

            var party = await dbContext.Parties
                .Include(c => c.Attendees)
                .FirstOrDefaultAsync(c => c.Id == template.AttendingPartyId);

            if (kangaroo != null)
            {
                kangaroo.Name = template.Name;

                //determine if a party to attend was selected
                if(party != null)
                {
                    /* 2 cases:
                     * (a) if fmr party is null, add the kangaroo to the new
                     * party and modify the prestige
                     * (b) if fmr party is not null, and the fmr party != new
                     * party, add the kangaroo to new party and account for the
                     * prestige of both parties
                     */
                    if(kangaroo.AttendingParty == null)
                    {
                        party.Prestige = (party.Attendees.Count() + 1) / 5;
                        kangaroo.AttendingPartyId = party.Id;
                    }
                    else if(kangaroo.AttendingParty != null && kangaroo.AttendingParty.Id != party.Id)
                    {
                        kangaroo.AttendingParty.Prestige = (party.Attendees.Count() - 1) / 5;
                        party.Prestige = (party.Attendees.Count() + 1) / 5;
                        kangaroo.AttendingPartyId = party.Id;
                    }
                }

                await dbContext.SaveChangesAsync();
                return RedirectToAction("KangarooInfo", new { kangaroo.Id });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> LeaveParty(Kangaroo template)
        {
            var kangaroo = await dbContext.Kangaroos
                .Include(c => c.AttendingParty)
                .FirstOrDefaultAsync(c => c.Id == template.Id);

            if (kangaroo != null && kangaroo.AttendingParty != null)
            {
                kangaroo.AttendingParty.Prestige = (kangaroo.AttendingParty.Attendees.Count() - 1) / 5;
                kangaroo.AttendingPartyId = null;

                await dbContext.SaveChangesAsync();
                return RedirectToAction("KangarooInfo", new { kangaroo.Id });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Kangaroo template)
        {
            var kangaroo = await dbContext.Kangaroos
                .Include(c => c.HostingParty)
                .Include(c => c.AttendingParty)
                .Include(c => c.HostingParty.Attendees)
                .FirstOrDefaultAsync(c => c.Id == template.Id);

            if (kangaroo != null)
            {
                //account for deleted kangaroo attending a party
                if(kangaroo.AttendingParty != null)
                {
                    kangaroo.AttendingParty.Prestige = (kangaroo.AttendingParty.Attendees.Count() - 1) / 5;
                }
                dbContext.Kangaroos.Remove(kangaroo);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}

