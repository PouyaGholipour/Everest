using DomainLayer.DTOs.Journal;
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
using TopLearn.Core.Generators;

namespace DomainServices.Services
{
    public class JournalService :Repository<Journal>, IJournalService
    {
        private readonly EverestDataBaseContext _context;
        public IUnitOfWork _unitOfWork { get; }
        public JournalService(EverestDataBaseContext context
                            , IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)context);
            _unitOfWork = unitOfWork;
        }

        public JornalListViewModel GetPagedList(int pageId, string journalTitleFilter)
        {
            IQueryable<Journal> result = _context.Journals;

            if (!string.IsNullOrEmpty(journalTitleFilter))
                result = result.Where(x => x.JournalTitle.Contains(journalTitleFilter));

            double take = 10;
            double skip = (pageId - 1) * take;

            JornalListViewModel journalList = new JornalListViewModel();
            journalList.CurrentPage = pageId;
            var pageCount = (Math.Ceiling(result.Count() / take));
            journalList.PageCount = Convert.ToInt32(pageCount);
            journalList.Journals = result.OrderBy(x => x.JournalTitle).Skip(Convert.ToInt32(skip)).Take(Convert.ToInt32(take)).ToList();

            return journalList;
        }

        public async Task AddJournal(AddJournalViewModel addJournal)
        {
            Journal journal = new Journal();
            journal.JournalTitle = addJournal.JournalTitle;
            journal.Content = addJournal.Content;
            journal.CreateDate = DateTime.Now;

            #region Save Avatar

            if (addJournal.ImageName != null)
            {
                string imagePath = "";
                journal.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(addJournal.ImageName.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/JournalImage", journal.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    addJournal.ImageName.CopyTo(stream);
                }
                journal.ImagePath = imagePath;
            }

            #endregion

            await CreateAsync(journal);
            await _unitOfWork.CommitAsync();

        }

        public async Task<EditJournalViewModel> GetJournalForShowEditMode(int journalId)
        {
            var journal = await GetAsync(j => j.Id == journalId);
            var editJournalViewModel = new EditJournalViewModel()
            {
                JournalTitle = journal.JournalTitle,
                Content = journal.Content,
                JournalImage = journal.ImageName,
            };

            return editJournalViewModel;
        }

        public async Task<ServiceException> EditJournal(EditJournalViewModel editJournal)
        {
            var journal = await GetAsync(j => j.Id == editJournal.JournalId);
            if(journal == null)
                return ServiceException.Create(
                    type: "NotFound",
                    title: "موجودیت یافت نشد",
                    detail: "مجله ای با این شناسه یافت نشد.");

            journal.JournalTitle = editJournal.JournalTitle;
            journal.Content = editJournal.Content;

            #region Save Avatar

            if (editJournal.ImageName != null)
            {
                string imagePath = "";
                journal.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(editJournal.ImageName.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/JournalImage", journal.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editJournal.ImageName.CopyTo(stream);
                }
                journal.ImagePath = imagePath;
            }

            #endregion

            await UpdateAsync(journal);
            await _unitOfWork.CommitAsync();

            return ServiceException.Create(
                    type: "Success",
                    title: "عملیات موفق",
                    detail: "عملیات ویرایش مجله با موفقیت انجام شد.");
        }

        public ServiceException RemoveJournal(int journalId)
        {
            var journal = GetById(journalId);

            if( journal == null)
                return ServiceException.Create(
                    type: "NotFound",
                    title: "موجودیت یافت نشد",
                    detail: "مجله ای با این شناسه یافت نشد.");

            journal.IsDelete = true;
            Update(journal);
            _unitOfWork.Commit();

            return ServiceException.Create(
                    type: "Success",
                    title: "عملیات موفق",
                    detail: "عملیات حذف مجله با موفقیت انجام شد.");
        }

        public async Task<List<GetJournalListViewModel>> GetJournaList(int take)
        {
            IQueryable<Journal> journals = await GetAllAsyncQuery();

            var journalsList = journals.OrderByDescending(c => c.CreateDate)
               .Take(take).Select(c => new GetJournalListViewModel
               {
                   Id = c.Id,
                   JournalTitle = c.JournalTitle,
                   Content = c.Content,
                   JournalImage = c.ImageName

               }).ToList();

            return journalsList;
        }

        public async Task<GetJournalListViewModel> GetJournalDetail(int journalId)
        {
            var journal = await GetByIdAsync(journalId);
            GetJournalListViewModel journalDetail = new GetJournalListViewModel()
            {
                Id = journalId,
                JournalTitle = journal.JournalTitle,
                JournalImage = journal.ImageName,
                Content = journal.Content,
            };
            return journalDetail;
        }
    }
}
