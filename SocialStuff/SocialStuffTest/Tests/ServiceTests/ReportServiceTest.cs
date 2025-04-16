using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocialStuff.Data;
using SocialStuff.Model;
using SocialStuff.Services.Implementations;
using SocialStuff.Services.Interfaces;
using System.Collections.Generic;

namespace SocialStuff.Tests.ServiceTests
{
    [TestClass]
    public class ReportServiceTests
    {
        private Mock<IRepository>? _repoMock;
        private Mock<IUserService>? _userServiceMock;
        private ReportService? _service;
        private List<Report>? _reports;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IRepository>();
            _userServiceMock = new Mock<IUserService>();
            _reports = new List<Report>();
            _repoMock.Setup(r => r.GetReportsList()).Returns(_reports);
            _service = new ReportService(_repoMock.Object, _userServiceMock.Object);
        }

        [TestMethod]
        public void GetReportById_ReportExists_ReturnsReport()
        {
            // Arrange
            var report = new Report(1, 1, "Spam", "Bad message", "Pending");
            _reports!.Add(report);

            // Act
            var result = _service!.GetReportById(1);

            // Assert
            Assert.AreEqual(report, result);
        }

        [TestMethod]
        public void GetReportById_ReportDoesNotExist_ReturnsNull()
        {
            // Arrange
            // Empty reports list

            // Act
            var result = _service!.GetReportById(1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddReport_Called_AddsToReportsList()
        {
            // Arrange
            var report = new Report(1, 1, "Spam", "Bad message", "Pending");

            // Act
            _service!.AddReport(report);

            // Assert
            Assert.IsTrue(_reports!.Contains(report));
            Assert.AreEqual(1, _reports.Count);
        }

        [TestMethod]
        public void CheckIfReportExists_ReportExists_ReturnsTrue()
        {
            // Arrange
            var report = new Report(1, 1, "Spam", "Bad message", "Pending") { ReporterUserID = 2 };
            _reports!.Add(report);

            // Act
            var result = _service!.CheckIfReportExists(1, 2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfReportExists_ReportDoesNotExist_ReturnsFalse()
        {
            // Arrange
            // Empty reports list

            // Act
            var result = _service!.CheckIfReportExists(1, 2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IncreaseReportCount_UserExists_CallsIncreaseReportCount()
        {
            // Arrange
            int userId = 1;
            var user = new User(userId, "user", "123", 0);
            _userServiceMock!.Setup(u => u.GetUserById(userId)).Returns(user);

            // Act
            _service!.IncreaseReportCount(userId);

            // Assert
            _userServiceMock.Verify(u => u.GetUserById(userId), Times.Once());
            Assert.AreEqual(1, user.GetReportedCount()); // Assuming IncreaseReportCount increments
        }

        [TestMethod]
        public void IncreaseReportCount_UserDoesNotExist_DoesNotThrow()
        {
            // Arrange
            int userId = 1;
            _userServiceMock!.Setup(u => u.GetUserById(userId)).Returns((User?)null);

            // Act
            _service!.IncreaseReportCount(userId);

            // Assert
            _userServiceMock.Verify(u => u.GetUserById(userId), Times.Once());
        }

        [TestMethod]
        public void LogReportedMessages_Called_CallsAddReportForEach()
        {
            // Arrange
            var reports = new List<Report>
            {
                new Report(1, 1, "Pending", "Spam", "Bad"),
                new Report(2, 2, "Open", "Abuse", "Worse")
            };

            // Act
            _service!.LogReportedMessages(reports);

            // Assert
            _repoMock!.Verify(r => r.AddReport(1, "Spam", "Bad", "Pending"), Times.Once());
            _repoMock.Verify(r => r.AddReport(2, "Abuse", "Worse", "Open"), Times.Once());
        }

        [TestMethod]
        public void SendReport_Called_DoesNotThrow()
        {
            // Arrange
            var report = new Report(1, 1, "Spam", "Bad message", "Pending");

            // Act
            _service!.SendReport(report);

            // Assert
            Assert.IsTrue(true); // Since method is empty, ensure it doesn't throw
        }
    }
}