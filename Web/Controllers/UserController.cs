using Business.Interface.Abstract;
using DataAccess;
using Entitiy.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public UserController() { }
        // GET: User
        public async Task<ActionResult> Index()
        {
            var users = await _userService.GetUsers();
            return View(users);
        }
        
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            if(ModelState.IsValid)
            {
                _userService.AddUser(user);
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<ActionResult> Edit(int Id)
        {
            var user = await _userService.GetUserById(Id);
            if(user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _userService.UpdateUser(user);
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Delete(int Id)
        {
            if (ModelState.IsValid)
            {
                _userService.DeleteUser(Id);
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Details(int Id)
        {
            if (ModelState.IsValid)
            {
                _userService.GetUserById(Id);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}