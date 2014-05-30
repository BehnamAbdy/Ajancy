SELECT     Persons.PersonID, Persons.NationalCode, Persons.FirstName, Persons.LastName, FuelCards.FuelCardID, Cars.CarID, 
                      CarPlateNumbers.CarPlateNumberID, DriverCertificationCars.DriverCertificationCarID, DriverCertificationCars.LockOutDate, 
                      AjancyDrivers.AjancyDriverID, AjancyDrivers.LockOutDate AS Expr1, DriverCertifications.DriverCertificationID
FROM         FuelCardSubstitutions INNER JOIN
                      FuelCards ON FuelCardSubstitutions.PersonalTypeFuelCardID = FuelCards.FuelCardID INNER JOIN
                      Cars ON FuelCards.CarID = Cars.CarID INNER JOIN
                      CarPlateNumbers ON Cars.CarID = CarPlateNumbers.CarID INNER JOIN
                      DriverCertificationCars ON CarPlateNumbers.CarPlateNumberID = DriverCertificationCars.CarPlateNumberID INNER JOIN
                      DriverCertifications ON DriverCertificationCars.DriverCertificationID = DriverCertifications.DriverCertificationID INNER JOIN
                      Persons ON DriverCertifications.PersonID = Persons.PersonID INNER JOIN
                      AjancyDrivers ON DriverCertificationCars.DriverCertificationCarID = AjancyDrivers.DriverCertificationCarID INNER JOIN
                      Ajancies ON AjancyDrivers.AjancyID = Ajancies.AjancyID INNER JOIN
                      Cities ON Ajancies.CityID = Cities.CityID
--WHERE     (FuelCards.FuelCardID = 12024)
WHERE Persons.NationalCode IN ('0386321361')

SELECT     Persons.PersonID, Persons.NationalCode, Persons.FirstName, Persons.LastName, FuelCards.FuelCardID, Cars.CarID, 
                      CarPlateNumbers.CarPlateNumberID, DriverCertificationCars.DriverCertificationCarID, DriverCertificationCars.LockOutDate, 
                      AjancyDrivers.AjancyDriverID, AjancyDrivers.LockOutDate AS Expr1, DriverCertifications.DriverCertificationID
FROM         FuelCardSubstitutions INNER JOIN
                      FuelCards ON FuelCardSubstitutions.PersonalTypeFuelCardID = FuelCards.FuelCardID INNER JOIN
                      Cars ON FuelCards.CarID = Cars.CarID INNER JOIN
                      CarPlateNumbers ON Cars.CarID = CarPlateNumbers.CarID INNER JOIN
                      DriverCertificationCars ON CarPlateNumbers.CarPlateNumberID = DriverCertificationCars.CarPlateNumberID INNER JOIN
                      DriverCertifications ON DriverCertificationCars.DriverCertificationID = DriverCertifications.DriverCertificationID INNER JOIN
                      Persons ON DriverCertifications.PersonID = Persons.PersonID INNER JOIN
                      AjancyDrivers ON DriverCertificationCars.DriverCertificationCarID = AjancyDrivers.DriverCertificationCarID
--WHERE     (FuelCards.FuelCardID = 12024)
WHERE Persons.NationalCode IN ('0386321361')

SELECT     Persons.PersonID, Persons.NationalCode, Persons.FirstName, Persons.LastName, FuelCards.FuelCardID, Cars.CarID, 
                      CarPlateNumbers.CarPlateNumberID, DriverCertificationCars.DriverCertificationCarID, DriverCertificationCars.LockOutDate, 
                      AjancyDrivers.AjancyDriverID, AjancyDrivers.LockOutDate AS Expr1, DriverCertifications.DriverCertificationID
