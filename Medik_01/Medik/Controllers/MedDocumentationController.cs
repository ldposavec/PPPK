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
        public async Task<IActionResult> Index()
        {
            var medDocumentations = await _context.MedDocumentations.ToListAsync();
            return View(medDocumentations);
        }

        // GET: MedDocumentationController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var medDocumentation = await _context.MedDocumentations
                .Include(md => md.Patient)
                .FirstOrDefaultAsync(md => md.Id == id);
            if (medDocumentation == null) return NotFound();
            return View(medDocumentation);
        }

        // GET: MedDocumentationController/Create
        public async Task<IActionResult> Create(long patientId)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == patientId);
            ViewBag.Patient = patient;
            if (patient == null) return NotFound();
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
            return View(medDocumentation);
        }

        // GET: MedDocumentationController/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var medDocumentation = await _context.MedDocumentations.FirstOrDefaultAsync(md => md.Id == id);
            if (medDocumentation == null) return NotFound();
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
            return View(medDocumentation);
        }

        // GET: MedDocumentationController/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var medDocumentation = await _context.MedDocumentations.FirstOrDefaultAsync(md => md.Id == id);
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
            _context.MedDocumentations.Remove(medDocumentation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Patient", new { id = medDocumentation.PatientId });
        }
    }
}
