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
            var task=new ActionTask(actionMock.Object);
           
            //action
            task.Run();

            //assert
            actionMock.Verify(a=>a(),Times.Once);
        }

        [Test]
        public void ShouldInvokeTaskFinished()
        {
            //arrange 
            var task = new ActionTask(()=>{});
            var callbackMock=new Mock<Action<ITask>>();
            task.Success += callbackMock.Object;

            //action 
            task.Run();

            //assert
            callbackMock.Verify(c=>c(It.IsAny<ITask>()),Times.Once);
        }

        [Test]
        public void ShouldInvokeTaskRunning()
        {
            //arrange 
            var task = new ActionTask(() => { });
            var callbackMock = new Mock<Action<ITask>>();
            task.Running += callbackMock.Object;

            //action 
            task.Run();

            //assert
            callbackMock.Verify(c => c(It.IsAny<ITask>()), Times.Once);
        }

        [Test]
        public void ShouldInvokeTaskFaulted_WhenThrowsException()
        {
            //arrange 
            var exception = new Exception();
            var task = new ActionTask(() => throw exception);
            var callbackMock = new Mock<Action<ITask,Exception>>();

            task.Faulted += callbackMock.Object;

            //action 
            task.Run();

            //assert
            callbackMock.Verify(c => c(It.IsAny<ITask>(),exception), Times.Once);
        }
    }
}