using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocialStuff.Data;
using SocialStuff.Model;
using SocialStuff.Services.Implementations;
using SocialStuff.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace SocialStuff.Tests.ServiceTests
{
    [TestClass]
    public class FeedServiceTests
    {
        private Mock<IRepository>? _repoMock;
        private Mock<IUserService>? _userServiceMock;
        private FeedService? _service;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IRepository>();
            _userServiceMock = new Mock<IUserService>();
            _service = new FeedService(_repoMock.Object, _userServiceMock.Object);
        }

        [TestMethod]
        public void GetFeedContent_Called_ReturnsPostsList()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post(1, "Title1", "Cat1", "Content1", DateTime.Now),
                new Post(2, "Title2", "Cat2", "Content2", DateTime.Now)
            };
            _repoMock!.Setup(r => r.GetFeedPostsList()).Returns(posts);

            // Act
            var result = _service!.GetFeedContent();

            // Assert
            Assert.AreEqual(posts, result);
            Assert.AreEqual(2, result.Count);
        }
    }
}
