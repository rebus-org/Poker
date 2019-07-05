﻿using System;
using System.Linq;
using NUnit.Framework;

namespace Poker.Tests
{
    [TestFixture]
    public class TestConfigurationPoker
    {
        const string XmlShizzle = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""appSetting1"" value=""local""/>
    <add key=""appSetting2"" />
  </appSettings>
  <connectionStrings>
    <add name=""connectionString1"" connectionString=""host=localhost; db=fleetmanager; user id=postgres; password=postgres"" />
    <add name=""connectionString2"" />
  </connectionStrings>
  <startup>
    <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.6.1"" />
  </startup>
  <runtime>
    <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
      <dependentAssembly>
        <assemblyIdentity name=""Newtonsoft.Json"" publicKeyToken=""30ad4fe6b2a6aeed"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-9.0.0.0"" newVersion=""9.0.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Microsoft.Owin"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.0.1.0"" newVersion=""3.0.1.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Npgsql"" publicKeyToken=""5d8b90d52f46fda7"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.1.0.0"" newVersion=""3.1.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Serilog"" publicKeyToken=""24c2f752a8e58a10"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-2.0.0.0"" newVersion=""2.0.0.0"" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>";

        [Test]
        public void CanGetKeysAndNames()
        {
            var poker = ConfigurationPoker.ParseXml(XmlShizzle);

            var appSettingKeys = poker.GetAppSettingKeys();
            var connectionStringNames = poker.GetConnectionStringNames();

            Assert.That(appSettingKeys, Is.EqualTo(new[] { "appSetting1", "appSetting2" }));
            Assert.That(connectionStringNames, Is.EqualTo(new[] { "connectionString1", "connectionString2" }));
        }

        [Test]
        public void CanGetAppSettingsAndConnectionStringsToo()
        {
            var poker = ConfigurationPoker.ParseXml(XmlShizzle);

            var appSettingValue = poker.GetAppSetting("appSetting1");
            var connectionStringValue = poker.GetConnectionString("connectionString1");

            Assert.That(appSettingValue, Is.EqualTo("local"));
            Assert.That(connectionStringValue, Is.EqualTo(@"host=localhost; db=fleetmanager; user id=postgres; password=postgres"));
        }

        [Test]
        public void GetsNullOnMiss()
        {
            var poker = ConfigurationPoker.ParseXml(XmlShizzle);

            Assert.That(poker.GetAppSetting("appSetting2"), Is.Null);
            Assert.That(poker.GetAppSetting("appSetting_does_not_exist"), Is.Null);
            Assert.That(poker.GetConnectionString("connectionString2"), Is.Null);
            Assert.That(poker.GetConnectionString("connectionString_does_not_exist"), Is.Null);
        }

        [Test]
        public void CanPokeXmlValuesIn_ExistingAppSetting_ExistingValueAttribute()
        {
            var applicator = ConfigurationPoker.ParseXml(XmlShizzle);

            applicator.SetAppSetting("appSetting1", "release");

            var xml = applicator.RenderXml();

            const string expectedXml = @"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""appSetting1"" value=""release"" />
    <add key=""appSetting2"" />
  </appSettings>
  <connectionStrings>
    <add name=""connectionString1"" connectionString=""host=localhost; db=fleetmanager; user id=postgres; password=postgres"" />
    <add name=""connectionString2"" />
  </connectionStrings>
  <startup>
    <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.6.1"" />
  </startup>
  <runtime>
    <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
      <dependentAssembly>
        <assemblyIdentity name=""Newtonsoft.Json"" publicKeyToken=""30ad4fe6b2a6aeed"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-9.0.0.0"" newVersion=""9.0.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Microsoft.Owin"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.0.1.0"" newVersion=""3.0.1.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Npgsql"" publicKeyToken=""5d8b90d52f46fda7"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.1.0.0"" newVersion=""3.1.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Serilog"" publicKeyToken=""24c2f752a8e58a10"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-2.0.0.0"" newVersion=""2.0.0.0"" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>";

            AssertXmlsAreEqual(xml, expectedXml);
        }

