using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Assignment1.Models;

namespace Assignment1.Controllers;

 public class HomeController : Controller
    {
        // List to store employees
        private static List<Employee> employees = new List<Employee>
        {
            // Default list of static employees for viewing
            new Employee { EmployeeID = 1, FirstName = "Mousam", LastName = "Dhakal", JobTitle = "Developer", Salary = 53000 },
            new Employee { EmployeeID = 2, FirstName = "Arbin", LastName = "Shrestha", JobTitle = "Designer", Salary = 62570 },
            new Employee { EmployeeID = 3, FirstName = "Parmod", LastName = "Shrestha", JobTitle = "QA", Salary = 130900 },
            new Employee { EmployeeID = 4, FirstName = "KP", LastName = "Oli", JobTitle = "Prime Minister", Salary = 300000 },
            new Employee { EmployeeID = 5, FirstName = "Albert", LastName = "Danison", JobTitle = "CEO", Salary = 190000 }
        };
        
        // Index page displays our project name, link to the Employee List and contributors
        public IActionResult Index()
        {
            return View();
        }

        // Action to display the list of employees
        public IActionResult EmployeeList()
        {
            return View(employees);
        }

        // Action to add a new employee
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
                // Generate a new Employee ID automatically
                // Based on the maximum Employee ID in the list and incremented by 1
                employee.EmployeeID = employees.Count > 0 ? employees.Max(e => e.EmployeeID) + 1 : 1;

                // Add the new employee to the list
                employees.Add(employee);

                // Redirect to the index page
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // Action to browse employees one by one
        public IActionResult Browse(int id = 0)
        {
            if (id < 0 || id >= employees.Count)
            {
                id = 0; // Reset to the first employee if out of bounds
            }

            var employee = employees[id];
            ViewBag.CurrentIndex = id;
            ViewBag.TotalEmployees = employees.Count - 1;

            return View(employee);
        }
    }