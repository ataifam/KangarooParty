using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KangarooParty.Data;
using KangarooParty.Models;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateKangarooModel template)
        {
            var kangaroo = new Kangaroo
            {
                Name = template.Name,

            };

            await dbContext.AddAsync(kangaroo);
            await dbContext.SaveChangesAsync();

            return View();
        }
    }
}

