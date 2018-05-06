using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleverBox.Data;
using CleverBox.Models;
using CleverBox.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CleverBox.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Test()
        {
            var user = await _userManager.GetUserAsync(User);

            if (!_context
                .Facts
                .Where(f => f.NextTime <= DateTime.Now)
                .Where(f => f.User == user)
                .Any())
                return View("Done");

            var fact = await _context
                 .Facts
                 .Where(f => f.NextTime <= DateTime.Now)
                 .Where(f => f.User == user)
                 .OrderBy(f => f.NextTime)
                 .FirstOrDefaultAsync();

            var thisFact = new TestViewModel
            {
                Fact = fact,
                InputText = null
            };

            return View(thisFact);
        }

        [HttpPost]
        public async Task<IActionResult> Evaluation(int factId, string input)
        {
            var fact = await _context
                .Facts
                .AsNoTracking()
                .SingleOrDefaultAsync(f => f.Id == factId);

            var user = await _userManager.GetUserAsync(User);


            if (input == fact.Value)
            {
                if (fact.RepetitionLevel != 10)
                {
                    Random random = new Random();
                    user.Points += random.Next(11, 15);
                    fact.RepetitionLevel++;
                }
            }
            else
                fact.RepetitionLevel = 0;

            fact.NextTime = SetDate(fact.RepetitionLevel);


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    _context.Update(fact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            }

            TimeSpan span = fact.NextTime - DateTime.Now;

            int daySpan = (int)Math.Round((decimal)span.TotalDays);

            string daysTo = "";

            if (daySpan == 0)
                daysTo = "Now";
            else
                daysTo = daySpan.ToString();

            var evaluateModel = new EvaluateViewModel
            {
                Value = fact.Value,
                Key = fact.Key,
                Input = input,
                DaysToNexTest = daysTo
            };

            return View(evaluateModel);
        }

        [NonAction]
        private DateTime SetDate(int repLevel)
        {
            DateTime date = DateTime.Now;

            switch (repLevel)
            {
                case 0:
                    date = date.AddDays(2 * 0);
                    break;
                case 1:
                    date = date.AddDays(2 * 1);
                    break;
                case 2:
                    date = date.AddDays(2 * 2);
                    break;
                case 3:
                    date = date.AddDays(3 * 3);
                    break;
                case 4:
                    date = date.AddDays(4 * 4);
                    break;
                case 5:
                    date = date.AddDays(5 * 5);
                    break;
                case 6:
                    date = date.AddDays(6 * 6);
                    break;
                case 7:
                    date = date.AddDays(7 * 7);
                    break;
                case 8:
                    date = date.AddDays(8 * 8);
                    break;
                case 9:
                    date = date.AddDays(9 * 9);
                    break;
                case 10:
                    date = date.AddDays(10 * 10);
                    break;
            }

            return date;
        }
    }
}
