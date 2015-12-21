﻿using Phrike.GroundControl.Helper;
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
        private const string RecordingSettingsElement = "RecordingSettings";
        private const string RecordingGameConfigElement = "RecordingGameConfig";
        private const string RecordingCameraConfigElement = "RecordingCameraConfig";

        private const string DefaultRecordingGameConfig = "-f dshow -i video=\"screen-capture-recorder\":audio=\"virtual-audio-capturer\" -vcodec libx264 -r 20 -qp 0 -preset ultrafast";
        private const string DefaultRecordingCameraConfig = "-f dshow -i video=\"Integrated Camera\" -vcodec libx264 -r 20 -qp 0 -preset ultrafast";
        private const int DefaultSensorType = 0;
        private static readonly string DefaultCOMPort = GetDefaultComPort();
        private const bool DefaultRecordingEnabled = true;

        public static SensorType SelectedSensorType { get; set; }
        public static string SensorComPort { get; set; }
        public static bool RecordingEnabled { get; set; }
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

                XElement XMLSensorType = doc.Descendants(SensorTypeElement).FirstOrDefault();
                XElement XMLComPort = doc.Descendants(ComPortElement).FirstOrDefault();
                XElement XMLRecordingEnabled = doc.Descendants(RecordingEnabledElement).FirstOrDefault();
                XElement XMLRecordingGameConfig = doc.Descendants(RecordingGameConfigElement).FirstOrDefault();
                XElement XMLRecordingCameraConfig = doc.Descendants(RecordingCameraConfigElement).FirstOrDefault();
                SelectedSensorType = (SensorType)Enum.Parse(typeof(SensorType), XMLSensorType.Value);
                SensorComPort = XMLComPort.Value.Equals("") ? DefaultCOMPort : XMLComPort.Value;
                RecordingEnabled = bool.Parse(XMLRecordingEnabled.Value);
                RecordingGameConfig = XMLRecordingGameConfig.Value.Equals("") ? DefaultRecordingGameConfig : XMLRecordingGameConfig.Value;
                RecordingCameraConfig = XMLRecordingCameraConfig.Value.Equals("") ? DefaultRecordingCameraConfig : XMLRecordingCameraConfig.Value;
            }
            catch (FileNotFoundException)
            {
                SelectedSensorType = DefaultSensorType;
                SensorComPort = DefaultCOMPort;
                RecordingEnabled = DefaultRecordingEnabled;
                RecordingGameConfig = DefaultRecordingGameConfig;
                RecordingCameraConfig = DefaultRecordingCameraConfig;
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
                    new XElement(RecordingEnabledElement, RecordingEnabled),
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
