using DomainLayer.DTOs.Prog;
using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
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
        Task AddProg(AddProgViewModel addProg);
        Task<EditProgViewModel> GetProgForShowEditMode(int progId);
        Task EditProg(EditProgViewModel editProg);
        void RemoveProg(int progId);
    }
}
