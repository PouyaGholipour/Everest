using DomainLayer.DTOs.User;
using DomainLayer.MainInterfaces;
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
            var userListModel = await _userService.GetUserList(pageId, userNameFilter, emailFilter);
            return View(userListModel);
        }

        [Route("/Admin/CreateUser")]
        public async Task<IActionResult> CreateUser()
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
            List<int> Ids = ViewData["Roles"] as List<int>;
            try
            {
                var userId = _userService.CreateUserFromAdmin(createUser, SelectedRoles).Result;

                _permissionRepository.AddRoleToUser(SelectedRoles, userId);
                return Redirect("/Admin/Admin/GetPagedList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام ایجاد کاربر خطایی روی داد. لطفا دوباره تلاش کنید.");
                
                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

                return View(createUser);
            }
            
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _userService.DeleteUser(id);
                return Redirect("/Admin/Admin/GetPagedList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام حذف کاربر خطایی روی داد. لطفا دوباره تلاش کنید.");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

                return Redirect("/Admin/Admin/GetPagedList");
            }
            
        }
        [Route("/Admin/EditUser/{id?}")]
        public async Task<IActionResult> EditUser(int id)
        {
            var userViewModel = await _userService.GetUserForShowEditMode(id);
            ViewData["Roles"] = _permissionService.GetRoles();

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
                await _userService.EditUserFromAdmin(editUserViewModel);
                _permissionRepository.EditUserRole(editUserViewModel.UserId, SelectedRoles);

                await _unitOfWork.CommitAsync();
                return Redirect("/Admin/Admin/GetPagedList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "هنگام ویرایش کاربر خطایی روی داد. لطفا دوباره تلاش کنید.");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

                return Redirect("/Admin/Admin/GetPagedList");
            }
        }
    }
}
