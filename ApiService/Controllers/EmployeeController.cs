using MariApps.MS.Training.MSA.EmployeeMS.Business.Contracts;
using MariApps.MS.Training.MSA.EmployeeMS.DataCarrier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IO;
using MariApps.MS.Training.MSA.EmployeeMS.ApiService.Filters;

namespace MariApps.MS.Training.MSA.EmployeeMS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        private readonly IConfiguration _configuration;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService, IConfiguration configuration)
        {
            _employeeService = employeeService;
            _configuration = configuration;
            logger.LogInformation("EmployeeMSController created");
        }

        public class EmployeeUpsertRequest
        {
            public EmployeePersonalEntity Personal { get; set; }
            public EmployeeProfessionalEntity Professional { get; set; }
        }

        [HttpGet]
        public ActionResult<List<EmployeePersonalEntity>> GetAll()
        {
            return _employeeService.GetAllEmployees();
        }

        [HttpGet("{id:int}")]
        public ActionResult<EmployeePersonalEntity> GetById(int id)
        {
            var emp = _employeeService.GetEmployeeById(id);
            if (emp == null) return NotFound();
            return emp;
        }

        [HttpPost]
        [RoleAuthorize("Admin")]
        public IActionResult Create(
    [FromBody] EmployeeUpsertRequest request,
    [FromHeader(Name = "X-Role")] string? xRole = null) // <-- Swagger will show this
        {
            if (request?.Personal == null || request?.Professional == null) return BadRequest();
            _employeeService.AddEmployee(request.Personal, request.Professional);
            return CreatedAtAction(nameof(GetById), new { id = request.Personal.EmployeeId }, null);
        }


        [HttpPut("{id:int}")]
        [RoleAuthorize("Admin")]
        public IActionResult Update(
      int id,
      [FromBody] EmployeeUpsertRequest request,
      [FromHeader(Name = "X-Role")] string? xRole = null) // <-- Swagger will show this
        {
            if (request?.Personal == null || request?.Professional == null) return BadRequest();
            if (id != request.Personal.EmployeeId || id != request.Professional.EmployeeId) return BadRequest();

            _employeeService.UpdateEmployee(request.Personal, request.Professional);
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        [RoleAuthorize("Admin")]
        public IActionResult Delete(
     int id,
     [FromHeader(Name = "X-Role")] string? xRole = null)
        {
            _employeeService.DeleteEmployee(id);
            return NoContent();
        }

        [HttpPost("{id:int}/image")]
        [RoleAuthorize("Admin")]
        public IActionResult UploadImage(
    int id,
    IFormFile file,
    [FromHeader(Name = "X-Role")] string? xRole = null)
        {
            if (file == null || file.Length == 0) return BadRequest("File is required");
            var root = _configuration.GetSection("Uploads").GetValue<string>("RootFolder");
            if (string.IsNullOrWhiteSpace(root)) return StatusCode(StatusCodes.Status500InternalServerError, "Upload folder not configured");
            Directory.CreateDirectory(root);
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"emp_{id}_profile{ext}";
            var fullPath = Path.Combine(root, fileName);
            using (var stream = System.IO.File.Create(fullPath))
            {
                file.CopyTo(stream);
            }
            _employeeService.UpdateEmployeeImageUrl(id, fullPath);
            return Ok(new { path = fullPath });
        }

        [HttpGet("{id:int}/image")]
        public IActionResult DownloadImage(int id)
        {
            var emp = _employeeService.GetEmployeeById(id);
            if (emp == null || string.IsNullOrWhiteSpace(emp.ProfileImageUrl)) return NotFound();
            var path = emp.ProfileImageUrl;
            if (!System.IO.File.Exists(path)) return NotFound();
            var contentType = "application/octet-stream";
            return PhysicalFile(path, contentType, Path.GetFileName(path));
        }

        [HttpDelete("{id:int}/image")]
        [RoleAuthorize("Admin")]
        public IActionResult DeleteImage(
       int id,
       [FromHeader(Name = "X-Role")] string? xRole = null)
        {
            var emp = _employeeService.GetEmployeeById(id);
            if (emp == null) return NotFound();
            var path = emp.ProfileImageUrl;
            if (!string.IsNullOrWhiteSpace(path) && System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _employeeService.UpdateEmployeeImageUrl(id, null);
            return NoContent();
        }

        [HttpGet("export/csv")]
        public IActionResult ExportCsv()
        {
            var employees = _employeeService.GetAllEmployees();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("EmployeeId,FullName,Gender,DOB,Age,Address,ContactNo,Email,ProfileImageUrl");
            foreach (var e in employees)
            {
                var line = string.Join(',', new string[]
                {
                    e.EmployeeId.ToString(),
                    EscapeCsv(e.FullName),
                    e.Gender.ToString(),
                    e.DOB.ToString("yyyy-MM-dd"),
                    e.Age.HasValue ? e.Age.Value.ToString() : string.Empty,
                    EscapeCsv(e.Address),
                    EscapeCsv(e.ContactNo),
                    EscapeCsv(e.Email),
                    EscapeCsv(e.ProfileImageUrl)
                });
                sb.AppendLine(line);
            }
            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "employees.csv");
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (value.Contains('"')) value = value.Replace("\"", "\"\"");
            if (value.Contains(',') || value.Contains('\n') || value.Contains('\r'))
                return $"\"{value}\"";
            return value;
        }

        [HttpPost("import/csv")]
        public IActionResult ImportCsv(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("CSV file is required.");
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            string? header = reader.ReadLine();
            if (header == null) return BadRequest("Empty file");
            int imported = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var cols = ParseCsvLine(line);
                if (cols.Length < 9) continue;
                try
                {
                    var personal = new EmployeePersonalEntity
                    {
                        EmployeeId = int.Parse(cols[0]),
                        FullName = cols[1],
                        Gender = string.IsNullOrEmpty(cols[2]) ? 'M' : cols[2][0],
                        DOB = DateTime.Parse(cols[3]),
                        Age = string.IsNullOrEmpty(cols[4]) ? (int?)null : int.Parse(cols[4]),
                        Address = cols[5],
                        ContactNo = cols[6],
                        Email = cols[7],
                        ProfileImageUrl = cols[8]
                    };
                    var professional = new EmployeeProfessionalEntity
                    {
                        EmployeeId = personal.EmployeeId,
                        Designation = "",
                        Department = "",
                        Qualification = null,
                        Experience = null,
                        Skill = null
                    };
                    _employeeService.AddEmployee(personal, professional);
                    imported++;
                }
                catch
                {
                    // skip invalid rows
                }
            }
            return Ok(new { imported });
        }

        private static string[] ParseCsvLine(string line)
        {
            var list = new List<string>();
            var sb = new System.Text.StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            sb.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                else
                {
                    if (c == ',')
                    {
                        list.Add(sb.ToString());
                        sb.Clear();
                    }
                    else if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }
            list.Add(sb.ToString());
            return list.ToArray();
        }

        // Profile document endpoints (stored by naming convention: emp_{id}_profiledoc.ext)
        [HttpPost("{id:int}/document")]
        [RoleAuthorize("Admin")]
        public IActionResult UploadDocument(
      int id,
      IFormFile file,
      [FromHeader(Name = "X-Role")] string? xRole = null)
        {
            if (file == null || file.Length == 0) return BadRequest("File is required");
            var root = _configuration.GetSection("Uploads").GetValue<string>("RootFolder");
            if (string.IsNullOrWhiteSpace(root)) return StatusCode(StatusCodes.Status500InternalServerError, "Upload folder not configured");
            Directory.CreateDirectory(root);
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"emp_{id}_profiledoc{ext}";
            var fullPath = Path.Combine(root, fileName);
            using (var stream = System.IO.File.Create(fullPath))
            {
                file.CopyTo(stream);
            }
            return Ok(new { path = fullPath });
        }

        [HttpGet("{id:int}/document")]
        public IActionResult DownloadDocument(int id)
        {
            var root = _configuration.GetSection("Uploads").GetValue<string>("RootFolder");
            if (string.IsNullOrWhiteSpace(root)) return NotFound();
            if (!Directory.Exists(root)) return NotFound();
            var files = Directory.GetFiles(root, $"emp_{id}_profiledoc.*");
            if (files.Length == 0) return NotFound();
            var path = files[0];
            var contentType = "application/octet-stream";
            return PhysicalFile(path, contentType, Path.GetFileName(path));
        }

        [HttpDelete("{id:int}/document")]
        [RoleAuthorize("Admin")]
        public IActionResult DeleteDocument(
     int id,
     [FromHeader(Name = "X-Role")] string? xRole = null)
        {
            var root = _configuration.GetSection("Uploads").GetValue<string>("RootFolder");
            if (string.IsNullOrWhiteSpace(root) || !Directory.Exists(root)) return NoContent();
            var files = Directory.GetFiles(root, $"emp_{id}_profiledoc.*");
            foreach (var f in files)
            {
                try { System.IO.File.Delete(f); } catch { }
            }
            return NoContent();
        }
    }
}
