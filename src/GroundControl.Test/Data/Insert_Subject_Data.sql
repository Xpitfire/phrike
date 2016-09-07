BULK INSERT Subject FROM 'C:\public\subject_data_null.csv'
WITH(FIRSTROW = 2, FIELDTERMINATOR = ',', ROWTERMINATOR = '\n')