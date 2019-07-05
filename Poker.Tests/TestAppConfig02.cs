using System;
using System.IO;
using NUnit.Framework;

namespace Poker.Tests
{
    [TestFixture]
    [Category("new")]
    public class TestAppConfig02
    {
        [Test]
        public void CanReadAppSettings()
        {
            //var path = Path.Combine(@"C:\apps\fm\cli", "fm.exe.config");
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fake", "FakeApp02.config");

            var poker = ConfigurationPoker.ReadFile(path);

            Assert.That(poker.GetAppSettingKeys().Count, Is.EqualTo(0));
        }

    }
}