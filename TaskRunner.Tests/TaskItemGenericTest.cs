using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace TaskRunner.Tests
{
    [TestFixture]
    public class TaskItemGenericTest
    {
        [Test]
        public async Task ShouldReturnTaskResult()
        {
            //arrange
            const string expectedResult = "task result";
            var funcMock=new Mock<Func<string>>();

            var task = new TaskWorkItem<string>(funcMock.Object);
            funcMock.Setup(f => f()).Returns(expectedResult);
            //action
            task.Run();
            var actualResult =await task.GetResultAsync();
            //assert
            Assert.That(actualResult,Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetResultAsync_ShouldThrowException_WhenFuncThrowsToo()
        {
            //arrange
            var exception=new NullReferenceException();
            var funcMock = new Mock<Func<string>>();

            var task = new TaskWorkItem<string>(funcMock.Object);
            funcMock.Setup(f => f()).Throws(exception);
            //action

            task.Run();

            async Task GetResult()
            {
                var actualResult = await task.GetResultAsync();
            }

            //assert
            Assert.ThrowsAsync<NullReferenceException>(GetResult);

        }

    }
}