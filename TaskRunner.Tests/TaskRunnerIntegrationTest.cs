using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskRunner.Tests
{
    [TestFixture]
    public class TaskRunnerIntegrationTest
    {
        [Test]
        public async Task ShouldRunTask()
        {
            //arrange 
            var taskRunner = TaskRunner.Instance;
            const string expectedResult = "Test string";
            var task = new AsyncTaskResult<string>(() => expectedResult);

            //action 
            taskRunner.AddTask(task);
            var actualResult = await task.GetResultAsync();

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task WhenPreviousTaskThrowExceptionNextTaskShouldBeExcuted()
        {
            //arrange 
            var taskRunner = TaskRunner.Instance;
            var task= new AsyncTaskResult<string>(()=>throw new NullReferenceException());
            const string expectedResult = "Test string";
            var secondTask = new AsyncTaskResult<string>(() => expectedResult);
            taskRunner.AddTask(task);
            taskRunner.AddTask(secondTask);
            
            //action 
            var actualResult = await secondTask.GetResultAsync();
            
            //assert    
            Assert.That(actualResult,Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task ShouldExecutesAllTasks()
        {
            //arrange 
            var taskRunner = TaskRunner.Instance;
            const int expectedTaskCount = 100;
            var actualCompletedTaskCount = 0;
            var tasks = new Task[expectedTaskCount];

            //action 
            for (var i=0; i < expectedTaskCount; i++)
            {
                var task = new AsyncTaskResult<string>(() =>
                {
                    actualCompletedTaskCount++;
                    return "test string";
                });
                taskRunner.AddTask(task);
                tasks[i] = task.GetResultAsync();
            }
            await Task.WhenAll(tasks);
            
            //assert
            Assert.That(actualCompletedTaskCount,Is.EqualTo(expectedTaskCount));
        }
    }
}
