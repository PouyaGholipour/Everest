using DomainLayer.DTOs.Report;
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
    public interface IReportService : IRepository<Report>
    {
        // Definition private function model
        ReportListViewModel GetPagedList(int pageId, string reportTitleFilter);
        Task AddReport(AddReportViewModel addReport);
        Task<EditReportViewModel> GetReportForShowEditMode(int id);
        Task<ServiceException> EditReport (EditReportViewModel editReport);
        ServiceException RemoveReport(int id);
        Task<List<GetReportListViewModel>> GetReportList(int take);
        Task<GetReportListViewModel> GetReportlDetail(int reportId);
    }
}
