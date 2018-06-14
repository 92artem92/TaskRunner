using System;
using System.Net;
using Moq;
using NUnit.Framework;

namespace TaskRunner.Tests
{
    [TestFixture]
    public class TaskRunnerTest
    {
        [Test]
        public void ShouldBeInOneInstance()
        {
            //arrange 
            //action
            var taskRunner1 = TaskRunner.GetTaskRunner();
            var taskRunner2 = TaskRunner.GetTaskRunner();
            //assert 
            Assert.That(taskRunner1,Is.EqualTo(taskRunner2));
        }

        [Test]
        public void ShouldRunTask()
        {
            //arrange 
            var taskRunner = TaskRunner.GetTaskRunner();
            var taskMock = new Mock<ITask>();
            taskMock.Setup(t => t.Run()).Callback(AssertTest);

            //action 
            taskRunner.AddTask(taskMock.Object);

            //assert
            void AssertTest()
            {
                Assert.Pass();
            }
        }


        [Test]
        public void WhenPreviousTaskThrowException_NextTaskShouldBeExcuted()
        {
            //arrange 
            var taskRunner = TaskRunner.GetTaskRunner();
            var taskMock = new Mock<ITask>();
            var secondTaskMock = new Mock<ITask>();
            taskMock.Setup(t => t.Run()).Throws<Exception>();
            secondTaskMock.Setup((t) => t.Run()).Callback(AssertTest);
            //action 
            taskRunner.AddTask(taskMock.Object);

            //assert
            void AssertTest()
            {
                Assert.Pass();
            }
        }

    }
}
