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
        // Get all employees ordered by ID
        var allEmployees = _context.Employees.OrderBy(e => e.EmployeeID).ToList();

        if (!allEmployees.Any())
        {
            // Handle case when there are no employees
            return RedirectToAction("EmployeeList");
        }

        // If id is 0 (default), use the first employee
        Employee? employee;
        int currentIndex;

        if (id == 0)
        {
            // Use first employee when no specific ID is provided
            employee = allEmployees.FirstOrDefault();
            currentIndex = 0;
        }
        else
        {
            // Find the employee with the specified ID
            employee = allEmployees.FirstOrDefault(e => e.EmployeeID == id);

            // If not found, use the first employee
            if (employee == null)
            {
                employee = allEmployees.FirstOrDefault();
                currentIndex = 0;
            }
            else
            {
                // Find the index of the employee in our ordered list
                currentIndex = allEmployees.IndexOf(employee);
            }
        }

        // If we still don't have an employee (empty DB), return not found
        if (employee == null)
        {
            return NotFound();
        }

        // Get previous and next employee IDs
        int? prevId = currentIndex > 0 ? allEmployees[currentIndex - 1].EmployeeID : null;
        int? nextId = currentIndex < allEmployees.Count - 1 ? allEmployees[currentIndex + 1].EmployeeID : null;

        // Set ViewBag properties
        ViewBag.CurrentId = employee.EmployeeID;
        ViewBag.PrevId = prevId;
        ViewBag.NextId = nextId;

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