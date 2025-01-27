using Medik.Enums;
using Medik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Medik.Controllers
{
    public class ExaminationController : Controller
    {
        private readonly ILogger<ExaminationController> _logger;
        private readonly MedikContext _context;

        public ExaminationController(ILogger<ExaminationController> logger, MedikContext medikContext)
        {
            _logger = logger;
            _context = medikContext;
        }

        // GET: ExaminationController
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: ExaminationController/Details/5
        public async Task<IActionResult> Details(long id)
        {
            return View();
        }

        // GET: ExaminationController/Create
        public async Task<IActionResult> Create(long patientId)
        {
            //ViewBag.ExamType = Enum.GetValues(typeof(ExamEnum)).Cast<ExamEnum>().ToList();
            ////ViewBag.Patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == patientId);
            //ViewBag.Patient = patientId;
            //return View();
            var patient = await _context.Patients
                .Select(p => new { p.Id, FullName = $"{p.FirstName} {p.LastName}" })
                .Where(p => p.Id == patientId)
                .FirstOrDefaultAsync();

            if (patient == null) return NotFound();

            var examTypes = Enum.GetValues(typeof(ExamEnum))
                .Cast<ExamEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.GetType().GetField(e.ToString()).GetCustomAttributes<DisplayAttribute>()?.FirstOrDefault().Name ?? e.ToString()
                }).ToList();

            ViewBag.Patient = patient;
            //ViewBag.ExamType = Enum.GetValues(typeof(ExamEnum)).Cast<ExamEnum>().ToList();
            ViewBag.ExamType = examTypes;
            return View();
        }

        // POST: ExaminationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Examination examination, IFormFile? picture)
        {
            if (ModelState.IsValid)
            {
                if (picture != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", picture.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await picture.CopyToAsync(stream);
                    }
                    examination.PicturePath = picture.FileName;
                }
                _context.Examinations.Add(examination);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Patient", new { id = examination.PatientId });
            }
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage); // Or use any logger
            }
            return View(examination);
        }

        // GET: ExaminationController/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            return View();
        }

        // POST: ExaminationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Examination examination)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExaminationController/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            return View();
        }

        // POST: ExaminationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id, Examination examination)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
