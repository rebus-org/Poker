using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Poker.Tests.Bugs
{
    [TestFixture]
    public class VerifyCanReadSomeParticularAppSetting 
    {
        [Test]
        public void ItWorksAsItShould()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bugs", "test01.xml");
            var xml = File.ReadAllText(filePath, Encoding.UTF8);

            Console.WriteLine(xml);

            var poker = new ConfigurationPoker(xml);

            Assert.That(poker.GetAppSetting("masterkey"), Is.EqualTo("bimmelimmelim"));
            Assert.That(poker.GetAppSetting("logpath"), Is.EqualTo(@"c:\data\fm5"));
        }
    }
}