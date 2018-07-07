using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserRepositoryLib;

namespace Account_Management.Controllers
{
    public class RoleController : Controller
    {
		// GET: Role
		RoleRepository roles = RoleRepository.Instance;
		// GET: Account
		public ActionResult Index(object str)
		{
			TempData["Roles"] = roles.GetAll();
			return View(str as object);
		}

		[HttpGet]
		public ViewResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(Role role)
		{
			if (validation(role))
			{
				ViewBag.Role = role;
				return View();
			}

			roles.Create(role);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			if (id == null) return RedirectToAction("Index");

			var existingRole = roles.GetById((int)id);

			if (existingRole == null) return RedirectToAction("Index");

			ViewBag.Role = existingRole;

			return View("Create");
		}

		[HttpPost]
		public ActionResult Edit(Role role)
		{
			if (validation(role))
			{
				ViewBag.Role = role;
				return View("Create");
			}
			roles.Edit((int)role.Id, role);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult Remove(Role role)
		{
			foreach (User item in UserRepository.Instance.GetAll())
			{
				if (item.RoleId == role.Id)
				{
					TempData["Roles"] = roles.GetAll();
					ViewBag.Error = "You can not delete the assigned role!";
					return View("Index");
				}
			}
			roles.Remove(roles.GetById((int)role.Id));
			return RedirectToAction("Index");
		}

		bool validation(Role role)
		{
			return (string.IsNullOrWhiteSpace(role.Name) || roles.GetAll().Where(r => r.Name.ToLower() == role.Name.ToLower()).Count() > 0);
		}
	}
}