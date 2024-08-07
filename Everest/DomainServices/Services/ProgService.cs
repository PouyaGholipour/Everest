using DomainLayer.DTOs.Prog;
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
    public class ProgService : Repository<Prog>, IProgService
    {
        private readonly EverestDataBaseContext _context;
        public IUnitOfWork _unitOfWork { get; }
        public ProgService(EverestDataBaseContext context
                            , IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)context);
            _unitOfWork = unitOfWork;
        }

        public ProgListViewModel GetPagedList(int pageId, string progTitleFilter)
        {
            IQueryable<Prog> result = _context.Progs;

            if (!string.IsNullOrEmpty(progTitleFilter))
                result = result.Where(x => x.Title.Contains(progTitleFilter));

            double take = 10;
            double skip = (pageId - 1) * take;

            ProgListViewModel progList = new ProgListViewModel();
            progList.CurrentPage = pageId;
            var pageCount = (Math.Ceiling(result.Count() / take));
            progList.PageCount = Convert.ToInt32(pageCount);
            progList.Progs = result.OrderBy(x => x.DateOfHolding).Skip(Convert.ToInt32(skip)).Take(Convert.ToInt32(take)).ToList();

            return progList;
        }

        public async Task AddProg(AddProgViewModel addProg)
        {
            Prog prog = new Prog();
            prog.Title = addProg.Title;
            prog.Description = addProg.Description;
            prog.CategoryId = 2;
            prog.ProgramType = addProg.ProgramType;
            prog.Price = decimal.Parse(addProg.Price);
            prog.Place = addProg.Place;
            prog.DateOfHolding = DateTime.Parse(addProg.DateOfHolding);
            prog.LeaderName = addProg.LeaderName;
            prog.PrerequisiteCourse = addProg.PrerequisiteCourse;
            prog.PrerequisitePrograms = addProg.PrerequisitePrograms;
            prog.WhichProgramPrerequisites = addProg.WhichProgramPrerequisites;
            prog.Status = addProg.Status;

            #region Save Avatar

            if (addProg.ImageName != null)
            {
                string imagePath = "";
                prog.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(addProg.ImageName.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProgImage", prog.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    addProg.ImageName.CopyTo(stream);
                }
                prog.ImagePath = imagePath;
            }

            #endregion

            await CreateAsync(prog);
            await _unitOfWork.CommitAsync();
        }

        public async Task<EditProgViewModel> GetProgForShowEditMode(int progId)
        {
            var prog = await GetAsync(p => p.Id == progId);
            var editProgViewModel = new EditProgViewModel()
            {
                Title = prog.Title,
                Description = prog.Description,
                LeaderName = prog.LeaderName,
                Place = prog.Place,
                WhichProgramPrerequisites = prog.WhichProgramPrerequisites,
                PrerequisiteCourse = prog.PrerequisiteCourse,
                PrerequisitePrograms = prog.PrerequisitePrograms,
                ProgImage = prog.ImageName,
                Price = prog.Price.ToString(),
                Status = prog.Status,
                DateOfHolding = prog.DateOfHolding.ToString("yyyy-MM-dd"),
                ProgramType = prog.ProgramType
            };

            return editProgViewModel;
        }

        public async Task<ServiceException> EditProg(EditProgViewModel editProg)
        {
            var program = await GetAsync(p => p.Id == editProg.ProgId);

            if(program == null)
                return ServiceException.Create(
                    type: "NotFound",
                    title: "موجودیت یافت نشد.",
                    detail: "برنامه ای با این شناسه موجودیت یافت نشد.");

            program.Title = editProg.Title;
            program.Description = editProg.Description;
            program.LeaderName = editProg.LeaderName;
            program.Place = editProg.Place;
            program.WhichProgramPrerequisites = editProg.WhichProgramPrerequisites;
            program.PrerequisiteCourse= editProg.PrerequisiteCourse;
            program.PrerequisitePrograms = editProg.PrerequisitePrograms;
            program.Price = decimal.Parse(editProg.Price);
            program.Status = editProg.Status;
            program.DateOfHolding = DateTime.Parse(editProg.DateOfHolding);
            program.ProgramType = editProg.ProgramType;

            #region Save Avatar

            if (editProg.ImageName != null)
            {
                string imagePath = "";
                program.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(editProg.ImageName.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProgImage", program.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editProg.ImageName.CopyTo(stream);
                }
                program.ImagePath = imagePath;
            }

            #endregion

            await UpdateAsync(program);
            await _unitOfWork.CommitAsync();

            return ServiceException.Create(
                    type: "Success",
                    title: "عملیات موفق.",
                    detail: "عملیات ویرایش برنامه با موفقیت انجام شد.");
        }

        public ServiceException RemoveProg(int progId)
        {
            var program = GetById(progId);

            if(program == null)
                return ServiceException.Create(
                    type: "NotFound",
                    title: "عملیات ناموفق.",
                    detail: "عملیات حذف برنامه با شکست مواجه شد.");

            program.IsDelete = true;
            _unitOfWork.Commit();

            return ServiceException.Create(
                    type: "Success",
                    title: "عملیات موفق.",
                    detail: "عملیات حذف برنامه با موفقیت انجام شد.");
        }

        public async Task<List<HeldProgListViewModel>> GetHeldProgList(int take)
        {
            IQueryable<Prog> courses = await GetAllAsyncQuery();

            var courseList = courses.OrderByDescending(c => c.DateOfHolding)
               .Take(take).Select(c => new HeldProgListViewModel
               {
                   Id = c.Id,
                   Title = c.Title,
                   DateOfHolding = c.DateOfHolding,
                   Image = c.ImageName,
                   Place = c.Place,
                   Status = c.Status,
               }).ToList();

            return courseList;
        }

        public async Task<ProgDetailViewModel> GetProgDetail(int id)
        {
            var prog = await GetByIdAsync(id);

            ProgDetailViewModel progDetail = new ProgDetailViewModel()
            {
                Id = prog.Id,
                Title=prog.Title,
                Description = prog.Description,
                HoldingDate = prog.DateOfHolding.ToString("yyyy-MM-dd"),
                Price = prog.Price,
                WhichProgramPrerequisites = prog.WhichProgramPrerequisites,
                PrerequisiteCourse = prog.PrerequisiteCourse,
                PrerequisitePrograms = prog.PrerequisitePrograms,
                Place = prog.Place,
                LeaderName = prog.LeaderName,
                Image = prog.ImageName
            };

            switch (prog.ProgramType)
            {
                case DomainLayer.Enums.ProgramType.Easy:
                    progDetail.ProgramType = "آسان";
                    break;
                case DomainLayer.Enums.ProgramType.Medium:
                    progDetail.ProgramType = "متوسط";
                    break;
                case DomainLayer.Enums.ProgramType.Hard:
                    progDetail.ProgramType = "سخت";
                    break;
                default:
                    progDetail.ProgramType = "آسان";
                    break;
            }

            return progDetail;
        }
    }
}
