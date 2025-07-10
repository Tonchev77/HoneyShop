namespace HoneyShop.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Web.Mvc;

    public class ShopController : BaseController
    {

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
