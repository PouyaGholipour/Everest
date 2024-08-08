using DomainLayer.DTOs.Report;
using DomainLayer.Entities;
using DomainLayer.MainInterfaces;
using DomainServices.Exception;
using DomainServices.Interface;
using InfrastructureLayer.ApplicationDbContext;
using InfrastructureLayer.MainServices;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Generators;

namespace DomainServices.Services
{
    public class ReportService : Repository<Report>, IReportService
    {
        private readonly EverestDataBaseContext _context;
        public IUnitOfWork _unitOfWork { get; }
        public ReportService(EverestDataBaseContext context
                            , IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = (this._context ?? (EverestDataBaseContext)context);
            _unitOfWork = unitOfWork;
        }

        public ReportListViewModel GetPagedList(int pageId, string reportTitleFilter)
        {
            IQueryable<Report> result = _context.Reports;

            if (!string.IsNullOrEmpty(reportTitleFilter))
                result = result.Where(x => x.ReportTitle.Contains(reportTitleFilter));

            double take = 10;
            double skip = (pageId - 1) * take;

            ReportListViewModel progList = new ReportListViewModel();
            progList.CurrentPage = pageId;
            var pageCount = (Math.Ceiling(result.Count() / take));
            progList.PageCount = Convert.ToInt32(pageCount);
            progList.Reports = result.OrderBy(x => x.DateOfHolding).Skip(Convert.ToInt32(skip)).Take(Convert.ToInt32(take)).ToList();

            return progList;
        }

        public async Task AddReport(AddReportViewModel addReport)
        {
            Report report = new Report()
            {
                ReportTitle = addReport.ReportTitle,
                ReportContent = addReport.ReportContent,
                Place = addReport.Place,
                DateOfHolding = DateTime.Parse(addReport.DateOfHolding),
            };

            #region Save Avatar

            if (addReport.ImageName != null)
            {
                string imagePath = "";
                report.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(addReport.ImageName.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ReportImage", report.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    addReport.ImageName.CopyTo(stream);
                }
                report.ImagePath = imagePath;
            }

            #endregion

            await CreateAsync(report);
            await _unitOfWork.CommitAsync();
        }

        public async Task<EditReportViewModel> GetReportForShowEditMode(int id)
        {
            var report = await GetAsync(r => r.Id == id);
            var viewModel = new EditReportViewModel()
            {
                ReportTitle = report.ReportTitle,
                ReportContent = report.ReportContent,
                ReportImage = report.ImageName,
                Place = report.Place,
                DateOfHolding = report.DateOfHolding.ToString("yyyy-MM-dd"),
            };
            return viewModel;
        }

        public async Task<ServiceException> EditReport(EditReportViewModel editReport)
        {
            var report = await GetAsync(r => r.Id == editReport.ReportId);

            if(report == null)
                return ServiceException.Create(
                    type: "NotFound",
                    title: "موجودیت یافت نشد.",
                    detail: "گزارشی با این شناسه موجودیت یافت نشد.");

            report = editReport.Adapt<Report>();
            report.ReportTitle = editReport.ReportTitle;
            report.ReportContent = editReport.ReportContent;
            report.Place = editReport.Place;
            report.DateOfHolding = DateTime.Parse(editReport.DateOfHolding);

            #region Save Avatar

            if (editReport.ImageName != null)
            {
                string imagePath = "";
                report.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(editReport.ImageName.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ReportImage", report.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editReport.ImageName.CopyTo(stream);
                }
                report.ImagePath = imagePath;
            }

            #endregion

            await UpdateAsync(report);
            await _unitOfWork.CommitAsync();

            return ServiceException.Create(
                    type: "Success",
                    title: "عملیات موفق",
                    detail: "عملیات ویرایش گزارش با موفقیت انجام شد.");
        }

        public ServiceException RemoveReport(int id)
        {
            var report = Get(r => r.Id == id);

            if(report == null)
                return ServiceException.Create(
                    type: "NotFound",
                    title: "موجودیت یافت نشد.",
                    detail: "گزارشی با این شناسه موجودیت یافت نشد.");

            report.IsDelete = true;
            Update(report);
            _unitOfWork.Commit();

            return ServiceException.Create(
                    type: "Success",
                    title: "عملیات موفق.",
                    detail: "عملیات حذف گزارش با موفقیت انجام شد.");
        }

        public async Task<List<GetReportListViewModel>> GetReportList(int take)
        {
            IQueryable<Report> reports = await GetAllAsyncQuery();

            var reportList = reports.OrderByDescending(c => c.DateOfHolding)
               .Take(take).Select(c => new GetReportListViewModel
               {
                   Id = c.Id,
                   Title = c.ReportTitle,
                   DateOfHolding = c.DateOfHolding.ToString("yyyy-MM-dd"),
                   Image = c.ImageName,
                   Place = c.Place

               }).ToList();

            return reportList;
        }

        public async Task<GetReportListViewModel> GetReportlDetail(int reportId)
        {
            var reportDetail = await GetByIdAsync(reportId);
            GetReportListViewModel getReport = new GetReportListViewModel()
            {
                Id = reportId,
                Title = reportDetail.ReportTitle,
                ReportContent = reportDetail.ReportContent,
                Place = reportDetail.Place,
                Image = reportDetail.ImageName,
                DateOfHolding = reportDetail.DateOfHolding.ToString("dd-MM-yyyy"),
            };
            return getReport;
        }
    }
}
