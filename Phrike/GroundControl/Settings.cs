using Phrike.GroundControl.Helper;
using Phrike.GroundControl.Models;
using System;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Xml.Linq;

namespace Phrike.GroundControl
{
    public static class Settings
    {
        private const string DefaultFileName = @"\settings.xml";
        private const string RootElementName = "Settings";
        private const string SensorSettingsElement = "SensorSettings";
        private const string SensorTypeElement = "SensorType";
        private const string ComPortElement = "ComPort";
        private const string RecordingEnabledElement = "RecordingEnabled";

        private const int DefaultSensorType = 0;
        private static readonly string DefaultCOMPort = GetDefaultComPort();
        private const bool DefaultRecordingEnabled = true;

        public static SensorType SelectedSensorType { get; set; }
        public static string SensorComPort { get; set; }
        public static bool RecordingEnabled { get; set; }

        private static string GetDefaultComPort()
        {
            return SerialPort.GetPortNames().FirstOrDefault() ?? "";
        }

        public static void LoadSettings()
        {
            try
            {
                XDocument doc = XDocument.Load(PathHelper.PhrikeDirectory + DefaultFileName);

                XElement XMLSensorType = doc.Descendants(SensorTypeElement).FirstOrDefault();
                XElement XMLComPort = doc.Descendants(ComPortElement).FirstOrDefault();
                XElement XMLRecordingEnabled = doc.Descendants(RecordingEnabledElement).FirstOrDefault();

                SelectedSensorType = (SensorType)Enum.Parse(typeof(SensorType), XMLSensorType.Value);
                SensorComPort = XMLComPort.Value.Equals("") ? DefaultCOMPort : XMLComPort.Value;
                RecordingEnabled = bool.Parse(XMLRecordingEnabled.Value);
            }
            catch (FileNotFoundException)
            {
                SelectedSensorType = DefaultSensorType;
                SensorComPort = DefaultCOMPort;
                RecordingEnabled = DefaultRecordingEnabled;
            } 
        }

        public static void SaveSettings()
        {
            new XDocument(
                new XElement(RootElementName,
                    new XElement(SensorSettingsElement,
                        new XElement(SensorTypeElement, (int)SelectedSensorType),
                        new XElement(ComPortElement, SelectedSensorType == SensorType.GMobiLab ? SensorComPort : "")
                    ),
                    new XElement(RecordingEnabledElement, RecordingEnabled)
                )
            )
            .Save(PathHelper.PhrikeDirectory + DefaultFileName);
        }
    }
}
