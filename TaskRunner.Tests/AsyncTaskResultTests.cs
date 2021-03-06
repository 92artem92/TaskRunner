﻿using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace TaskRunner.Tests
{
    [TestFixture]
    public class AsyncTaskResultTests
    {
        [Test]
        public async Task ShouldReturnTaskResult()
        {
            //arrange
            const string expectedResult = "task result";
            var funcMock=new Mock<Func<string>>();
            var task = new AsyncTaskResult<string>(funcMock.Object);
            funcMock.Setup(f => f()).Returns(expectedResult);

            //action
            task.Run();
            var actualResult =await task.GetResultAsync();

            //assert
            Assert.That(actualResult,Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetResultAsyncShouldThrowExceptionWhenFuncThrowsToo()
        {
            //arrange
            var exception=new NullReferenceException();
            var funcMock = new Mock<Func<string>>();
            var task = new AsyncTaskResult<string>(funcMock.Object);
            funcMock.Setup(f => f()).Throws(exception);
            
            //action
            task.Run();
            async Task GetResult()
            {
                await task.GetResultAsync();
            }

            //assert
            Assert.ThrowsAsync<NullReferenceException>(GetResult);
        }
    }
}