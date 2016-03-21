using Katelyn.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katelyn.Tests
{
    [TestClass]
    public class MyTestClass
        : IListener
    {
        private int _errorCount = 0;
        private int _successCount = 0;


        [TestMethod]
        public void MyTestMethod()
        {
            Crawler.Crawl(new Uri("http://localhost/"), this);
        }

        public void OnEnd()
        {
            _errorCount.ShouldBe(0);
            _successCount.ShouldBeGreaterThan(0);
        }

        public void OnError(string address, Exception exception)
        {
            _errorCount++;
        }

        public void OnSuccess(string address)
        {
            _successCount++;
        }
    }
}
