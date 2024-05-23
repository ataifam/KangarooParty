﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KangarooParty.Data;
using KangarooParty.Models;
using Microsoft.AspNetCore.Mvc;
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
                .FirstOrDefaultAsync(c => c.Id == id);

            if (kangaroo != null)
            {
                return View(kangaroo);
            }

            return RedirectToAction("Index");
        }

        //html functionality needed
        [HttpPost]
        public async Task<IActionResult> KangarooInfo(Kangaroo template)
        {
            //include all kangaroo related fields, then query specific kangaroo 
            var kangaroo = await dbContext.Kangaroos
                .Include(c => c.HostingParty)
                .Include(c => c.AttendingParty)
                .FirstOrDefaultAsync(c => c.Id == template.Id);

            if (kangaroo != null)
            {
                kangaroo.Name = template.Name;

                await dbContext.SaveChangesAsync();
                return View(kangaroo);
            }
            return RedirectToAction("Index");
        }
    }
}

