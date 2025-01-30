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

        // GET: ExaminationController/Details/5
        public async Task<IActionResult> Details(long id)
        {
            return View();
        }

        // GET: ExaminationController/Create
        public async Task<IActionResult> Create(long patientId)
        {
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
                    examination.PicturePath = await CreateFile(picture);
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
            var examination = await _context.Examinations
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (examination == null) return NotFound();

            var examTypes = Enum.GetValues(typeof(ExamEnum))
                .Cast<ExamEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.GetType().GetField(e.ToString()).GetCustomAttributes<DisplayAttribute>()?.FirstOrDefault().Name ?? e.ToString()
                }).ToList();

            var patient = await _context.Patients
                .Select(p => new { p.Id, FullName = $"{p.FirstName} {p.LastName}" })
                .Where(p => p.Id == examination.PatientId)
                .FirstOrDefaultAsync();

            if (patient == null) return NotFound();

            ViewBag.Patient = patient;
            ViewBag.ExamType = examTypes;
            return View(examination);
        }

        // POST: ExaminationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Examination examination, IFormFile? picture)
        {
            if (id != examination.Id) return NotFound();
            examination.PicturePath = _context.Examinations.AsNoTracking().FirstOrDefault(e => e.Id == id)?.PicturePath;
            if (ModelState.IsValid)
            {
                if (examination.PicturePath != null)
                {
                    DeleteFile(examination.PicturePath);
                }
                if (picture != null)
                {
                    examination.PicturePath = await CreateFile(picture);
                }
                try
                {
                    _context.Update(examination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Examinations.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Patient", new { id = examination.PatientId });
            }
            return View(examination);
        }

        // GET: ExaminationController/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var examination = await _context.Examinations
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (examination == null) return NotFound();
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == examination.PatientId);
            ViewBag.Patient = patient;
            return View(examination);
        }

        // POST: ExaminationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id, Examination examination)
        {
            var exam = await _context.Examinations
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (exam == null) return NotFound();
            if (exam.PicturePath != null)
            {
                DeleteFile(exam.PicturePath);
            }
            _context.Examinations.Remove(exam);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Patient", new { id = exam.PatientId });
        }

        [HttpGet]
        public async Task<IActionResult> DownloadImage(long id)
        {
            var examination = await _context.Examinations.FirstOrDefaultAsync(e => e.Id == id);
            if (examination == null) return NotFound();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", examination.PicturePath);
            if (!System.IO.File.Exists(path)) return NotFound();
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "image/png", Path.GetFileName(path));
        }

        public async Task<string> CreateFile(IFormFile? picture)
        {
            var guidPictureName = $"{Path.GetFileNameWithoutExtension(picture.FileName)} - {Guid.NewGuid()}{Path.GetExtension(picture.FileName)}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.CreateDirectory(path).Exists) Directory.CreateDirectory(path);
            path = Path.Combine(path, guidPictureName);
            System.IO.File.Create(path).Dispose();
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }
            return guidPictureName;
        }

        public void DeleteFile(string path)
        {
            path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", path);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
