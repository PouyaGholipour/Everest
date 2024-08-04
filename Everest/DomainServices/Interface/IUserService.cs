using DomainLayer.DTOs.Course;
using DomainLayer.DTOs.User;
using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.MainInterfaces;
using DomainServices.Exception;
using InfrastructureLayer.MainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Interface
{
    public interface IUserService : IRepository<User>
    {
        //// Definition private function model
        Task<ServiceException> AddUser(RegisterViewModel register);
        int AddUser(User user);
        Task<EditUserViewModel> GetUserForShowEditMode(int id);
        ServiceException DeleteUser(int id);
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        Task<ServiceException> LoginUser(LoginViewModel login);
        void DeleteFromDataBase(int id);
        bool ActiveAccount(string activeCode);
        Task<bool> ResetPassword(ResetPasswordViewModel reset);
        Task<bool> ForgotPasswordService(ForgotPasswordViewModel reset);
        Task<UserListViewModel> GetUserList(int pageId = 1, string userNameFilter = "", string emailFilter = "");
        Task<int> CreateUserFromAdmin(CreateUserViewModel createUser, List<int> SelectedRole);
        Task<ServiceException> EditUserFromAdmin(EditUserViewModel editUser);
        Task<EditUserInformationViewModel> EditUserInformationGet(string userName);
        Task<ServiceException> EditUserInformationPost(EditUserInformationViewModel editUser, string name);
        Task<List<GetCourseForUserViewModel>> GetUserCourses(string userName);
        Task<List<GetProgForUserViewModel>> GetUserProgs(string userName);
    }
}