        [Test]
        public void CanPokeXmlValuesIn_ExistingAppSetting_NonExistentValueAttribute()
        {
            var applicator = ConfigurationPoker.ParseXml(XmlShizzle);

            applicator.SetAppSetting("appSetting2", "release");

            var xml = applicator.RenderXml();

            const string expectedXml = @"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""appSetting1"" value=""local"" />
    <add key=""appSetting2"" value=""release"" />
  </appSettings>
  <connectionStrings>
    <add name=""connectionString1"" connectionString=""host=localhost; db=fleetmanager; user id=postgres; password=postgres"" />
    <add name=""connectionString2"" />
  </connectionStrings>
  <startup>
    <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.6.1"" />
  </startup>
  <runtime>
    <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
      <dependentAssembly>
        <assemblyIdentity name=""Newtonsoft.Json"" publicKeyToken=""30ad4fe6b2a6aeed"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-9.0.0.0"" newVersion=""9.0.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Microsoft.Owin"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.0.1.0"" newVersion=""3.0.1.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Npgsql"" publicKeyToken=""5d8b90d52f46fda7"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.1.0.0"" newVersion=""3.1.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Serilog"" publicKeyToken=""24c2f752a8e58a10"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-2.0.0.0"" newVersion=""2.0.0.0"" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>";

            AssertXmlsAreEqual(xml, expectedXml);
        }

        [Test]
        public void CanPokeXmlValuesIn_NonExistentAppSetting()
        {
            var applicator = ConfigurationPoker.ParseXml(XmlShizzle);

            applicator.SetAppSetting("appSetting3", "release");

            var xml = applicator.RenderXml();

            const string expectedXml = @"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""appSetting1"" value=""local"" />
    <add key=""appSetting2"" />
    <add key=""appSetting3"" value=""release"" />
  </appSettings>
  <connectionStrings>
    <add name=""connectionString1"" connectionString=""host=localhost; db=fleetmanager; user id=postgres; password=postgres"" />
    <add name=""connectionString2"" />
  </connectionStrings>
  <startup>
    <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.6.1"" />
  </startup>
  <runtime>
    <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
      <dependentAssembly>
        <assemblyIdentity name=""Newtonsoft.Json"" publicKeyToken=""30ad4fe6b2a6aeed"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-9.0.0.0"" newVersion=""9.0.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Microsoft.Owin"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.0.1.0"" newVersion=""3.0.1.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Npgsql"" publicKeyToken=""5d8b90d52f46fda7"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.1.0.0"" newVersion=""3.1.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Serilog"" publicKeyToken=""24c2f752a8e58a10"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-2.0.0.0"" newVersion=""2.0.0.0"" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>";

            AssertXmlsAreEqual(xml, expectedXml);
        }

        [Test]
        public void CanPokeXmlValuesIn_ExistingConnectionString_ExistingConnectionStringAttribute()
        {
            var applicator = ConfigurationPoker.ParseXml(XmlShizzle);

            applicator.SetConnectionString("connectionString1", "a_connection");

            var xml = applicator.RenderXml();

            const string expectedXml = @"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""appSetting1"" value=""local"" />
    <add key=""appSetting2"" />
  </appSettings>
  <connectionStrings>
    <add name=""connectionString1"" connectionString=""a_connection"" />
    <add name=""connectionString2"" />
  </connectionStrings>
  <startup>
    <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.6.1"" />
  </startup>
  <runtime>
    <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
      <dependentAssembly>
        <assemblyIdentity name=""Newtonsoft.Json"" publicKeyToken=""30ad4fe6b2a6aeed"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-9.0.0.0"" newVersion=""9.0.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Microsoft.Owin"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.0.1.0"" newVersion=""3.0.1.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Npgsql"" publicKeyToken=""5d8b90d52f46fda7"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.1.0.0"" newVersion=""3.1.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Serilog"" publicKeyToken=""24c2f752a8e58a10"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-2.0.0.0"" newVersion=""2.0.0.0"" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>";

            AssertXmlsAreEqual(xml, expectedXml);
        }

