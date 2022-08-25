using GymApp14V1.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymApp14V1.Controllers
{
    public class CommonInfoController : Controller
    {
        public IActionResult About()
        {
            var model = new CommonInfoViewModel();

            model.PageHeader = new PageHeaderViewModel
            {
                HeadLine = "About Us",
                SubTitle = "Eleifend quam adipiscing vitae proin sagittis nisl."
            };

            return View("../CommonInfo/About", model);
        }

        public IActionResult Privacy()
        {
            var model = new CommonInfoViewModel();

            model.PageHeader = new PageHeaderViewModel
            {
                HeadLine = "Privacy",
                SubTitle = "Eleifend quam adipiscing vitae proin sagittis nisl."
            };

            return View();
        }

    }
}
