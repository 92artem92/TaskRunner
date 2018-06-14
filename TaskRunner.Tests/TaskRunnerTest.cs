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
        public void ShouldRunTask()
        {
            //arrange 
            var taskRunner = TaskRunner.GetTaskRunner();
            var taskMock = new Mock<ITaskWorkItem>();
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
            var taskMock = new Mock<ITaskWorkItem>();
            var secondTaskMock = new Mock<ITaskWorkItem>();
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
        public void ShouldRunAllTasks()
        {
            //arrange 
            var taskRunner = TaskRunner.GetTaskRunner();
            const int taskCount = 100;
            var actualTaskCount = 0;
            var task = new TaskWorkItem(()=>{});
            task.TaskCompleted += (t) =>  
                actualTaskCount++;

            //action 
            for (var i = 0; i < taskCount; i++)
            {
                taskRunner.AddTask(task);
            }
            
            //assert
            Assert.That(actualTaskCount,Is.EqualTo(task));
        }
    }
}
