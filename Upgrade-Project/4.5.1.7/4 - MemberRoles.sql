 ALTER TABLE [Members]
ADD Roles int
NULL DEFAULT(0)
Go

 ALTER TABLE [Pages]
ADD AccessRoles int
NULL DEFAULT(0)
Go


 ALTER TABLE [Pages]
ADD EditingRoles int
NULL DEFAULT(0)
Go