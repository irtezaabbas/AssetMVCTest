CREATE TABLE [dbo].[Assets]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AssetTag] NCHAR(20) NOT NULL, 
    [AssetState] NCHAR(30) NOT NULL, 
    [UsedBy] NCHAR(50) NOT NULL, 
    [UsageType] NCHAR(30) NOT NULL, 
    [AssignedOn] SMALLDATETIME NOT NULL
)