FROM         FuelCardSubstitutions INNER JOIN
                      FuelCards ON FuelCardSubstitutions.PersonalTypeFuelCardID = FuelCards.FuelCardID INNER JOIN
                      Cars ON FuelCards.CarID = Cars.CarID INNER JOIN
                      CarPlateNumbers ON Cars.CarID = CarPlateNumbers.CarID INNER JOIN
                      DriverCertificationCars ON CarPlateNumbers.CarPlateNumberID = DriverCertificationCars.CarPlateNumberID INNER JOIN
                      DriverCertifications ON DriverCertificationCars.DriverCertificationID = DriverCertifications.DriverCertificationID INNER JOIN
                      Persons ON DriverCertifications.PersonID = Persons.PersonID INNER JOIN
                      AjancyDrivers ON DriverCertificationCars.DriverCertificationCarID = AjancyDrivers.DriverCertificationCarID INNER JOIN
                      Ajancies ON AjancyDrivers.AjancyID = Ajancies.AjancyID INNER JOIN
                      Cities ON Ajancies.CityID = Cities.CityID
WHERE Persons.NationalCode IN ('0386321361')

SELECT     Persons.PersonID, Persons.NationalCode, Persons.FirstName, Persons.LastName, FuelCards.FuelCardID, Cars.CarID, 
                      CarPlateNumbers.CarPlateNumberID, DriverCertificationCars.DriverCertificationCarID, DriverCertificationCars.LockOutDate, 
                      AjancyDrivers.AjancyDriverID, AjancyDrivers.LockOutDate AS Expr1, DriverCertifications.DriverCertificationID
FROM         FuelCardSubstitutions INNER JOIN
                      FuelCards ON FuelCardSubstitutions.PersonalTypeFuelCardID = FuelCards.FuelCardID INNER JOIN
                      Cars ON FuelCards.CarID = Cars.CarID INNER JOIN
                      CarPlateNumbers ON Cars.CarID = CarPlateNumbers.CarID INNER JOIN
                      DriverCertificationCars ON CarPlateNumbers.CarPlateNumberID = DriverCertificationCars.CarPlateNumberID INNER JOIN
                      DriverCertifications ON DriverCertificationCars.DriverCertificationID = DriverCertifications.DriverCertificationID INNER JOIN
                      Persons ON DriverCertifications.PersonID = Persons.PersonID INNER JOIN
                      AjancyDrivers ON DriverCertificationCars.DriverCertificationCarID = AjancyDrivers.DriverCertificationCarID INNER JOIN
                      Ajancies ON AjancyDrivers.AjancyID = Ajancies.AjancyID INNER JOIN
                      Cities ON Ajancies.CityID = Cities.CityID
WHERE Persons.NationalCode IN ('0386321361')

SELECT     Persons.PersonID, Persons.NationalCode, Persons.FirstName, Persons.LastName, FuelCards.FuelCardID, Cars.CarID, 
                      CarPlateNumbers.CarPlateNumberID, DriverCertificationCars.DriverCertificationCarID, DriverCertificationCars.LockOutDate, 
                      AjancyDrivers.AjancyDriverID, AjancyDrivers.LockOutDate AS Expr1, DriverCertifications.DriverCertificationID
FROM         FuelCardSubstitutions INNER JOIN
                      FuelCards ON FuelCardSubstitutions.PersonalTypeFuelCardID = FuelCards.FuelCardID INNER JOIN
                      Cars ON FuelCards.CarID = Cars.CarID INNER JOIN
                      CarPlateNumbers ON Cars.CarID = CarPlateNumbers.CarID INNER JOIN
                      DriverCertificationCars ON CarPlateNumbers.CarPlateNumberID = DriverCertificationCars.CarPlateNumberID INNER JOIN
                      DriverCertifications ON DriverCertificationCars.DriverCertificationID = DriverCertifications.DriverCertificationID INNER JOIN
                      Persons ON DriverCertifications.PersonID = Persons.PersonID INNER JOIN
                      AjancyDrivers ON DriverCertificationCars.DriverCertificationCarID = AjancyDrivers.DriverCertificationCarID INNER JOIN
                      Ajancies ON AjancyDrivers.AjancyID = Ajancies.AjancyID INNER JOIN
                      Cities ON Ajancies.CityID = Cities.CityID
WHERE FuelCards.FuelCardID IN(12023, 12024)