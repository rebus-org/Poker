using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace Poker
{
    public class ConfigurationPoker
    {
        readonly XmlDocument _xml;

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