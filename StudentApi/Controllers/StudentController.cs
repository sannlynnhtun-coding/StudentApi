using StudentApi.Dtos;
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
                .Select(x => new StudentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    FatherName = x.FatherName,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    IsDelete = x.IsDelete
                })
                .ToList();

            return Ok(lst);
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] StudentCreateRequestDto request)
        {
            var student = new TblStudent
            {
                Code = Guid.NewGuid().ToString("N").Substring(0, 8), // optional: auto-generate Code
                Name = request.Name,
                FatherName = request.FatherName,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
                IsDelete = false
            };

            _db.TblStudents.Add(student);
            int result = _db.SaveChanges();

            var response = new StudentCreateResponseDto
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Student created successfully." : "Failed to create student."
            };

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] StudentUpdateRequestDto request)
        {
            var student = _db.TblStudents.FirstOrDefault(x => x.Id == id && x.IsDelete == false);
            if (student is null)
            {
                return NotFound();
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

            var response = new StudentCreateResponseDto
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Student updated successfully." : "Failed to update student."
            };

            return Ok(response);
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

            var response = new StudentCreateResponseDto
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Student deleted successfully." : "Failed to delete student."
            };

            return Ok(response);
        }
    }
}
