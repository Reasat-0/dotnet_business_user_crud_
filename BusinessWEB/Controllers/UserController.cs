using BusinessWEB.Data;
using Microsoft.AspNetCore.Mvc;
using BusinessWEB.Models;
using System.Data;
using ClosedXML.Excel;

namespace BusinessWEB.Controllers
{
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db) { 
        
            _db = db;
        
        }
        public IActionResult Index()
        {
            IEnumerable<User> usersList = _db.Users;
            return View(usersList);
        }


        public IActionResult Create()
        {
            return View();
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User obj)


        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "User added successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Update(int? id) {
            if (id == null || id == 0) {
                return NotFound();
            }
            var gotUser = _db.Users.Find(id);

            if (gotUser == null) {
                return NotFound();
            }

            
            return View(gotUser);
        
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(User obj)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "User updated successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id) {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var gotUser = _db.Users.Find(id);

            
            if (gotUser == null) {
                return NotFound();
            }

            _db.Users.Remove(gotUser);
            _db.SaveChanges();
            TempData["success"] = "User deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Export()
        {

            DataTable dt = new DataTable("Grid");
            var excelColumns = new DataColumn[6] 
            {  new DataColumn("Id"),
               new DataColumn("Name"),
               new DataColumn("Email"),
               new DataColumn("Phone"),
               new DataColumn("Address"),
               new DataColumn("Created At") 
            };
            dt.Columns.AddRange(excelColumns);

            var categories = _db.Users;

            foreach (var item in categories)
            {
                dt.Rows.Add(item.Id, item.Name, item.Email, item.Phone, item.Address, item.CreatedAt);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(
                        stream.ToArray(), 
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                        "UserReport.xlsx");
                }
            }


            /*return RedirectToAction("Index");*/
        }

    }
}
