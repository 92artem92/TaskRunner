using Moq;
using NUnit.Framework;
using System;
using System.Threading;

namespace TaskRunner.Tests
{
    [TestFixture]
    public class TaskRunnerTest
    {
        [Test]
        public void ShouldRunTask()
        {
            //arrange 
            var taskRunner = TaskRunner.Instance;
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
            var taskRunner = TaskRunner.Instance;
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

        [Test]
        public void ShouldExecutesAllTasks()
        {
            //arrange 
            var taskRunner = TaskRunner.Instance;
            var taskMock = new Mock<ITask>();
            var expectedTaskCount = 100;
            var actualCompletedTaskCount = 0;
            taskMock.Setup(t => t.Run()).Raises(t => expectedTaskCount++);
           
            //action 
            for (var i=0; i < expectedTaskCount; i++)
            {
                taskRunner.AddTask(taskMock.Object);
            }

            //assert
            Thread.Sleep(0);
        }
    }
}
