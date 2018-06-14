using System;
using System.Threading.Tasks;
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
            var taskMock = new Mock<ITaskItem>();
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
        public void ShouldRunAllTasks()
        {
            //arrange 
            var taskRunner = TaskRunner.GetTaskRunner();
            const int expectedTaskCount = 100;
            var actualInvokationCount = 0;
            var taskMock = new Mock<ITaskItem>();
            taskMock.Setup(t => t.Run()).Callback(AssertTest);

            for (var i = 0; i < expectedTaskCount; i++)
            {
                taskRunner.AddTask(taskMock.Object);
            }
            void AssertTest()
            {
                actualInvokationCount++;
            }
            //action 
            taskRunner.AddTask(taskMock.Object);

            //assert
            Assert.That(actualInvokationCount,Is.EqualTo(expectedTaskCount));
        }
    }

    [TestFixture]
    public class TaskItemTest
    {
        [Test]
        public void ShouldExecuteDelegate()
        {
            //arrange
            var task=new TaskItem(Action);
           
            //action
            task.Run();

            //assert
            void Action()
            {
                Assert.Pass();
            }
        }
    }

    [TestFixture]
    public class TaskItemGenericTest
    {
        [Test]
        public async Task ShouldReturnTaskResult()
        {
            //arrange
            const string expectedResult = "task result";
            var task = new TaskItem<string>(Function);
            string Function()
            {
                return expectedResult;
            }
            //action
            task.Run();
            var actualResult =await task.GetResultAsync();
            //assert
            Assert.That(actualResult,Is.EqualTo(expectedResult));
        }


    }

}
