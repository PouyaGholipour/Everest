using DomainLayer.DTOs.Prog;
using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
using DomainServices.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Interface
{
    public interface IProgService : IRepository<Prog>
    {
        // Definition private function model
        ProgListViewModel GetPagedList(int pageId, string progTitleFilter);
        Task<List<HeldProgListViewModel>> GetHeldProgList(int take);
        Task<ProgDetailViewModel> GetProgDetail(int id);
        Task AddProg(AddProgViewModel addProg);
        Task<EditProgViewModel> GetProgForShowEditMode(int progId);
        Task<ServiceException> EditProg(EditProgViewModel editProg);
        ServiceException RemoveProg(int progId);
    }
}
