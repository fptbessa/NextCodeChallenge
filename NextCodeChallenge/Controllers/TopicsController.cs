using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextCodeChallenge.Models.Entities;

namespace NextCodeChallenge.Controllers
{
    public class TopicsController : Controller
    {
        private readonly NextCodeChallengeDbContext _context;

        public TopicsController(NextCodeChallengeDbContext context)
        {
            _context = context;
        }

        // GET: Topics
        public async Task<IActionResult> Index()
        {
            var nextCodeChallengeDbContext = _context.Topics.Include(t => t.CreatorUser);
            return View(await nextCodeChallengeDbContext.ToListAsync());
        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .Include(t => t.CreatorUser)
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                TempData["GenericMessage"] = "Only logged in users can create posts.";

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopicId,Title,Description")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.CreatorUserId = HttpContext.Session.GetInt32("UserId").Value;
                topic.CreationDate = DateTime.Now;

                _context.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            TempData["GenericMessage"] = "Edited successfully.";

            return RedirectToAction("Index", "Home");
        }

        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            ViewData["CreatorUserId"] = new SelectList(_context.Users, "UserId", "Password", topic.CreatorUserId);
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicId,Title,Description,CreatorUserId,CreationDate")] Topic topic)
        {
            if (id != topic.TopicId)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetInt32("UserId") != topic.CreatorUserId)
            {
                TempData["ErrorMessage"] = "Error: User did not create this post.";

                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.TopicId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            TempData["GenericMessage"] = "Edited successfully.";

            return RedirectToAction("Index", "Home");
        }

        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .Include(t => t.CreatorUser)
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _context.Topics.FindAsync(id);

            if (topic != null && HttpContext.Session.GetInt32("UserId") != topic.CreatorUserId)
            {
                TempData["ErrorMessage"] = "Error: User did not create this post.";

                return RedirectToAction("Index", "Home");
            }
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            TempData["GenericMessage"] = "Edited successfully.";

            return RedirectToAction("Index", "Home");
        }

        private bool TopicExists(int id)
        {
            return _context.Topics.Any(e => e.TopicId == id);
        }
    }
}
