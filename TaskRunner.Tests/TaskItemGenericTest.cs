using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

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


    }
}