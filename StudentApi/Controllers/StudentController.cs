using Microsoft.AspNetCore.Mvc;
using StudentApi.Database.AppDbContextModels;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _db;

        public StudentController()
        {
            _db = new AppDbContext();
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var lst = _db.TblStudents
                .Where(x => x.IsDelete == false)
                .ToList();
            return Ok(lst);
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] TblStudent request)
        {
            var student = new TblStudent
            {
                Code = request.Code,
                Name = request.Name,
                FatherName = request.FatherName,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
                IsDelete = false
            };
            _db.TblStudents.Add(student);
            int result = _db.SaveChanges();
            return Ok(new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Student created successfully." : "Failed to create student."
            });
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] TblStudent request)
        {
            var student = _db.TblStudents.FirstOrDefault(x => x.Id == id && x.IsDelete == false);
            if (student is null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(request.Code))
            {
                student.Code = request.Code;
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                student.Name = request.Name;
            }
            if (!string.IsNullOrEmpty(request.FatherName))
            {
                student.FatherName = request.FatherName;
            }

            student.ModifiedBy = "System";
            student.ModifiedDate = DateTime.Now;

            int result = _db.SaveChanges();
            return Ok(new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Student updated successfully." : "Failed to update student."
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _db.TblStudents.FirstOrDefault(x => x.Id == id && x.IsDelete == false);
            if (student is null)
            {
                return NotFound();
            }

            student.IsDelete = true;
            int result = _db.SaveChanges();
            return Ok(new
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Student deleted successfully." : "Failed to delete student."
            });
        }
    }
}
