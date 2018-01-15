using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace W.Tests
{
    [TestClass]
    public class TestAnything
    {
        [TestMethod]
        public async Task CannotReset_CancellationToken()
        {
            async Task<bool> SleepAsync(CancellationToken token, int msSleep)
            {
                try
                {
                    await Task.Delay(msSleep);
                    token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException) { return true; }
                return false;
            }

            var cts = new CancellationTokenSource();

            cts.CancelAfter(10);
            Assert.IsFalse(cts.IsCancellationRequested);
            Assert.IsFalse(cts.Token.IsCancellationRequested);

            var result = await SleepAsync(cts.Token, 50);
            Assert.IsTrue(result);
            Assert.IsTrue(cts.IsCancellationRequested);
            Assert.IsTrue(cts.Token.IsCancellationRequested);

            cts.CancelAfter(500); //this does NOT reset the token
            Assert.IsTrue(cts.IsCancellationRequested);
            Assert.IsTrue(cts.Token.IsCancellationRequested);

            //Assert.IsFalse(cts.IsCancellationRequested);
            //Assert.IsFalse(cts.Token.IsCancellationRequested);
            //result = await SleepAsync(cts.Token, 1000);
            //Assert.IsTrue(result);
            //Assert.IsTrue(cts.IsCancellationRequested);
            //Assert.IsTrue(cts.Token.IsCancellationRequested);
        }
    }
}
