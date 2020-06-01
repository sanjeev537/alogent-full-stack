using System;
using Assessment.Web.Controllers;
using Assessment.Web.Models;
using Moq;
using NUnit.Framework;

namespace Assessment.Web.Tests
{
    [TestFixture]
    class BoardsControllerTests
    {
        //private readonly Mock<IBoardRepository> boardRepo;// = new Mock<IBoardRepository>();
        //private readonly BoardsController controller;// = new BoardsController(boardRepo.Object);

        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void Constructor_CreatesController()
        {
            var boardRepo = Mock.Of<IBoardRepository>();
            var controller = new BoardsController(boardRepo);
            Assert.NotNull(controller);
        }

        [Test]
        public void GetAll_DoesLookupThroughRepository()
        {
            var boardRepo = new Mock<IBoardRepository>();
            var controller = new BoardsController(boardRepo.Object);

            controller.GetAll();

            boardRepo.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void Find_NegativeId_ThrowsOutOfRangeException()
        {
            var boardRepo = Mock.Of<IBoardRepository>();
            var controller = new BoardsController(boardRepo);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                controller.Find(-1);
            });
        }

        [Test]
        public void Find_ZeroId_ThrowsOutOfRangeException()
        {
            var boardRepo = Mock.Of<IBoardRepository>();
            var controller = new BoardsController(boardRepo);
            Assert.Throws<ArgumentOutOfRangeException>(() => 
            {
                controller.Find(0);
            });
        }

        [Test]
        public void Find_ValidId_DoesLookupThroughRepository()
        {
            var boardRepo = new Mock<IBoardRepository>();
            boardRepo.Setup(x => x.Find(It.IsAny<int>())).Returns(new Board());

            var controller = new BoardsController(boardRepo.Object);

            controller.Find(1);

            boardRepo.Verify(x => x.Find(1), Times.Once);
        }

        [Test]
        public void Post_NegativeId_ThrowsOutOfRangeException()
        {
            var boardRepo = Mock.Of<IBoardRepository>();
            var controller = new BoardsController(boardRepo);
            var board = new Board() { Id = -1, Name="Name #-1" };

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                controller.Post(board);
            });
        }

        [Test]
        public void Post_ZeroId_ThrowsOutOfRangeException()
        {
            var boardRepo = Mock.Of<IBoardRepository>();
            var controller = new BoardsController(boardRepo);
            var board = new Board() { Id = 0,Name="Name #0" };
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                controller.Post(board);
            });
        }

        [Test]
        public void Post_EmptyName_ThrowsArgumentNullException()
        {
            var boardRepo = Mock.Of<IBoardRepository>();
            var controller = new BoardsController(boardRepo);
            var board = new Board() { Id = 1, Name = "" };
            Assert.Throws<ArgumentNullException>(() =>
            {
                controller.Post(board);
            });
        }

        [Test]
        public void Post_DuplicateId_ThrowsException()
        {
            var boardRepo = new Mock<IBoardRepository>();
            boardRepo.Setup(x => x.Add(It.IsAny<Board>())).Returns(false);

            var controller = new BoardsController(boardRepo.Object);
            var board = new Board() { Id = 3,Name="Name #3" };

            Assert.Throws<Exception>(() =>
            {
                controller.Post(board);
            });
        }

        [Test]
        public void Post_ValidId_DoesLookupThroughRepository()
        {
            var boardRepo = new Mock<IBoardRepository>();
            //boardRepo.Setup(x => x.Find(It.IsAny<int>())).Returns(new Board());
            boardRepo.Setup(x => x.Add(It.IsAny<Board>())).Returns(true);

            var board = new Board() { Id = 3,Name="Name #3" };

            var controller = new BoardsController(boardRepo.Object);

            controller.Post(board);

            boardRepo.Verify(x => x.Add(board), Times.Once);
        }
    }
}
