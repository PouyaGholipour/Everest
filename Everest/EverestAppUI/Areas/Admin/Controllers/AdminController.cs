using DomainLayer.DTOs.User;
using DomainLayer.MainInterfaces;
using DomainServices.Exception;
using DomainServices.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AdminController(IUserService userService,
                               IPermissionService permissionService,
                               IPermissionRepository permissionRepository,
                               IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _permissionService = permissionService;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetPagedList(int pageId = 1, string userNameFilter = "", string emailFilter = "")
        {
            try
            {
                var userListModel = await _userService.GetUserList(pageId, userNameFilter, emailFilter);
                return View(userListModel);
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام بارگذاری لیست کاربران خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Admin/Index/");
            }
        }

        [Route("/Admin/CreateUser")]
        public IActionResult CreateUser()
        {
            ViewData["Roles"] = _permissionService.GetAll();
            return View();
        }

        [HttpPost]
        [Route("/Admin/CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserViewModel createUser, List<int> SelectedRoles)
        {
            if(!ModelState.IsValid)
                return View(createUser);
            ViewData["Roles"] = SelectedRoles;
            try
            {
                var userId = await _userService.CreateUserFromAdmin(createUser, SelectedRoles);
                _permissionRepository.AddRoleToUser(SelectedRoles, userId);
                return Redirect("/Admin/Admin/GetPagedList");
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام افزودن کاربر جدید خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return View(createUser);
            }
            
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var result = _userService.DeleteUser(id);

                if (result.Type == "NotFound")
                    ViewBag.NotFound = result.Detail;

                return Redirect("/Admin/Admin/GetPagedList");
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام حذف کاربر خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Admin/GetPagedList");
            }
            
        }
        [Route("/Admin/EditUser/{id?}")]
        public async Task<IActionResult> EditUser(int id)
        {
            if (id == 0)
            { 
                var error = ServiceException.Create(
                    type: "NotFound",
                    title: "شناسه موجود نمیباشد.",
                    detail: "شناسه مورد نظر برای ویرایش کاربر یافت نشد.");
                ViewBag.error = error.Detail;

                return Redirect("/Admin/Admin/GetPagedList");
            }

            var userViewModel = await _userService.GetUserForShowEditMode(id);
            ViewData["Roles"] = _permissionRepository.GetRoleList();

            return View(userViewModel);
        }

        [HttpPost]
        [Route("/Admin/EditUser/{id?}")]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel, List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
                return View(editUserViewModel);

            try
            {
                var message = await _userService.EditUserFromAdmin(editUserViewModel);

                if(message.Type == "NotFound")
                    TempData["ServiceMessage"] = $"{message.Title} {System.Environment.NewLine} {message.Detail}";

                _permissionRepository.EditUserRole(editUserViewModel.UserId, SelectedRoles);

                await _unitOfWork.CommitAsync();
                return Redirect("/Admin/Admin/GetPagedList");
            }
            catch (ServiceException exception)
            {
                exception = ServiceException.Create(
                    type: "OperationFailed",
                    title: "خطا در انجام عملیات",
                    detail: "هنگام ویرایش کاربر خطایی روی داد. لطفا دوباره تلاش کنید.");

                ViewBag.error = $"{exception.Title} {System.Environment.NewLine} {exception.Detail}";

                if (exception.InnerException != null)
                {
                    ViewBag.error += "" + exception.InnerException.Message;
                }

                return Redirect("/Admin/Admin/GetPagedList");
            }
        }
    }
}
