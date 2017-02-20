using System;
using System.IO;
using NUnit.Framework;

namespace Poker.Tests
{
    [TestFixture]
    public class TestAppConfig
    {
        [Test]
        public void CanGetPath()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fake", "FakeApp.config");

            using (Swindler.AppConfig.Use(path))
            {
                var pokerPath = AppConfig.GetPath();

                Assert.That(pokerPath, Is.EqualTo(path));
            }
        }
    }
}