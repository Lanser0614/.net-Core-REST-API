using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers.Post;

[ApiController]
[Route("api/[controller]")]
public class PostController : Controller
{
    private readonly ApplicationDbContext _context;

    public PostController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public JsonResult Index(
        [FromQuery(Name = "pageSize")] int pageSize,
        [FromQuery(Name = "page")] int page
    )
    {
        var employees = _context.Employees
            // .OrderBy(b => b.Id)
            // .Skip(page)
            // .Take(pageSize)
            .ToList();
        var lists = new Dictionary<string, List<Employee>>();
        lists.Add("data", employees);
        return Json(lists);
    }

    [HttpGet("{id:int}")]
    public IActionResult Show(int id)
    {
        var data = _context.Employees.Find(id);
        return Json(data);
    }


    // public IActionResult Edit()
    // {
    // return View();
    // }

    [HttpPost]
    public JsonResult Store(Employee employee)
    {
        var email = _context.Employees.Any(e => e.Email == employee.Email);
        if (email)
        {
            var message = GetMessage("This email already exists", 422.ToString());
            return Json(message);
        }

        _context.Employees.Add(employee);
        _context.SaveChanges();
        return Json(employee);
    }


    private Dictionary<string, string> GetMessage(string message, string code)
    {
        Dictionary<string, string> airports = new Dictionary<string, string>();

        airports.Add("message", message);
        airports.Add("code", code);
        return airports;
    }


    [HttpDelete("{id:int}")]
    public JsonResult Destroy(int id)
    {
        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

        if (employee == null)
        {
            var message = GetMessage("Not found", 404.ToString());
            return Json(message);
        }

        _context.Employees.Remove(employee);
        return Json(this.GetMessage("deleted", 204.ToString()));
    }
}