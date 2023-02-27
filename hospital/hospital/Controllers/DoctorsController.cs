using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hospital.Models;

namespace hospital.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly HospitalContext _context;

        public DoctorsController(HospitalContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var hospitalContext = _context.Doctors.Include(d => d.Clinc);
            return View(await hospitalContext.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Clinc)
                .FirstOrDefaultAsync(m => m.DocId == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["ClincId"] = new SelectList(_context.Clinics, "ClincId", "ClincName");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocId,DocName,ClincId")] Doctor doctor, IFormFile DocImg)
        {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(DocImg.FileName);
                doctor.DocImg = DocImg.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await DocImg.CopyToAsync(fileStream);
                }
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClincId"] = new SelectList(_context.Clinics, "ClincId", "ClincName", doctor.ClincId);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["ClincId"] = new SelectList(_context.Clinics, "ClincId", "ClincName", doctor.ClincId);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DocId,DocName,ClincId")] Doctor doctor , IFormFile DocImg)
        {
            if (id != doctor.DocId)
            {
                return NotFound();
            }

            ModelState.Remove("DocImg");

            if (ModelState.IsValid)
            {
                try
                {
                    if (DocImg != null && DocImg.Length > 0)
                    {
                        var fileName = Path.GetFileName(DocImg.FileName);
                        doctor.DocImg = DocImg.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await DocImg.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        var existingModel = _context.Doctors.AsNoTracking().FirstOrDefault(x => x.DocId == id);
                        doctor.DocImg = existingModel.DocImg;

                    }
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.DocId))
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
            ViewData["ClincId"] = new SelectList(_context.Clinics, "ClincId", "ClincName", doctor.ClincId);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Clinc)
                .FirstOrDefaultAsync(m => m.DocId == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Doctors == null)
            {
                return Problem("Entity set 'HospitalContext.Doctors'  is null.");
            }
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
          return (_context.Doctors?.Any(e => e.DocId == id)).GetValueOrDefault();
        }
    }
}
