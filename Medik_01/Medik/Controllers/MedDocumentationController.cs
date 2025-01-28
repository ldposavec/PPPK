using Medik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medik.Controllers
{
    public class MedDocumentationController : Controller
    {
        private readonly ILogger<MedDocumentationController> _logger;
        private readonly MedikContext _context;

        public MedDocumentationController(ILogger<MedDocumentationController> logger, MedikContext medikContext)
        {
            _logger = logger;
            _context = medikContext;
        }

        // GET: MedDocumentationController
        public async Task<IActionResult> Index(string search = "")
        {
            //var medDocumentations = await _context.MedDocumentations.ToListAsync();
            var medDocumentations = string.IsNullOrEmpty(search) ? await _context.MedDocumentations.Include(m => m.Patient).ToListAsync() : await _context.MedDocumentations
                .Include(m => m.Patient)
                .Where(m => m.Diagnosis.Contains(search) || m.Patient.FirstName.Contains(search) || m.Patient.LastName.Contains(search))
                .ToListAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_MedDocumentationListPartial", medDocumentations);
            }

            return View(medDocumentations);
        }

        // GET: MedDocumentationController/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var medDocumentation = await _context.MedDocumentations
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(md => md.Id == id);
            if (medDocumentation == null) return NotFound();
            return View(medDocumentation);
        }

        // GET: MedDocumentationController/Create
        public async Task<IActionResult> Create(long patientId)
        {
            var patient = await _context.Patients
                .Select(p => new { p.Id, FullName = $"{p.FirstName} {p.LastName}" })
                .Where(p => p.Id == patientId)
                .FirstOrDefaultAsync();

            if (patient == null) return NotFound();

            ViewBag.Patient = patient;
            //return View(new MedDocumentation { Patient = patient });
            return View();
        }

        // POST: MedDocumentationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedDocumentation medDocumentation)
        {
            if (ModelState.IsValid)
            {
                _context.MedDocumentations.Add(medDocumentation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Patient", new { id = medDocumentation.PatientId});
            }

            ViewBag.Patient = await _context.Patients
                .Select(p => new { p.Id, FullName = $"{p.FirstName} {p.LastName}" })
                .Where(p => p.Id == medDocumentation.PatientId)
                .FirstOrDefaultAsync();
            return View(medDocumentation);
        }

        // GET: MedDocumentationController/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var medDocumentation = await _context.MedDocumentations
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(md => md.Id == id);
            if (medDocumentation == null) return NotFound();

            var patient = await _context.Patients
                .Select(p => new { p.Id, FullName = $"{p.FirstName} {p.LastName}" })
                .Where(p => p.Id == medDocumentation.PatientId)
                .FirstOrDefaultAsync();
            if (patient == null) return NotFound();

            ViewBag.Patient = patient;
            return View(medDocumentation);
        }

        // POST: MedDocumentationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, MedDocumentation medDocumentation)
        {
            if (medDocumentation == null) return NotFound();
            if (id != medDocumentation.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                _context.MedDocumentations.Update(medDocumentation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Patient", new { id = medDocumentation.PatientId });
            }
            ViewBag.Patient = await _context.Patients
                .Select(p => new { p.Id, FullName = $"{p.FirstName} {p.LastName}" })
                .Where(p => p.Id == medDocumentation.PatientId)
                .FirstOrDefaultAsync();
            return View(medDocumentation);
        }

        // GET: MedDocumentationController/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var medDocumentation = await _context.MedDocumentations
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(md => md.Id == id);
            if (medDocumentation == null) return NotFound();
            return View(medDocumentation);
        }

        // POST: MedDocumentationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id, MedDocumentation medDocumentation)
        {
            if (medDocumentation == null) return NotFound();
            if (id != medDocumentation.Id) return BadRequest();
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == p.MedDocumentations.Where(m => m.Id == id).FirstOrDefault().PatientId);
            _context.MedDocumentations.Remove(medDocumentation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Patient", new { id = patient.Id });
        }
    }
}
