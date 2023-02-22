using HSS.Admin.Data;
using HSS.SharedModels.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSS.Admin.Controllers
{
    [Authorize()]
    public class TenantsController : Controller
    {
        private readonly HSSAdminContext _context;

        public TenantsController(HSSAdminContext context)
        {
            _context = context;
        }

        // GET: Tenants
        public async Task<IActionResult> Index()
        {
              return View(await _context.TenantEntity.ToListAsync());
        }

        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TenantEntity == null)
            {
                return NotFound();
            }

            var tenantEntity = await _context.TenantEntity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tenantEntity == null)
            {
                return NotFound();
            }

            return View(tenantEntity);
        }

        // GET: Tenants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status")] TenantEntity tenantEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tenantEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tenantEntity);
        }

        // GET: Tenants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TenantEntity == null)
            {
                return NotFound();
            }

            var tenantEntity = await _context.TenantEntity.FindAsync(id);
            if (tenantEntity == null)
            {
                return NotFound();
            }
            return View(tenantEntity);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status")] TenantEntity tenantEntity)
        {
            if (id != tenantEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tenantEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TenantEntityExists(tenantEntity.Id))
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
            return View(tenantEntity);
        }

        // GET: Tenants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TenantEntity == null)
            {
                return NotFound();
            }

            var tenantEntity = await _context.TenantEntity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tenantEntity == null)
            {
                return NotFound();
            }

            return View(tenantEntity);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TenantEntity == null)
            {
                return Problem("Entity set 'HSSAdminContext.TenantEntity'  is null.");
            }
            var tenantEntity = await _context.TenantEntity.FindAsync(id);
            if (tenantEntity != null)
            {
                _context.TenantEntity.Remove(tenantEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TenantEntityExists(int id)
        {
          return _context.TenantEntity.Any(e => e.Id == id);
        }
    }
}
