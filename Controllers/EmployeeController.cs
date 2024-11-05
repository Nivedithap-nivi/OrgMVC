using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrgMVC.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace OrgMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;


        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var employee = await GetEmployeeAsync();

            return View("EmployeeView", employee);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Create()
        {
            Employee employee = new Employee();
            return View("CreateDepartment", employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var isSuccess = await PostDepartmentAsync(employee);
                if (isSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to insert employee.");
                }

            }
            return View(employee);
        }
        private async Task<bool> PostDepartmentAsync(Employee employee)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:7012/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonSerializer.Serialize(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("api/Emps", content);

            return response.IsSuccessStatusCode;
        }

        private async Task<List<Employee>> GetEmployeeAsync()
        {

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:7012/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/Emps");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Employee>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                throw new Exception("Failed to retrieve employee from API");
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return View("EditEmpView", employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var isSuccess = await UpdateEmployeeAsync(employee);
                if (isSuccess) return RedirectToAction("Index");
                ModelState.AddModelError(string.Empty, "Failed to update employee.");
            }
            return View("EditEmpView", employee);
        }

        private async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:7012/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"api/DeptTbls/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Employee>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;

        }

        private async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:7012/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Serialize the employee object to JSON
            var json = JsonSerializer.Serialize(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the PUT request to the Web API
            HttpResponseMessage response = await client.PutAsync($"api/DeptTbls/{employee.EmpId}", content);

            // Return true if the request was successful
            return response.IsSuccessStatusCode;
        }
        [HttpGet]
        [Route("Home/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View("DeleteEmpView", employee);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [Route("Home/DeleteConfirmed/{id}")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isSuccess = await DeleteEmployeeAsync(id);
            if (isSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to delete department");
            }
            return RedirectToAction("Index");
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7012/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.DeleteAsync($"api/DeptTbls/{id}");

            return response.IsSuccessStatusCode;
            // GET: EmployeeController
            public ActionResult Index()
        {
            return View();
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
