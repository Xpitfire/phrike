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
        private const string ScreenRecordingEnabledElement = "ScreenRecordingEnabled";
        private const string WebcamRecordingEnabledElement = "WebcamRecordingEnabled";
        private const string RecordingSettingsElement = "RecordingSettings";
        private const string RecordingGameConfigElement = "RecordingGameConfig";
        private const string RecordingCameraConfigElement = "RecordingCameraConfig";

        private const string DefaultRecordingGameConfig = "-f dshow -i video=\"screen-capture-recorder\":audio=\"virtual-audio-capturer\" -vcodec libx264 -r 20 -qp 0 -preset ultrafast";
        private const string DefaultRecordingCameraConfig = "-f dshow -i video=\"Integrated Camera\" -vcodec libx264 -r 20 -qp 0 -preset ultrafast";
        private const int DefaultSensorType = 0;
        private static readonly string DefaultCOMPort = GetDefaultComPort();
        private const bool DefaultScreenRecordingEnabled = true;
        private const bool DefaultWebcamRecordingEnabled = false;

        public static SensorType SelectedSensorType { get; set; }
        public static string SensorComPort { get; set; }
        public static bool ScreenRecordingEnabled { get; set; }
        public static bool WebcamRecordingEnabled { get; set; }
        public static string RecordingGameConfig { get; set; }
        public static string RecordingCameraConfig { get; set; }

        private static string GetDefaultComPort()
        {
            return SerialPort.GetPortNames().FirstOrDefault() ?? "";
        }

        public static void LoadSettings()
        {
            try
            {
                XDocument doc = XDocument.Load(PathHelper.PhrikeDirectory + DefaultFileName);

                XElement xmlSensorType = doc.Descendants(SensorTypeElement).FirstOrDefault();
                XElement xmlComPort = doc.Descendants(ComPortElement).FirstOrDefault();
                XElement xmlScreenRecordingEnabled = doc.Descendants(ScreenRecordingEnabledElement).FirstOrDefault();
                XElement xmlWebcamRecordingEnabled = doc.Descendants(WebcamRecordingEnabledElement).FirstOrDefault();
                XElement xmlRecordingGameConfig = doc.Descendants(RecordingGameConfigElement).FirstOrDefault();
                XElement xmlRecordingCameraConfig = doc.Descendants(RecordingCameraConfigElement).FirstOrDefault();

                SelectedSensorType = (SensorType)Enum.Parse(typeof(SensorType), xmlSensorType.Value);
                SensorComPort = xmlComPort.Value.Equals("") ? DefaultCOMPort : xmlComPort.Value;
                ScreenRecordingEnabled = bool.Parse(xmlScreenRecordingEnabled.Value);
                WebcamRecordingEnabled = bool.Parse(xmlWebcamRecordingEnabled.Value);
                RecordingGameConfig = xmlRecordingGameConfig.Value.Equals("") ? DefaultRecordingGameConfig : xmlRecordingGameConfig.Value;
                RecordingCameraConfig = xmlRecordingCameraConfig.Value.Equals("") ? DefaultRecordingCameraConfig : xmlRecordingCameraConfig.Value;
            }         
            catch (FileNotFoundException)
            {
                SetDefaultValues();
            }
            catch (Exception)
            {
                SetDefaultValues();
                SaveSettings();
            }
        }

        private static void SetDefaultValues()
        {
            SelectedSensorType = DefaultSensorType;
            SensorComPort = DefaultCOMPort;
            ScreenRecordingEnabled = DefaultScreenRecordingEnabled;
            WebcamRecordingEnabled = DefaultWebcamRecordingEnabled;
            RecordingGameConfig = DefaultRecordingGameConfig;
            RecordingCameraConfig = DefaultRecordingCameraConfig;
        }

        public static void SaveSettings()
        {
            new XDocument(
                new XElement(RootElementName,
                    new XElement(SensorSettingsElement,
                        new XElement(SensorTypeElement, (int)SelectedSensorType),
                        new XElement(ComPortElement, SelectedSensorType == SensorType.GMobiLab ? SensorComPort : "")
                    ),
                    new XElement(ScreenRecordingEnabledElement, ScreenRecordingEnabled),
                    new XElement(WebcamRecordingEnabledElement, WebcamRecordingEnabled),
                    new XElement(RecordingSettingsElement, 
                        new XElement(RecordingGameConfigElement, RecordingGameConfig),
                        new XElement(RecordingCameraConfigElement, RecordingCameraConfig)
                    )
                )
            )
            .Save(PathHelper.PhrikeDirectory + DefaultFileName);
        }
    }
}
