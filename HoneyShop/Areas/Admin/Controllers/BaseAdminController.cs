﻿namespace HoneyShop.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    using static HoneyShop.GCommon.ApplicationConstants;

    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public abstract class BaseAdminController : Controller
    {
        protected bool IsUserAuthenticated()
        {
            return this.User.Identity?.IsAuthenticated ?? false;
        }

        protected string? GetUserId()
        {
            string? userId = null;

            bool isAuthenticated = this.IsUserAuthenticated();

            if (isAuthenticated)
            {
                userId = this.User
                    .FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return userId;
        }
    }
}