        [Test]
        public void CanPokeXmlValuesIn_ExistingConnectionString_NonExistentConnectionStringAttribute()
        {
            var applicator = ConfigurationPoker.ParseXml(XmlShizzle);

            applicator.SetConnectionString("connectionString2", "a_connection");

            var xml = applicator.RenderXml();

            const string expectedXml = @"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""appSetting1"" value=""local"" />
    <add key=""appSetting2"" />
  </appSettings>
  <connectionStrings>
    <add name=""connectionString1"" connectionString=""host=localhost; db=fleetmanager; user id=postgres; password=postgres"" />
    <add name=""connectionString2"" connectionString=""a_connection"" />
  </connectionStrings>
  <startup>
    <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.6.1"" />
  </startup>
  <runtime>
    <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
      <dependentAssembly>
        <assemblyIdentity name=""Newtonsoft.Json"" publicKeyToken=""30ad4fe6b2a6aeed"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-9.0.0.0"" newVersion=""9.0.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Microsoft.Owin"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.0.1.0"" newVersion=""3.0.1.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Npgsql"" publicKeyToken=""5d8b90d52f46fda7"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.1.0.0"" newVersion=""3.1.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Serilog"" publicKeyToken=""24c2f752a8e58a10"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-2.0.0.0"" newVersion=""2.0.0.0"" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>";

            AssertXmlsAreEqual(xml, expectedXml);
        }

        [Test]
        public void CanPokeXmlValuesIn_NonExistentConnectionString()
        {
            var applicator = ConfigurationPoker.ParseXml(XmlShizzle);

            applicator.SetConnectionString("connectionString3", "a_connection");

            var xml = applicator.RenderXml();

            const string expectedXml = @"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""appSetting1"" value=""local"" />
    <add key=""appSetting2"" />
  </appSettings>
  <connectionStrings>
    <add name=""connectionString1"" connectionString=""host=localhost; db=fleetmanager; user id=postgres; password=postgres"" />
    <add name=""connectionString2"" />
    <add name=""connectionString3"" connectionString=""a_connection"" />
  </connectionStrings>
  <startup>
    <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.6.1"" />
  </startup>
  <runtime>
    <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
      <dependentAssembly>
        <assemblyIdentity name=""Newtonsoft.Json"" publicKeyToken=""30ad4fe6b2a6aeed"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-9.0.0.0"" newVersion=""9.0.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Microsoft.Owin"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.0.1.0"" newVersion=""3.0.1.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Npgsql"" publicKeyToken=""5d8b90d52f46fda7"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-3.1.0.0"" newVersion=""3.1.0.0"" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name=""Serilog"" publicKeyToken=""24c2f752a8e58a10"" culture=""neutral"" />
        <bindingRedirect oldVersion=""0.0.0.0-2.0.0.0"" newVersion=""2.0.0.0"" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>";

            AssertXmlsAreEqual(xml, expectedXml);
        }

        static void AssertXmlsAreEqual(string xml, string expectedXml)
        {
            var normalizedXml = NormalizeNewlines(xml);
            var normalizedExpectedXml = NormalizeNewlines(expectedXml);

            Console.WriteLine($@"Here are the first characters:

{string.Join(" ", normalizedXml.Take(5).Select(c => $"{(int)c:00}"))}

{string.Join(" ", normalizedExpectedXml.Take(5).Select(c => $"{(int)c:00}"))}

");

            Console.WriteLine($@"Here they are - this is the expected XML:

""""""""""
{normalizedExpectedXml}
""""""""""

and this is the actual XML:

""""""""""
{normalizedXml}
""""""""""
");


            //Assert.That(normalizedXml, CompareConstraint.IsIdenticalTo(normalizedExpectedXml));

            Assert.That(normalizedXml, Is.EqualTo(normalizedExpectedXml));
        }

        static string NormalizeNewlines(string str)
        {
            var normalizedStr = string.Join(Environment.NewLine,
                str.Split(new[] { "\r", "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));

            // don't know how this one snuck in, but let's just remove it
            const int utf8Bom = 65279;

            if (normalizedStr.First() == utf8Bom)
            {
                normalizedStr = normalizedStr.Substring(1);
            }

            return normalizedStr;
        }
    }
}
