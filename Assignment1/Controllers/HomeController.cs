using Assignment1.Data;
using Assignment1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult EmployeeList()
    {
        var employees = _context.Employees.ToList();
        return View(employees);
    }

    [HttpGet]
    public IActionResult AddEmployee()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddEmployee(Employee employee)
    {
        if (ModelState.IsValid)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return RedirectToAction("EmployeeList");
        }

        return View(employee);
    }

    public IActionResult Browse(int id = 0)
    {
        var employee = _context.Employees.Skip(id).FirstOrDefault();
        if (employee == null)
        {
            return NotFound();
        }

        ViewBag.CurrentIndex = id;
        ViewBag.TotalEmployees = _context.Employees.Count() - 1;

        return View(employee);
    }

    [HttpGet]
    public IActionResult EditEmployee(int id)
    {
        var employee = _context.Employees.Find(id);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }


    [HttpPost]
    public IActionResult EditEmployee(Employee updatedEmployee)
    {
        if (ModelState.IsValid)
        {
            _context.Employees.Update(updatedEmployee);
            _context.SaveChanges();
            return RedirectToAction("EmployeeList");
        }

        return View(updatedEmployee);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    [HttpPost, ActionName("DeleteEmployee")]
    public async Task<IActionResult> DeleteEmployeeConfirmed(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(EmployeeList));
    }
}