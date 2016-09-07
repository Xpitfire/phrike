# Generic Data Format

The new interface is simplified and each sample only includes an array with the different sensors as an index. 

The interface ISensorHub is allows retrieving the recorded data as an IEnumerable of Samples. ISensorHubDevice additionally provides methods for recording data.
The IEnumberable is an array which contains Samples. A Sample is an simple array in which there are the sample-values for each enabled sensor. 

![GDF](https://gitlab.com/OperationPhrike/phrike/uploads/69aa254f06caf32e53003e39329eeff9/GDF.jpg)

Which index belongs to which sensor can be determined with  GetSensorValueIndexInSample. 

SensorUtil.GetSampleValues returns a double-array which contains the sensor-values from the selected index. 

The class SensorDataFileStreamer implements the interface ISensorHub for g.mobilab binary data. 

The class SensorDevice implements an ISensorHubDevice for the g.mobilab sensors but data transfer is not supported yet.