using AccessToAuth.Data.Context;
using AccessToAuth.Data.Entities.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccessTo.Web.Controllers
{
    [Authorize(Policy = "MainRole")]
    public class EmployeeController : MainController
    {

        #region Constructor

        private readonly DatabaseContext _context;
        public EmployeeController(DatabaseContext context)
        {
            _context = context;
        }

        #endregion

        // GET: Employee
        //[AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Policy = "EmployeeListPolicy")]
        // [Authorize(Policy = "ClaimOrRole")]
        //[Authorize(Policy = "ClaimRequirement")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.ID == ID);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,City,Gender")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Edit/7e622d86-ef85-46a0-6aa0-08dbdd9aaf68
        public async Task<IActionResult> Edit(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(ID);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/7e622d86-ef85-46a0-6aa0-08dbdd9aaf68
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid ID, [Bind("ID,FirstName,LastName,City,Gender")] Employee employee)
        {
            if (ID != employee.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID))
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
            return View(employee);
        }

        // GET: Employee/Delete/7e622d86-ef85-46a0-6aa0-08dbdd9aaf68
        public async Task<IActionResult> Delete(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.ID == ID);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/7e622d86-ef85-46a0-6aa0-08dbdd9aaf68
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid ID)
        {
            var employee = await _context.Employees.FindAsync(ID);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }







        private bool EmployeeExists(Guid ID)
        {
            return _context.Employees.Any(e => e.ID == ID);
        }

    }
}