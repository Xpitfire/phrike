# Sensor and Filter API

The sensor API consists of the following components:

  - [A class for controlling the actual gMobiLab+ hardware (`GMobiLab.SensorDevice`)](#recording-data-from-sensors)
  - [Classes for reading files with sensor data in a format that is natively produced by a sensor system.](#retrieving-recorded-data)
  - [Filters (operating on double collections)](#filtering-data)

## Recording data from Sensors

Recording is supported with the gMobiLab+ hardware. See the [corresponding wiki page](Sensor-Engineering) for details on how to set up the hardware. The class to use for this is `Phrike.GMobiLab.SensorDevice`. An example of recording Channel 5 from a sensor connected to COM6 follows:

```c#
using (var device = new SensorDevice("COM6:"))
{
    device.SetSensorEnabled(device.Sensors[GMobiLabSensors.Channel5Id]);
    device.SetSdFilename("MYCHAN5RECORDING"); // Note that a unique number is appended automatically
    device.StartRecording();
    Console.ReadLine(); // Record until Enter is pressed.
    device.StopRecording();
}
```

To enable all sensors, you can iterate over the `Sensors` list and call `SetSensorEnabled` for each one.

To select a COM-Port, you may want to use [`System.IO.Ports.SerialPort.GetPortNames`](https://msdn.microsoft.com/en-us/library/system.io.ports.serialport.getportnames) -- but note that the `SensorDevice` constructor expects the port name to end with a colon `:`.

## Retrieving recorded data

The sensor API retrieves recorded data from files. Supported are gMobilab+ `.bin` files (using `Phrike.GMobiLab.SensorDataFileStreamer`) and CSV-files from the Schuhfried Biofeedback 2000 System (using `Phrike.Sensors.BiofeedbackCsvFileStreamer`). Both classes take a filename in their constructor and implement the `ISensorHub` interface that can be used to stream the data or retrieve it at once. The following retrieves all samples recorded in a gMobiLab file:

```c#
var filename = "MYCHAN5RECORDING_MP0001.BIN";
Sample[] samples;
using (ISensorHub filereader = new SensorDataFileStreamer(filename))
{
    samples = filereader.ReadSamples().ToArray();
}
```

A single `Sample` contains the values of all sensors that were recorded per sample as a `IReadOnlyList<double>`. This is necessary for streaming a file, but for further processing, it is often more elegant to have one array of values per Sensor instead. For this, the `DataBundle` and `DataSeries` classes exist. `DataBundle` is a bundle of `DataSeries` and a `DataSeries` is an array (`double[]`) of values with additional meta-data such as the sample rate. Although possible (`ISensorHub` provides methods to retrieve metadata of sensors too) it is cumbersome to manually transpose from `Sample[]` & `ISensorHub` to `DataBundle`, so `DataBundle` provides a method `FromHub` that takes an `ISensorHub`, reads all samples, retrieves metadata and returns the information as a new `DataBundle` object with one `DataSeries` per sensor. Using `DataBundle` the above example looks like this:

```c#
DataBundle data;
using (ISensorHub filereader = new SensorDataFileStreamer(filename)) {
    data = DataBundle.FromHub(filereader);
}
```

If the recorded data comes from a gMobiLab file that contains channel 5, the following then retrieves the `DataSeries` and some metadata for it:

```c#
DataSeries chan5 = data.DataSeries.Single(ds => ds.Name == "Channel 05");
double[] chan5values = chan5.Data;
double chan5average = chan5.Statistics.Average;
```

## Filtering data

Data filtering is in principle independent of data retrieval as the filter API operates solely on `IReadOnlyList<double>`s. The central component of the filter API is the `IFilter` interface which contains just a single method `Filter`:

```c#
public interface IFilter
{
    IReadOnlyList<double> Filter(IReadOnlyList<double> unfilteredData);
}

```

The `unfilteredData` is left unchanged and a new collection containing the filtered data is returned.

### Concrete Filters

The following generally useful filters are provided:

* Smoothing
  - `MedianFilter`
  - `AverageFilter`
  - `GaussFilter` (Gauss-curve weighted average)
* Discarding extreme values
  - `BinaryThresholdFilter` (sets values that deviate more than a custom threshold from all other values to zero)
* Finding peaks
  - `EdgeDetectionFilter` (intensifies peaks)
  - `PeakFilter` (sets all but the most extreme value to zero)

Except `BinaryThresholdFilter` all filters have a *radius* which determines how many values they will consider at once (number of processed values <= radius * 2 + 1). In general the greater the radius, the stronger the effect of the filter will be.

For example, this filters an array of doubles with an `AverageFilter` with radius 1:
```c#
double[] rawValues = new[] {0, 3, 10, 0};
IReadOnlyList<double> filteredValues = new AverageFilter(1).Filter(rawValues);
```

### Chaining multiple filters

Often multiple filters should be applied one after another to some input data, such that the first filter's output is fed to the second filter as input and so on. For this, the `FilterChain` class is provided which provides a constructor taking a `params` array of `IFilter`s and an `Add` method that can add additional filters. Usage could look like this:

```c#
var chain = new FilterChain(new GaussFilter(4), new EdgeDetectionFilter(2));
// or equivalently:
var chain = new FilterChain();
chain.Add(new GaussFilter(4));
chain.Add(new EdgeDetectionFilter(2));


IReadOnlyList<double> filtedValues = chain.Filter(rawValues);
```

Note that `FilterChain` too implements the `IFilter` interface, making the filter API an instance of the [Composite Pattern](https://en.wikipedia.org/wiki/Composite_pattern), with `IFilter` as Component, the concrete filter classes as `Leaf`s and `FilterChain` as Composite.

### Calculating the pulse rate

To calculate a pule rate, multiple filters are needed. `PulseCalculator.MakePulseFilterChain()` returns a filter chain that calculates the pulse from a heart curve as recorded by gMobiLab+'s ECG (Channel 5):

```c#
IFilter pulseFilter = PulseCalculator.MakePulseFilterChain();
IReadOnlyList<double> pulseRates = pulseFilter.Filter(chan5values);
```

The pulse is returned in beats per minute.