using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Moq;
using Sentry.Protocol;
using SQLitePCL;
using UserManagementTestApp.Models;
using Xunit;
using xUnitTestProject;
using ZPool.Models;
using ZPool.Pages.Administration;

namespace xUnitTestProject1
{
    public class AdminPagesTextClass : ZPoolTestBase
    {
        private Mock<IUserStore<AppUser>> _store;
        private Mock<UserManager<AppUser>> _userManager;
        private UserManager<AppUser> _mgr;
        private PasswordHasher<AppUser> hasher = new PasswordHasher<AppUser>();

        public AdminPagesTextClass()
        {
            _store = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(_store.Object, null, hasher, null, null, null, null, null, null);

            //_mgr = new UserManager<AppUser>(_store.Object, null, hasher, null, null, null, null, null, null);
            _mgr = _userManager.Object;
            _mgr.UserValidators.Add(new UserValidator<AppUser>());
            _mgr.PasswordValidators.Add(new PasswordValidator<AppUser>());

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

            Assert.Equal(message, pageModel.StatusMessage);
            Assert.NotNull(pageModel.Users);
        }

        [Fact]
        public async Task AdminEditUserPage_Routing_Test()
        {
            var pageModel = new EditUserModel(_mgr);

            var result = pageModel.OnGetAsync(1, "");

            Assert.NotNull(pageModel.UserId);
            Assert.Equal(1, pageModel.UserId);
        }



    }
}
