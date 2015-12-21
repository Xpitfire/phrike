using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using DataAccess;
using Phrike.Sensors;

namespace Phrike.GroundControl.Controller
{

  class PositionDataController
  {
    public DataSeries PositionSpeedSeries { get; private set; }
    public DataSeries PositionAccelSeries { get; private set; }
    public DataSeries PositionIdleMovementSeries { get; private set; }
    public Double AverageSpeed { get; private set; }
    public Double AverageAccel { get; private set; }
    public Double TotalDistance { get; private set; }
    public Double Altitude { get; private set; }
    public Double MaxSpeed { get; private set; }

    public TimeSpan TotalTime { get; private set; }
    public TimeSpan TotalIdleTime { get; private set; }

    public PositionDataController()
    {
    }

    private void CalculatePlotData(IEnumerable<PositionData> allPositionData)
    {
      const Double CM_TO_KM = 100000;
      const Double CM_TO_M = 100;
      const Double MS_TO_S = 1000;
      const Double H_TO_MS = MS_TO_S * 3600;
      const Double CMPMS2_TO_MPS2 = MS_TO_S * MS_TO_S / CM_TO_M;
      const Double CMPMS_TO_KMPH = H_TO_MS / CM_TO_KM;
      double[] speedData = new double[allPositionData.Count()];
      double[] accelData = new double[allPositionData.Count()];
      double[] idleMovementData = new double[allPositionData.Count()];

      Double totalDistance = 0;
      Double prevSpeed = 0;
      Double currentSpeed = 0;
      Double distance = 0;
      Double accel = 0;
      Double totalDistanceZ = 0;
      Double distanceZ = 0;
      int idleMovement = 0;
      DateTime startTime;
      TimeSpan timeElapsed = TimeSpan.Zero, totalTimeElapsed = TimeSpan.Zero;
      PositionData prevData, currentData;
      IEnumerator<PositionData> iter = allPositionData.GetEnumerator();
      iter.MoveNext();
      prevData = iter.Current;
      startTime = prevData.Time;
      TotalIdleTime = TimeSpan.Zero;
      int i = 0;
      while (iter.MoveNext())
      {
        currentData = iter.Current;
        timeElapsed = currentData.Time - prevData.Time;
        totalTimeElapsed = currentData.Time - startTime;
        distanceZ = Math.Abs(currentData.Z - prevData.Z);
        totalDistanceZ += distanceZ;
        distance = Math.Sqrt(Math.Pow(currentData.X - prevData.X, 2) + Math.Pow(currentData.Y - prevData.Y, 2) + Math.Pow(distanceZ, 2));
        totalDistance += distance;
        currentSpeed = distance / timeElapsed.TotalMilliseconds;
        accel = (currentSpeed - prevSpeed) / (timeElapsed.TotalMilliseconds);
        speedData[i] = currentSpeed * CMPMS_TO_KMPH;
        accelData[i] = accel * CMPMS2_TO_MPS2;

        if (currentSpeed == 0)
        {
          idleMovement++;
          TotalIdleTime += timeElapsed;
        }
        else
        {
          idleMovement = 0;
        }
        idleMovementData[i] = idleMovement;
        prevData = currentData;
        prevSpeed = currentSpeed;
        i++;
      }

      PositionSpeedSeries = new DataSeries(speedData, 2, "PositionData", "Geschwindigkeit", Unit.Unknown);
      PositionAccelSeries = new DataSeries(accelData, 2, "PositionData", "Beschleunigung", Unit.Unknown);
      PositionIdleMovementSeries = new DataSeries(idleMovementData, 2, "PositionData", "Stillstand", Unit.Unknown);

      AverageSpeed = speedData.Average();
      AverageAccel = accelData.Average();
      MaxSpeed = speedData.Max();
      TotalTime = totalTimeElapsed;
      TotalDistance = totalDistance / CM_TO_KM;
      Altitude = totalDistanceZ / CM_TO_KM;
    }

    public bool LoadData(int id)
    {
      using (var unitOfWork = new UnitOfWork())
      {
        var allPositionData = unitOfWork.PositionDataRepository.Get(
            data => data.Test.Id == id);
        if(allPositionData.Count() == 0)
        {
          return false;
        }
        CalculatePlotData(allPositionData);
      }
      return true;
    }
    public bool LoadData(IEnumerable<PositionData> allPositionData)
    {
      if(allPositionData.Count() == 0)
      {
        return false;
      }
      CalculatePlotData(allPositionData);
      return true;
    }
  }
}
