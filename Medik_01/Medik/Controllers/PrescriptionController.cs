using Medik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medik.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly ILogger<PrescriptionController> _logger;
        private readonly MedikContext _context;

        public PrescriptionController(ILogger<PrescriptionController> logger, MedikContext medikContext)
        {
            _logger = logger;
            _context = medikContext;
        }

        // GET: PrescriptionController/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var precription = await _context.Prescriptions
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (precription == null) return NotFound();
            return View(precription);
        }

        // GET: PrescriptionController/Create
        public async Task<IActionResult> Create(long patientId)
        {
            var patient = await _context.Patients
                .Select(p => new { p.Id, FullName = $"{p.FirstName} {p.LastName}" })
                .Where(p => p.Id == patientId)
                .FirstOrDefaultAsync();

            if (patient == null) return NotFound();
            ViewBag.Patient = patient;

            return View();
        }

        // POST: PrescriptionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prescription);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Patient", new { id = prescription.PatientId });
            }

            var patient = await _context.Patients
                .Select(p => new { p.Id, FullName = $"{p.FirstName} {p.LastName}" })
                .Where(p => p.Id == prescription.PatientId)
                .FirstOrDefaultAsync();

            if (patient == null) return NotFound();
            ViewBag.Patient = patient;

            return View(prescription);
        }

        // GET: PrescriptionController/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (prescription == null) return NotFound();

            var patient = await _context.Patients
                .Select(p => new { p.Id, FullName = $"{p.FirstName} {p.LastName}" })
                .Where(p => p.Id == prescription.PatientId)
                .FirstOrDefaultAsync();
            if (patient == null) return NotFound();

            ViewBag.Patient = patient;
            return View(prescription);
        }

        // POST: PrescriptionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Prescription prescription)
        {
            var patient = await _context.Patients
                .Select(p => new { p.Id, FullName = $"{p.FirstName} {p.LastName}" })
                .Where(p => p.Id == prescription.PatientId)
                .FirstOrDefaultAsync();

            if (id != prescription.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Prescriptions.Any(p => p.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Patient", new { id = prescription.PatientId });
            }
            return View(prescription);
        }

        // GET: PrescriptionController/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (prescription == null) return NotFound();

            var patient = await _context.Patients
                .Select(p => new { p.Id, FullName = $"{p.FirstName} {p.LastName}" })
                .Where(p => p.Id == prescription.PatientId)
                .FirstOrDefaultAsync();
            if (patient == null) return NotFound();

            ViewBag.Patient = patient;
            return View(prescription);
        }

        // POST: PrescriptionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id, Prescription prescription)
        {
            var presc = await _context.Prescriptions
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (presc == null) return NotFound();
            _context.Prescriptions.Remove(presc);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Patient", new { id = presc.PatientId });
        }
    }
}
