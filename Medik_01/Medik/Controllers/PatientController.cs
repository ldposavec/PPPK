using Medik.Enums;
using Medik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medik.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly MedikContext _context;

        public PatientController(ILogger<PatientController> logger, MedikContext medikContext)
        {
            _logger = logger;
            _context = medikContext;
        }

        // GET: PatientController
        public async Task<IActionResult> Index(string search = "")
        {
            var patients = await _context.Patients
                .Where(p => p.LastName.Contains(search) || p.Oib.Contains(search))
                .ToListAsync();
            return View(patients);
        }

        // GET: PatientController/Details/5
        public async Task <IActionResult> Details(long id)
        {
            var patient = await _context.Patients
                .Include(p => p.MedDocumentations)
                .Include(p => p.Examinations)
                .Include(p => p.Prescriptions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null) return NotFound();
            return View(patient);
        }

        // GET: PatientController/Create
        public IActionResult Create()
        {
            ViewBag.GenderList = Enum.GetValues(typeof(GenderEnum)).Cast<GenderEnum>().ToList();
            return View();
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage); // Or use any logger
            }
            ViewBag.GenderList = Enum.GetValues(typeof(GenderEnum)).Cast<GenderEnum>().ToList();
            return View(patient);
        }

        // GET: PatientController/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            ViewBag.GenderList = Enum.GetValues(typeof(GenderEnum)).Cast<GenderEnum>().ToList();

            return View(patient);
        }

        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Patient patient)
        {
            if (patient == null) return NotFound();
            if (id != patient.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.GenderList = Enum.GetValues(typeof(GenderEnum)).Cast<GenderEnum>().ToList();
            return View(patient);
        }

        // GET: PatientController/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id, IFormCollection collection)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null) return NotFound();
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error deleting patient");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
