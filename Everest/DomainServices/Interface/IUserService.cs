using DomainLayer.DTOs.User;
using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.MainInterfaces;
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
        Task<ClientMessageType> AddUser(RegisterViewModel register);
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        Task<ClientMessageType> LoginUser(LoginViewModel login);
        void DeleteFromDataBase(int id);
        bool ActiveAccount(string activeCode);
        Task<bool> ResetPassword(ResetPasswordViewModel reset);
        Task<bool> ForgotPasswordService(ForgotPasswordViewModel reset);
    }
}
