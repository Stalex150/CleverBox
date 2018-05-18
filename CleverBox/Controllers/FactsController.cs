using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CleverBox.Data;
using CleverBox.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CleverBox.Models.ViewModels;

namespace CleverBox.Controllers
{
    [Authorize]
    public class FactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public FactsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var factList = await _context
                .Facts
                .OrderBy(f => f.NextTime)
                .Where(f => f.User == user)
                .ToListAsync();

            var factViewList = new List<FactViewModel>();

            foreach (var fact in factList)
            {
                string date = "";
                string level = "";

                if (fact.NextTime < DateTime.Now)
                    date = "Now";
                else
                    date = fact.NextTime.ToString(@"dd\/MM\/yyyy");

                if (fact.RepetitionLevel == 10)
                    level = "Max";
                else
                    level = fact.RepetitionLevel.ToString();

                var factView = new FactViewModel
                {
                    Id = fact.Id,
                    UserId = fact.UserId,
                    User = fact.User,
                    Key = fact.Key,
                    Value = fact.Value,
                    RepetitionLevel = level,
                    NextTime = date
                };

                factViewList.Add(factView);
            }

            return View(factViewList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id == null)
            {
                return NotFound();
            }

            var fact = await _context.Facts
                .SingleOrDefaultAsync(m => m.Id == id);

            if (fact == null)
            {
                return NotFound();
            }

            return View(fact);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Key,Value")] Fact CurFact)
        {
            var user = await _userManager.GetUserAsync(User);

            var fact = new Fact
            {
                User = user,
                UserId = user.Id,
                Key = CurFact.Key,
                Value = CurFact.Value,
                RepetitionLevel = 0,
                NextTime = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                _context.Add(fact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(fact);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fact = await _context
                .Facts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fact == null)
            {
                return NotFound();
            }
            return View(fact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Key,Value,RepetitionLevel,NextTime")] Fact fact)
        {
            if (id != fact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactExists(fact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var fact = await _context
                .Facts
                .SingleOrDefaultAsync(m => m.Id == id);

            _context.Facts.Remove(fact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FactExists(int id)
        {
            return _context.Facts.Any(e => e.Id == id);
        }
    }
}
