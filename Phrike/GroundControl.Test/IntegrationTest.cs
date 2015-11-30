﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phrike.GroundControl.Controller;
using Phrike.GroundControl.ViewModels;

namespace GroundControl.Test
{
    [TestClass]
    public class IntegrationTest
    {
        /// <summary>
        /// This test checks if an file import and Plot object instanciation is working
        /// without any exceptions.
        /// </summary>
        [TestMethod]
        public void TestUnrealEngineViewModel()
        {
            var analysisVM = new AnalysisViewModel();
            analysisVM.LoadPulsDataTask("./Samples/EKG-Signal_zur_Verarbeitung_23_04_15.bin");
            // could load application
        }

        /// <summary>
        /// This test requires a directory containing the unreal assets (OperationsPhrike-Balance) within
        /// the developmnet root.
        /// It uses a relative symbolic link to which is copied on build within the bin/Debug or bin/Release 
        /// sub-directory.
        /// 
        /// Used symbolic link (OperationPhrikeBalanceExec.link):
        /// $cmd> mklink OperationPhrikeExec.link ..\..\UnrealData\Balance\Balance.exe
        /// </summary>
        [TestMethod]
        public void TestUnrealEngineController()
        {
            var stressTestVM = new DebugViewModel();
            stressTestVM.StartUnrealEngineTask();
        }
    }
}
