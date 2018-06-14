using System;
using Moq;
using NUnit.Framework;

namespace TaskRunner.Tests
{
    [TestFixture]
    public class TaskWorkItemItemTest
    {
        [Test]
        public void ShouldExecuteDelegate()
        {
            //arrange
            var actionMock=new Mock<Action>();
            var task=new TaskWorkItem(actionMock.Object);
           
            //action
            task.Run();

            //assert
            actionMock.Verify(a=>a(),Times.Once);
        }

        [Test]
        public void ShouldInvokeTaskFinished()
        {
            //arrange 
            var task = new TaskWorkItem(()=>{});
            var callbackMock=new Mock<Action<ITaskWorkItem>>();
            task.TaskCompleted += callbackMock.Object;

            //action 
            task.Run();

            //assert
            callbackMock.Verify(c=>c(It.IsAny<ITaskWorkItem>()),Times.Once);
        }

        [Test]
        public void ShouldInvokeTaskRunning()
        {
            //arrange 
            var task = new TaskWorkItem(() => { });
            var callbackMock = new Mock<Action<ITaskWorkItem>>();
            task.TaskRunning += callbackMock.Object;

            //action 
            task.Run();

            //assert
            callbackMock.Verify(c => c(It.IsAny<ITaskWorkItem>()), Times.Once);
        }

        [Test]
        public void ShouldInvokeTaskFaulted_WhenThrowsException()
        {
            //arrange 
            var exception = new Exception();
            var task = new TaskWorkItem(() => throw exception);
            var callbackMock = new Mock<Action<ITaskWorkItem,Exception>>();

            task.TaskFaulted += callbackMock.Object;

            //action 
            task.Run();

            //assert
            callbackMock.Verify(c => c(It.IsAny<ITaskWorkItem>(),exception), Times.Once);
        }
    }
}