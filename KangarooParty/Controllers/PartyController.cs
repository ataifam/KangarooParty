using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KangarooParty.Data;
using KangarooParty.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            //send all kangaroos to view 
            ViewData["Kangaroos"] = new SelectList(dbContext.Kangaroos, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePartyModel template)
        {
            //find kangaroo selected in form
            var kangaroo = dbContext.Find<Kangaroo>(template.HostId);
            if(kangaroo != null)
            {
                var party = new Party
                {
                    Host = kangaroo,
                };

                //assign kangaroo this new party to host
                kangaroo.HostingParty = party;
                kangaroo.HostingPartyId = party.Id;

                await dbContext.AddAsync(party);
                await dbContext.SaveChangesAsync();
            }

            return View();
        }
    }
}

