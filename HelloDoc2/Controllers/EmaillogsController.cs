using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelloDoc2.DataContext;
using HelloDoc2.DataModels;

namespace HelloDoc2.Controllers
{
    public class EmaillogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmaillogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Emaillogs
        public async Task<IActionResult> Index()
        {
              return _context.Emaillogs != null ? 
                          View(await _context.Emaillogs.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Emaillogs'  is null.");
        }

        // GET: Emaillogs/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Emaillogs == null)
            {
                return NotFound();
            }

            var emaillog = await _context.Emaillogs
                .FirstOrDefaultAsync(m => m.Emaillogid == id);
            if (emaillog == null)
            {
                return NotFound();
            }

            return View(emaillog);
        }

        // GET: Emaillogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Emaillogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Emaillogid,Emailtemplate,Subjectname,Emailid,Confirmationnumber,Filepath,Roleid,Requestid,Adminid,Physicianid,Createdate,Sentdate,Isemailsent,Senttries,Action")] Emaillog emaillog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emaillog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emaillog);
        }

        // GET: Emaillogs/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Emaillogs == null)
            {
                return NotFound();
            }

            var emaillog = await _context.Emaillogs.FindAsync(id);
            if (emaillog == null)
            {
                return NotFound();
            }
            return View(emaillog);
        }

        // POST: Emaillogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Emaillogid,Emailtemplate,Subjectname,Emailid,Confirmationnumber,Filepath,Roleid,Requestid,Adminid,Physicianid,Createdate,Sentdate,Isemailsent,Senttries,Action")] Emaillog emaillog)
        {
            if (id != emaillog.Emaillogid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emaillog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmaillogExists(emaillog.Emaillogid))
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
            return View(emaillog);
        }

        // GET: Emaillogs/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Emaillogs == null)
            {
                return NotFound();
            }

            var emaillog = await _context.Emaillogs
                .FirstOrDefaultAsync(m => m.Emaillogid == id);
            if (emaillog == null)
            {
                return NotFound();
            }

            return View(emaillog);
        }

        // POST: Emaillogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Emaillogs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Emaillogs'  is null.");
            }
            var emaillog = await _context.Emaillogs.FindAsync(id);
            if (emaillog != null)
            {
                _context.Emaillogs.Remove(emaillog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmaillogExists(decimal id)
        {
          return (_context.Emaillogs?.Any(e => e.Emaillogid == id)).GetValueOrDefault();
        }
    }
}
