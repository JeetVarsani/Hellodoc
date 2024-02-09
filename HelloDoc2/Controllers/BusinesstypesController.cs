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
    public class BusinesstypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusinesstypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Businesstypes
        public async Task<IActionResult> Index()
        {
              return _context.Businesstypes != null ? 
                          View(await _context.Businesstypes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Businesstypes'  is null.");
        }

        // GET: Businesstypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Businesstypes == null)
            {
                return NotFound();
            }

            var businesstype = await _context.Businesstypes
                .FirstOrDefaultAsync(m => m.Businesstypeid == id);
            if (businesstype == null)
            {
                return NotFound();
            }

            return View(businesstype);
        }

        // GET: Businesstypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Businesstypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Businesstypeid,Name")] Businesstype businesstype)
        {
            if (ModelState.IsValid)
            {
                _context.Add(businesstype);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(businesstype);
        }

        // GET: Businesstypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Businesstypes == null)
            {
                return NotFound();
            }

            var businesstype = await _context.Businesstypes.FindAsync(id);
            if (businesstype == null)
            {
                return NotFound();
            }
            return View(businesstype);
        }

        // POST: Businesstypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Businesstypeid,Name")] Businesstype businesstype)
        {
            if (id != businesstype.Businesstypeid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(businesstype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinesstypeExists(businesstype.Businesstypeid))
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
            return View(businesstype);
        }

        // GET: Businesstypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Businesstypes == null)
            {
                return NotFound();
            }

            var businesstype = await _context.Businesstypes
                .FirstOrDefaultAsync(m => m.Businesstypeid == id);
            if (businesstype == null)
            {
                return NotFound();
            }

            return View(businesstype);
        }

        // POST: Businesstypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Businesstypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Businesstypes'  is null.");
            }
            var businesstype = await _context.Businesstypes.FindAsync(id);
            if (businesstype != null)
            {
                _context.Businesstypes.Remove(businesstype);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusinesstypeExists(int id)
        {
          return (_context.Businesstypes?.Any(e => e.Businesstypeid == id)).GetValueOrDefault();
        }
    }
}
