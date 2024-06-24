//using System.Threading.Tasks;
//using System.Web.Mvc;
////using LeinNeiro.AspNet.Identity;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin;
//using Moq;
//using ServerContainer;
//using ServerContainer.Controllers;
//using ServerContainer.Models;
//using Xunit;
//namespace ServerContainer.Tests.Controllers
//{
//    public class AccountControllerTests
//    {
//        [Fact]
//        public async Task Login_Post_InvalidModelState_ReturnsViewWithModel()
//        {
//            // Arrange
//            var mockUserManager = new Mock<ApplicationUserManager>(Mock.Of<IUserStore<ApplicationUser>>());
//            var mockSignInManager = new Mock<ApplicationSignInManager>(mockUserManager.Object, Mock.Of<IOwinContext>());
//            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object);
//            controller.ModelState.AddModelError("error", "test error");
//            var model = new LoginViewModel();

//            // Act
//            var result = await controller.Login(model, null);

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.Equal(model, viewResult.Model);
//        }

//        //[Fact]
//        //public async Task Login_Post_ValidModelState_ReturnsRedirectOnSuccess()
//        //{
//        //    // Arrange
//        //    var model = new LoginViewModel { Email = "Koko", Password = "Koko-1" };
//        //    var mockUserManager = new Mock<ApplicationUserManager>(Mock.Of<IUserStore<ApplicationUser>>());
//        //    var mockSignInManager = new Mock<ApplicationSignInManager>(mockUserManager.Object, Mock.Of<IOwinContext>());
//        //    mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password, false, false))
//        //        .ReturnsAsync(SignInStatus.Success);
//        //    var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object);

//        //    // Act
//        //    var result = await controller.Login(model, "returnUrl");

//        //    // Assert
//        //    var redirectResult = Assert.IsType<RedirectResult>(result);
//        //    Assert.Equal("returnUrl", redirectResult.Url);
//        //}

        
//        [Fact]
//        public async Task Login_Post_ValidModelState_ReturnsRedirectOnSuccess()
//        {
//            // Arrange
//            var model = new LoginViewModel { Email = "Koko", Password = "Koko-1" };
//           // var mockUserStore = Mock.Of<IUserStore<ApplicationUser>>();
//           // var mockUserManager = new ApplicationUserManager(mockUserStore);
//           // var mockContext = Mock.Of<IOwinContext>();
//          //  var mockSignInManager = new ApplicationSignInManager(mockUserManager, mockContext.Authentication);

//            var controller = new AccountController();

//            // Act
//            var result = await controller.Login(model, "returnUrl");

//            // Assert
//            var redirectResult = Assert.IsType<RedirectResult>(result);
//            Assert.Equal("returnUrl", redirectResult.Url);
//        }

//        // Дополнительные тесты для остальных случаев SignInStatus
//    }
//}