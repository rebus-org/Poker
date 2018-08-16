using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Poker
{
    /// <summary>
    /// Wraps an application configuration file, allowing for in-mem mutation of it
    /// </summary>
    public class ConfigurationPoker
    {
        static readonly List<string> EmptyList = new List<string>();

        readonly XmlDocument _xml;

        /// <summary>
        /// Parses the given <paramref name="appConfigXml"/> XML as an application configuration file
        /// </summary>
        public ConfigurationPoker(string appConfigXml)
        {
            if (appConfigXml == null) throw new ArgumentNullException(nameof(appConfigXml));

            _xml = new XmlDocument();

            try
            {
                _xml.LoadXml(appConfigXml);
            }
            catch (Exception exception)
            {
                throw new XmlException($"Could not parse XML: '{appConfigXml}'", exception);
            }
        }

        public IReadOnlyCollection<string> GetAppSettingKeys()
        {
            var appSettings = _xml.SelectNodes("/configuration/appSettings/add");

            return appSettings?.Cast<XmlNode>()
                       .Select(node =>
                           node.Attributes?.Cast<XmlAttribute>().FirstOrDefault(a => a.Name == "key")?.Value)
                       .Where(key => key != null)
                       .ToList()
                   ?? EmptyList;
        }

        public IReadOnlyCollection<string> GetConnectionStringNames()
        {
            var connectionStrings = _xml.SelectNodes("/configuration/connectionStrings/add");

            return connectionStrings?.Cast<XmlNode>()
                       .Select(node => node.Attributes?.Cast<XmlAttribute>().FirstOrDefault(a => a.Name == "name")?.Value)
                       .Where(name => name != null)
                       .ToList()
                   ?? EmptyList;
        }

        /// <summary>
        /// Gets the value of the appsetting with the given <paramref name="key"/>
        /// </summary>
        public string GetAppSetting(string key)
        {
            var appSetting = _xml.SelectSingleNode($"/configuration/appSettings/add[@key='{key}']");

            if (appSetting != null)
            {
                var xmlAttributeCollection = appSetting.Attributes;

                if (xmlAttributeCollection == null)
                {
                    throw new XmlException("Makes no sense because we got here by XPathing by attribute");
                }

                var existingValueAttribute = xmlAttributeCollection.OfType<XmlAttribute>().FirstOrDefault(a => a.Name == "value");

                if (existingValueAttribute != null)
                {
                    return existingValueAttribute.Value;
                }

                return null;
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the connection string with the given <paramref name="name"/>
        /// </summary>
        public string GetConnectionString(string name)
        {
            var connectionString = _xml.SelectSingleNode($"/configuration/connectionStrings/add[@name='{name}']");

            if (connectionString != null)
            {
                var xmlAttributeCollection = connectionString.Attributes;

                if (xmlAttributeCollection == null)
                {
                    throw new XmlException("Makes no sense because we got here by XPathing by attribute");
                }

                var existingValueAttribute = xmlAttributeCollection.OfType<XmlAttribute>().FirstOrDefault(a => a.Name == "connectionString");

                if (existingValueAttribute != null)
                {
                    return existingValueAttribute.Value;
                }

                return null;
            }

            return null;
        }

        /// <summary>
        /// Sets the value of the appsetting with the given <paramref name="key"/>
        /// </summary>
        public void SetAppSetting(string key, string value)
        {
            var appSetting = _xml.SelectSingleNode($"/configuration/appSettings/add[@key='{key}']");

            if (appSetting != null)
            {
                var xmlAttributeCollection = appSetting.Attributes;

                if (xmlAttributeCollection == null)
                {
                    throw new XmlException("Makes no sense because we got here by XPathing by attribute");
                }

                var existingValueAttribute = xmlAttributeCollection.OfType<XmlAttribute>().FirstOrDefault(a => a.Name == "value");

                if (existingValueAttribute != null)
                {
                    existingValueAttribute.Value = value;
                    return;
                }

                var newValueAttribute = _xml.CreateAttribute("value");
                newValueAttribute.Value = value;
                xmlAttributeCollection.Append(newValueAttribute);

                return;
            }

            var appSettingsNode = _xml.SelectSingleNode("/configuration/appSettings");

            if (appSettingsNode == null)
            {
                throw new XmlException("Could not find /configuration/appSettings in XML");
            }

            var xmlElement = _xml.CreateElement("add");
            var keyAttribute = _xml.CreateAttribute("key");
            keyAttribute.Value = key;
            var valueAttribute = _xml.CreateAttribute("value");
            valueAttribute.Value = value;
            xmlElement.Attributes.Append(keyAttribute);
            xmlElement.Attributes.Append(valueAttribute);
            appSettingsNode.AppendChild(xmlElement);
        }

        /// <summary>
        /// Sets the value of the connection string with the given <paramref name="name"/>
        /// </summary>
        public void SetConnectionString(string name, string value)
        {
            var connectionString = _xml.SelectSingleNode($"/configuration/connectionStrings/add[@name='{name}']");

            if (connectionString != null)
            {
                var xmlAttributeCollection = connectionString.Attributes;

                if (xmlAttributeCollection == null)
                {
                    throw new XmlException("Makes no sense because we got here by XPathing by attribute");
                }

                var existingValueAttribute = xmlAttributeCollection.OfType<XmlAttribute>().FirstOrDefault(a => a.Name == "connectionString");

                if (existingValueAttribute != null)
                {
                    existingValueAttribute.Value = value;
                    return;
                }

                var newValueAttribute = _xml.CreateAttribute("connectionString");
                newValueAttribute.Value = value;
                xmlAttributeCollection.Append(newValueAttribute);

                return;
            }

            var connectionStringsNode = _xml.SelectSingleNode("/configuration/connectionStrings");

            if (connectionStringsNode == null)
            {
                throw new XmlException("Could not find /configuration/connectionStrings in XML");
            }

            var xmlElement = _xml.CreateElement("add");
            var nameAttribute = _xml.CreateAttribute("name");
            nameAttribute.Value = name;
            var connectionStringAttribute = _xml.CreateAttribute("connectionString");
            connectionStringAttribute.Value = value;
            xmlElement.Attributes.Append(nameAttribute);
            xmlElement.Attributes.Append(connectionStringAttribute);
            connectionStringsNode.AppendChild(xmlElement);
        }

        /// <summary>
        /// Renders the current state of the in-mem application configuration file into XML
        /// </summary>
        public string RenderXml()
        {
            using (var destination = new StringWriter())
            {
                using (var writer = new XmlTextWriter(destination))
                {
                    writer.Formatting = Formatting.Indented;

                    _xml.WriteTo(writer);
                }

                return destination.ToString();
            }
        }
    }
}