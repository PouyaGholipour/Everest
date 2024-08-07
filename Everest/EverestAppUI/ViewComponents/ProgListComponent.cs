using DomainLayer.DTOs.Prog;
using DomainServices.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EverestAppUI.ViewComponents
{
    public class ProgListComponent : ViewComponent
    {
        private IProgService _progService;
        private readonly ILogger<ProgListComponent> _logger;
        public ProgListComponent(IProgService progService, ILogger<ProgListComponent> logger)
        {
            _progService = progService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var progList = await _progService.GetHeldProgList(6);
                return View("ProgList", progList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ProgListComponent: {ex.Message}");
                return View(new List<HeldProgListViewModel>());
            }
        }
    }
}
