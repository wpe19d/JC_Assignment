using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JumpCloudAssignment.Models;
using Newtonsoft.Json;
using Xunit;

namespace JumpCloudAssignment.Tests
{
    public class ActionServiceTests
    {
        private ActionService _sut;

        public ActionServiceTests()
        {
            _sut = new ActionService();
        }

        #region AddAction Tests

        [Theory(DisplayName = "AddAction Should Not Throw Error For Valid ActionInfo")]
        [InlineData("{\"action\":\"run\",\"time\":500}")]
        [InlineData("{\"action\":\"jump\",\"time\":100}")]
        [InlineData("{\"action\":\"run\",\"time\":250}")]
        public void AddActionShouldNotThrowErrorForValidAction(string action)
        {
            //Arrange
            var expectedResult = ActionMessages.SuccessMessage;

            //Act
            var result = _sut.AddAction(action);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory(DisplayName = "Add ActionInfo Should Throw Error for Invalid ActionInfo")]
        //Invalid Action: Type Walk
        [InlineData("{\"action\":\"walk\",\"time\":500}")]
        //Invalid Action: Missing Action data
        [InlineData("{\"time\":500}")]
        [InlineData("{\"action\":,\"time\":500}")]
        //Invalid Action: Missing Time data
        [InlineData("{\"action\":\"run\",}")]
        [InlineData("{\"action\":\"run\",\"time\":}")]
        //Invalid Action: Negative Time 
        [InlineData("{\"action\":\"run\",\"time\":-1}")]
        //Invalid Action: Misspelled Action
        [InlineData("{\"actoin\":\"run\",\"time\":500}")]
        //Invalid Action: Misspelled Time
        [InlineData("{\"action\":\"run\",\"tmie\":100}")]
        //Invalid Action: Missing all action data
        [InlineData("{}")]
        public void AddActionShouldThrowErrorForInvalidAction(string action)
        {
            //Arrange
            var expectedResult = $"{ActionMessages.InvalidAction}. Invalid ActionInfo: {action}";

            //Act
            var result = _sut.AddAction(action);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact(DisplayName = "Add Action Should Throw Error For Empty Input")]
        public void AddActionShouldThrowErrorForEmptyInput()
        {
            //Arrange
            var expectedResult = ActionMessages.EmptyAction;

            //Act
            var result = _sut.AddAction(string.Empty);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact(DisplayName = "Add Action Should Handle Async Requests")]
        public void AddActionShouldHandleAsynchronousRequests()
        {
            //Arrange
            var expectedResult = ActionMessages.SuccessMessage;

            List<string> results = new List<string>();
            //Act
            Parallel.For(0, 100, index =>
            {
                ActionInfo actionInfo = new ActionInfo
                {
                    Action = (index % 2 == 0) ? "jump" : "run",
                    Time = index
                };

                var json = JsonConvert.SerializeObject(actionInfo);
                var result = _sut.AddAction(json);
                results.Add(result);
            });

            //Assert
            Assert.All(results, result => Assert.Equal(expectedResult, result));
        }

        [Fact(DisplayName = "Add Action and Get Statistics Should Handle Async Requests")]
        public async Task AddActionAndGetStatisticsShouldHandleAsyncRequests()
        {
            //Arrange
            var tasks = new Task<List<string>>[4];

            for (int i = 0; i < tasks.Length - 1; i++)
            {
                var addTask = Task.Run(() =>
                {
                    var results = new List<string>();

                    for (int i = 0; i < 100; i++)
                    {
                        var actionInfo = new ActionInfo
                        {
                            Action = (i % 2 == 0) ? "jump" : "run",
                            Time = i
                        };

                        var json = JsonConvert.SerializeObject(actionInfo);
                        var result = _sut.AddAction(json);
                        results.Add(result);
                    }

                    return results;
                });

                tasks[i] = addTask;
            }

            var getTask = Task.Run(() =>
            {
                var results = new List<string>();
                for (int i = 0; i < 100; i++)
                {
                    var result = _sut.GetStats();
                    results.Add(result);
                    Task.Delay(TimeSpan.FromMilliseconds(100));
                }

                return results;
            });

            tasks[tasks.Length - 1] = getTask;

            //Act
            var results = await Task.WhenAll(tasks);

            //Assert
            for (int i = 0; i < results.Length - 1; i++)
            {
                Assert.All(results[i], result => Assert.Equal(string.Empty, result));
            }

            Assert.All(results.Last(), result => Assert.NotEqual(string.Empty, result));
        }

        #endregion

        #region GetStats Tests


        #endregion
    }
}
