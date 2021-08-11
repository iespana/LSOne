using System;
using System.IO;
using System.Xml.Serialization;

namespace LSOne.Services.Interfaces.Configurations
{
    [XmlRoot("PARAMETERS")]
    public class SiteServiceConfig
    {
        // public Conversion Conversion { get; set; } Ignored for now
        [XmlElement("SiteServiceConfiguration")]
        public SiteServiceSettings SiteServiceSettings { get; set; }

        public static SiteServiceConfig Read(string fileName)
        {
            var xs = new XmlSerializer(typeof(SiteServiceConfig));
            if (File.Exists(fileName))
            {
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    return xs.Deserialize(fs) as SiteServiceConfig;
            }
            return new SiteServiceConfig();
        }

        // Should not be needed, all updates are done manually in config files
        public void Save(string fileName)
        {
            var xs = new XmlSerializer(typeof(SiteServiceConfig));
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                xs.Serialize(fs, this);
        }

        // Should not be needed, all updates are done manually in config files
        public void Save()
        {
            var xs = new XmlSerializer(typeof(SiteServiceConfig));
            using (var fs = new FileStream(ConfigFilePath("GlobalConfig.xml"), FileMode.Create, FileAccess.Write))
                xs.Serialize(fs, this);
        }

        public static SiteServiceConfig Read()
        {

            var path = ConfigFilePath("GlobalConfig.xml");
            if (File.Exists(path))
            {
                try
                {
                    return Read(path);
                }
                catch (Exception)
                {

                }

            }

            SiteServiceConfig config = new SiteServiceConfig();
            config.SiteServiceSettings = new SiteServiceSettings();
            return config;

        }

        public static bool ConfigExists()
        {
            return File.Exists(ConfigFilePath("GlobalConfig.xml"));
        }

        private static string ConfigFilePath(string fileName)
        {
            string path;
           
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "LS Retail", "Global", fileName);

            return path;
        }
    }
}
