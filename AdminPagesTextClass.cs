using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Sentry.Protocol;
using SQLitePCL;
using Xunit;
using xUnitTestProject;
using ZPool.Models;
using ZPool.Pages.Administration;
using ZPool.Services.EFServices;

namespace xUnitTestProject1
{
    public class AdminPagesTextClass : ZPoolTestBase
    {
        private Mock<UserManager<AppUser>> _userManager;
        private UserManager<AppUser> _mgr;
        
        public AdminPagesTextClass()
        {
            _userManager = new Mock<UserManager<AppUser>>(new Mock<IUserStore<AppUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<AppUser>>().Object,
                new IUserValidator<AppUser>[0],
                new IPasswordValidator<AppUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AppUser>>>().Object);

            _mgr = _userManager.Object;
        }

        [Fact]
        public void AdminUserAdmin_OnGetResult_Test()
        {
            // Arrange
            var pageModel = new UserAdministrationModel(_mgr);

            var result = pageModel.OnGetAsync(string.Empty);

            Assert.IsType<Task<IActionResult>>(result);
        }

        [Fact]
        public async Task AdminUserAdmin_OnGetResult_WithMessage_Test()
        {
            // Arrange
            var pageModel = new UserAdministrationModel(_mgr);
            string message = "This is a test message";

            await pageModel.OnGetAsync(message);
            var result = await pageModel.OnGetAsync(message);

            Assert.Equal(message, pageModel.StatusMessage);
            Assert.NotNull(pageModel.Users);
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task AdminEditUserPage_Routing_Test()
        {
            var pageModel = new EditUserModel(_mgr);

            var result = pageModel.OnGetAsync(1, "");

            Assert.Equal(1, pageModel.UserId);
        }

    }
}
