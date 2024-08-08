using DomainLayer.DTOs.Role;
using DomainLayer.MainInterfaces;
using DomainServices.Exception;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IPermissionRepository _permissionRepository;
        public RoleController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        public IActionResult Index()
        {
            var roleList = _permissionRepository.GetRoles();
            return View(roleList);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRole(CreateRoleViewModel createRole)
        {
            int roleId = _permissionRepository.CreateRole(createRole);
            return RedirectToAction("Index", "Role", new {area="Admin"});
        }

        [HttpGet]
        public IActionResult EditRole(int roleId)
        {
            if (roleId == 0)
            {
                var error = ServiceException.Create(
                    type: "NotFound",
                    title: "شناسه موجود نمیباشد.",
                    detail: "شناسه مورد نظر برای بارگذاری اطلاعات نقش یافت نشد.");
                ViewBag.error = error.Detail;

                return Redirect("/Admin/Role/Index/");
            }
            try
            {
                var roleModel = _permissionRepository.ShowRoleForEditMode(roleId);
                return View(roleModel);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام انجام عملیات خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Role/Index/");
            }
        }

        [HttpPost]
        public IActionResult EditRole(CreateRoleViewModel createRole)
        {
            try
            {
                _permissionRepository.EditRole(createRole);
                return RedirectToAction("Index", "Role", new {area = "Admin"});
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام انجام عملیات خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Role/Index/");
            }
        }

        public IActionResult DeleteRole(int roleId)
        {
            _permissionRepository.DeleteRole(roleId);
            return RedirectToAction("Index", "Role", new { area = "Admin" });
        }
    }
}
