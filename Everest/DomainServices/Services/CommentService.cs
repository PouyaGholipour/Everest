using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
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
    public class CommentService : Repository<Comment>, ICommentService
    {
        private readonly EverestDataBaseContext _context;
        public IUnitOfWork _unitOfWork { get; }
        public CommentService(EverestDataBaseContext context
                            , IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)context);
        }
    }
}
