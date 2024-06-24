using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServerContainer;
using ServerContainer.Controllers;
using ServerContainer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using Microsoft.Owin;
namespace ServerContainer.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
      

        private Mock<ControllerContext> _controllerContextMock;
        private HomeController _controller;
        private string _name = "testSite";
        private string _subject = "PHP";
        private string expectedPath = @"D:\vs projects\ServerContainer\App_Data\Projects\admin_ispi\2faecc8f-091e-4303-a1f1-ea88f39e835a_chinalang\Dockerfile";
        [TestInitialize]
        public void Setup()
        {
            _controllerContextMock = new Mock<ControllerContext>();
            _controllerContextMock.SetupGet(p => p.HttpContext.User.Identity.Name).Returns("admin_ispi");

            _controller = new HomeController
            {
                ControllerContext = _controllerContextMock.Object
            };
        }

        [TestMethod]
        public async Task RunMySite_ReturnsRedirectToRouteResult_WhenCalled()
        {
            // Act
            var result = await _controller.RunMySite(_name, _subject);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public async Task RunMySite_CopiesDockerfile_WhenSubjectIsPHP()
        {
            // Arrange
            _subject = "NodeJS";

            // Act
            var result = await _controller.RunMySite(_name, _subject);

            // Assert
            Assert.IsTrue(File.Exists(expectedPath), @"D:\vs projects\ServerContainer\App_Data\Projects\admin_ispi\2faecc8f-091e-4303-a1f1-ea88f39e835a_chinalang\Dockerfile");
        }

        [TestMethod]
        public void Index_ReturnsNotNull()
        {
            // Arrange
           // var controller = new YourController();  // Замените на название вашего контроллера

           

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Tasks_Returns()
        {
            // Arrange
            // var controller = new YourController();  // Замените на название вашего контроллера



            // Act
            var result = _controller.Tasks() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MySite_Returns()
        {
            // Arrange
            // var controller = new YourController();  // Замените на название вашего контроллера



            // Act
            var result = _controller.MySite() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DownloadTask()
        {
            // Arrange
            // var controller = new YourController();  // Замените на название вашего контроллера



            // Act
            var result = _controller.DownloadTask(@"Задание картинка.jpg") ;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
