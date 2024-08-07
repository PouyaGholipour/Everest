using DomainLayer.DTOs.Journal;
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
    public interface IJournalService : IRepository<Journal>
    {
        // Definition private function model

        JornalListViewModel GetPagedList(int pageId, string journalTitleFilter);
        Task AddJournal(AddJournalViewModel addJournal);
        Task<EditJournalViewModel> GetJournalForShowEditMode(int journalId);
        Task<ServiceException> EditJournal(EditJournalViewModel editJournal);
        ServiceException RemoveJournal(int journalId);
        Task<List<GetJournalListViewModel>> GetJournaList(int take);
        Task<GetJournalListViewModel> GetJournalDetail(int journalId);
    }
}
