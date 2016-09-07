using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Phrike.Sensors.Filters;

namespace Sensors.Test
{
    [TestClass]
    public class SensorFilterTest
    {
        [TestMethod]
        public void RadiusFilterTest()
        {
            RadiusFilterBase rfb = new RadiusFilterBase(3);
        }
    }
}
