using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace LSOne.DataLayer.DataProviders.ProviderConfig
{
    /// <summary>
    /// This class serves as a base class for applications that wish to use SGEN'd XML serializers for configuration information
    /// This can speed up configuration access and thus application startup time
    /// A utility class to serialize configuration sections fromo configuration files
    /// <example> Example for usage:
    /// <code>
    /// <configuration>
    ///		<configSections>
    ///			<sectionGroup name="Mekkanis">
    ///				<section name="MySection" type="MyXmlBasedConfigurationSectionHandler, MyAssembly" />
    ///			</sectionGroup>
    ///		</configSections>
    /// </configuration>
    /// </code>
    /// where MyXmlBasedConfigurationSectionHandler inherits from BaseSerializedSectionHandler
    /// </example>
    /// </summary>
    public abstract class BaseSerializedSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// The seralizatio instance
        /// </summary>
        protected abstract XmlSerializer Serializer { get; }

        /// <inheritdoc />
        public object Create(object parent, object configContext, XmlNode section)
        {
            var serializer = Serializer;
            
            var attr = section.Attributes["file"];
            if (attr != null)
            {
                bool optional = false;
                var optionalAttr = section.Attributes["optional"];
                if (optionalAttr != null)
                {
                    bool.TryParse(optionalAttr.InnerText, out optional);
                }
                var resolvedFileName = ResolveConfigFile(attr.InnerText, optional);
                if (resolvedFileName != null)
                {
                    using (var sr = new StreamReader(resolvedFileName))
                    {
                        return serializer.Deserialize(sr);
                    }
                }
                return null;
            }
            
            return serializer.Deserialize(new XmlTextReader(new StringReader(section.OuterXml)));
        }

        /// <summary>
        /// Gets the serialized object's raw xml 
        /// </summary>
        /// <param name="obj">The object to get the xml for</param>
        /// <returns></returns>
        public string GetRawXml(object obj)
        {
            var serializer = Serializer;
            using (var w = new StringWriter())
            {
                serializer.Serialize(w, obj);
                w.Flush();
                return w.ToString();
            }
        }

        private static string ResolveConfigFile(string fileName, bool optional)
        {
            return ResolveConfigFile(fileName, optional, true);
        }

        private static string ResolveConfigFile(string fileName, bool optional, bool fallbackOnAppDomainBasePath)
        {
            //EnvironmentUtils.ResolveEnvironmentVariables(ref fileName);

            var lookedIn = new List<string>();
            var folders = Combine(
                    Directory.GetCurrentDirectory(),                        // Working folder
                    Application.StartupPath,                                // Folder where the application was started
                    Path.GetDirectoryName(Application.ExecutablePath),      // Application's executable path
                    fallbackOnAppDomainBasePath ? Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) : null);
            string hintFolders = ConfigurationManager.AppSettings["ConfigFileFolders"];
            if (!string.IsNullOrEmpty(hintFolders))
            {
                folders.AddRange(hintFolders.Split(';'));
            }

            string configFile = CheckDefaultFile(
                lookedIn,
                fileName,
                folders,
                Combine("", "..", "..\\..", "..\\..\\.."),
                Combine("", "config", "cfg"));

            if (!optional)
            {
                if (configFile == null)
                {
                    var lookedInMsg = "";
                    foreach (var s in lookedIn)
                    {
                        if (lookedInMsg.Length > 0)
                            lookedInMsg += Environment.NewLine;
                        lookedInMsg += s;
                    }
                    // Can't find the file
                    string error = string.Format(Properties.Resources.ErrorInConfigurationDefaultConfigFileNotFound,
                        fileName, Environment.NewLine, Directory.GetCurrentDirectory(),
                        lookedInMsg);
                    throw new Exception(string.Format(Properties.Resources.ErrorInConfiguration, fileName, Environment.NewLine, error));
                }

                System.Diagnostics.Debug.Assert(File.Exists(configFile));
            }
            return configFile;
        }

        private static string CheckDefaultFile(List<string> lookedIn,
            string defaultFile,
            List<string> basePaths,
            List<string> relativePaths,
            List<string> folders)
        {
            for (int i = 0; i < basePaths.Count; i++)
            {
                for (int j = 0; j < folders.Count; j++)
                {
                    if (relativePaths != null && relativePaths.Count > 0)
                    {
                        for (int k = 0; k < relativePaths.Count; k++)
                        {
                            string file = Path.Combine(
                                Path.Combine(Path.Combine(basePaths[i], relativePaths[k]), folders[j]),
                                defaultFile);
                            if (File.Exists(file))
                                return file;
                            lookedIn.Add(SaveFullPath(file));
                        }
                    }
                    else
                    {
                        string file = Path.Combine(Path.Combine(basePaths[i], folders[j]), defaultFile);
                        if (File.Exists(file))
                            return file;
                        lookedIn.Add(SaveFullPath(file));
                    }
                }
            }
            return null;
        }
        private static List<string> Combine(params string[] items)
        {
            var combined = new List<string>();
            // Get unique names
            foreach (string s in items)
            {
                if (s != null && !combined.Contains(s))
                    combined.Add(s);
            }
            return combined;
        }

        private static string SaveFullPath(string path)
        {
            try
            {
                return Path.GetFullPath(path);
            }
            catch
            {
                return path;
            }
        }
    }
}