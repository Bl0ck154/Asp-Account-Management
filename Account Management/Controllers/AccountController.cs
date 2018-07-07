using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UserRepositoryLib;

namespace Account_Management.Controllers
{
	public class AccountController : Controller
	{
		UserRepository users = UserRepository.Instance;
		RoleRepository roles = RoleRepository.Instance;
		// GET: Account
		public ActionResult Index()
		{
			TempData["Users"] = users.GetAll();
			return View();
		}

		[HttpGet]
		public ViewResult Create()
		{
			ViewBag.RoleId = roles.GetAll().Select(role => new SelectListItem() { Text = role.Name, Value = role.Id.ToString() });
			return View();
		}

		[HttpPost]
		public ActionResult Create(User user)
		{
			if (isFieldsNotValid(user))
			{
				ViewBag.RoleId = roles.GetAll().Select(role => new SelectListItem() {
					Text = role.Name, Value = role.Id.ToString(), Selected = role.Id == user.RoleId ? true : false });
				ViewBag.User = user;
				return View();
			}

			users.Create(user);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			if (id == null) return RedirectToAction("Index");

			var existingUser = users.GetById((int)id);

			if (existingUser == null) return RedirectToAction("Index");

			ViewBag.RoleId = roles.GetAll().Select(role => new SelectListItem()
			{
				Text = role.Name,
				Value = role.Id.ToString(),
				Selected = role.Id == existingUser.RoleId ? true : false
			});
			ViewBag.User = existingUser;

			return View("Create");
		}

		[HttpPost]
		public ActionResult Edit(User user)
		{
			if (isFieldsNotValid(user))
			{
				ViewBag.User = user;
				return View("Create");
			}
			users.Edit((int)user.Id, user);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult Remove(int? id)
		{
			if (id == null) return RedirectToAction("Index");

			var existingUser = users.GetById((int)id);

			if (existingUser == null) return RedirectToAction("Index");

			ViewBag.User = existingUser;

			ViewBag.Roles = roles;

			return View("Remove");
		}

		[HttpPost]
		public ActionResult Remove(User user)
		{
			users.Remove(users.GetById((int)user.Id));
			return RedirectToAction("Index");
		}

		static string regexPattern = @"[a-zA-Z0-9-_\.]{2,}@\w+(\.\w+)+";
		bool isFieldsNotValid(User user)
		{
			return (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.Email) ||
				string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.Password) ||
				user.Login.Length < 5 || user.Password.Length < 6 || !Regex.IsMatch(user.Email, regexPattern) ||
				users.GetAll().Where(u => u.Login.ToLower() == user.Login.ToLower()).Count() >0);
		}
	}
}