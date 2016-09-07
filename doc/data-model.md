# Data Model
![DataModel](https://gitlab.com/OperationPhrike/phrike/uploads/77213afb2502a6e6f79eba5d562cb410/DataModel.png)



## Data Model Key
| Object | Description |
| -------- | -------- |
| Single-Arrow | References a single Object |
| Double-Arrow | References a Collection of Objects |

# Data Model Description
The Data Model is based around a **Test**, taken by a specific **Subject**. The **Test** contains notes, a title and a date, as well as single-object-references to **Scenario** and **Subject**. 
The **Scenario** contains the Version and Execution Path for the (UE-)**Scenario** and Information regarding the minimap containing the position of the Zero-Point and the Scale.
The **Subject** contains basic information about the (Test-)**Subject** and his status in the army.
A **Test** contains a set of **PositionData**, which specifies the position and viewing-angle the **Subject** had inside the **Scenario** during the **Test** at a specific Time. 
The **Test** also contains a set of **AuxilaryData** which are Video, Pictures, SensorData and other files essential for the Test. An entry in **AuxilaryData** consists of its *Test*, the path where to find the file, a short description (optional) and a MimeType to identify the files type.
Another component of the **Test** are the **SurveyResult**'s, which contain the **Subject**s **(Survey)answers** to certain **(Survey)questions** from a **Survey**.

# Access the Database
Could be best described using a Code-Snippet:

```csharp
using (UnitOfWork unitOfWork = new UnitOfWork())
{
	Subject subject = new Subject()
	{
		FirstName = "Max",
		LastName = "Musterman",
		DateOfBirth = new DateTime(1981, 10, 24),
		Gender = Gender.Male,
		CountryCode = "AT",
		Function = "-debug-",
		City = "Hagenberg",
		ServiceRank = "Kloputzer"
	};
	Scenario scenario = new Scenario
	{
		Name = "Balance",
		ExecutionPath = "ich bin eine Biene",
		MinimapPath = "Balance/minimap.png",
		Version = "1.0",

		ZeroX = 1921,
		ZeroY = 257,
		Scale = 1354.0 / 24000.0
	};
	Test test = new Test()
	{
		Subject = subject,
		Scenario = scenario,
		Time = DateTime.Now,
		Title = "Testrun DEBUG - " + DateTime.Now,
		Location = "PLS 5"
	};
	unitOfWork.TestRepository.Insert(test);
	unitOfWork.Save();

	/// ...
	
	Test x = unitOfWork.TestRepository.Get(test => test.Scenario.Name == "Balance").FirstOrDefault();
	Logger.Debug(x?.Scenario.Name + "; " + x?.Subject.FirstName);
}
```
