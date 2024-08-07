using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
using DomainServices.Exception;
using DomainServices.Interface;
using InfrastructureLayer.ApplicationDbContext;
using InfrastructureLayer.MainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Services
{
    public class ProgUserService : Repository<ProgUser>, IProgUserService
    {
        private readonly EverestDataBaseContext _context;
        private readonly IUserRepositoy _userRepositoy;
        public IUnitOfWork _unitOfWork { get; }
        public ProgUserService(EverestDataBaseContext context,
                           IUnitOfWork unitOfWork,
                           IUserRepositoy userRepositoy) : base(context, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)context);
            _userRepositoy = userRepositoy;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceException> AddProgToUser(int userId, int progId)
        {
            if (userId == 0 || progId == 0)
                return ServiceException.Create(type: "NotFound",
                    title: "اطلاعات یافت نشد",
                    detail: ".اطلاعاتی با این شناسه ها برای انجام عملیات یافت نشد");

            var progUser = new ProgUser()
            {
                UserId = userId,
                ProgId = progId
            };

            await CreateAsync(progUser);
            return ServiceException.Create(type: "Success",
                    title: "عملیات موفق",
                    detail: "این برنامه به لیست برنامه های شما افزوده شد.");
        }

        public async Task<ServiceException> RemoveProgUser(int userId, int progId)
        {
            if (userId == 0 || progId == 0)
                return ServiceException.Create(type: "NotFound",
                    title: "اطلاعات یافت نشد",
                    detail: ".اطلاعاتی با این شناسه ها برای انجام عملیات یافت نشد");

            var userProg = await GetListAsync(x => x.UserId == userId && x.ProgId == progId);
            RemoveRange(userProg);
            return ServiceException.Create(type: "Success",
                    title: "عملیات موفق",
                    detail: "این برنامه از لیست برنامه های شما حذف شد.");
        }
    }
}
