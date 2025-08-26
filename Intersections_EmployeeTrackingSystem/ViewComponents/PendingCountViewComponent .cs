using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Intersections_EmployeeTrackingSystem.ViewComponents
{
    public class PendingCountViewComponent : ViewComponent
    {
        private readonly IIntersectionChangeRequestService _changeService;

        public PendingCountViewComponent(IIntersectionChangeRequestService changeService)
        {
            _changeService = changeService;
        }

        public IViewComponentResult Invoke()
        {
            var count = _changeService.GetPending().Count;
            return View(count);
        }
    }
}
