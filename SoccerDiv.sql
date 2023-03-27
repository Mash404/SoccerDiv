--Sports Table--
CREATE TABLE [dbo].[Sports]
(
	--Primary Key--
	[Sports_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Sports_Name] NVARCHAR(50) NOT NULL,
	[Sports_Description] NVARCHAR(1000) NOT NULL,
	[Sports_Image] VARCHAR(MAX) NULL,
)

--Team Table--
CREATE TABLE [dbo].[Team]
(
	--Primary Key--
	[Team_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	--Foreign Key ( Sports Name )--
	[Sports_ID] int NOT NULL FOREIGN KEY REFERENCES Sports(Sports_ID),
	[Team_Name] NVARCHAR(100) NOT NULL,
	[Team_Description] NVARCHAR(1000) NOT NULL,
	[Team_Rating] FLOAT,
	[Team_Ranking] SMALLINT,
	[Team_Logo] VARCHAR(MAX) NULL,
	[Team_Country] VARCHAR(100) NULL,
)

--User Table
CREATE TABLE [dbo].[Customer]
(
	--Primary Key--
	[Customer_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	--Foreign Key ( Favourite Team )--
	[Team_ID] INT NOT NULL FOREIGN KEY REFERENCES Team(Team_ID),
	[Customer_Name] NVARCHAR(100) NOT NULL,
	[Customer_Email] NVARCHAR(50) NOT NULL UNIQUE,
	[Customer_PhoneNO] NVARCHAR(50) NOT NULL,
	[Customer_Address] NVARCHAR(200) NOT NULL,
	[Customer_Password] NVARCHAR(100) NOT NULL,
	[Customer_Image] VARCHAR(MAX) NULL,
)

--Player Table--
CREATE TABLE [dbo].[Player]
(
	--Primary Key--
	[Player_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	--Foreign Key ( Sports Name )
	[Sports_ID] int NOT NULL FOREIGN KEY REFERENCES Sports(Sports_ID),
	--Foreign Key ( Team Name )
	[Team_ID] INT NOT NULL FOREIGN KEY REFERENCES Team(Team_ID),
	[Player_Name] NVARCHAR(50) NOT NULL,
	[Player_Age] INT NOT NULL,
	[Player_Nationality] NVARCHAR(100) NOT NULL,
	[Player_Position] NVARCHAR(50) NOT NULL,
	[Player_Rating] FLOAT,
	[Player_Ranking] SMALLINT,
	[Player_Image] VARCHAR(MAX) NULL,
)

--Tournament Table--
CREATE TABLE [dbo].[Tournamnet]
(
	--Primary Key--
	[Tournament_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	--Foreign Key ( Sports Name )--
	[Sports_ID] INT NOT NULL FOREIGN KEY REFERENCES Sports(Sports_ID),
	[Tournament_Name] NVARCHAR(50),
	[Tournament_Description] NVARCHAR(1000),
	[Tournament_Logo] VARCHAR(MAX) NULL,
)

--Venue Table--
CREATE TABLE [dbo].[Venue]
(
	--Primary Key--
	[Venue_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Venue_Name] NVARCHAR(50),
	[Venue_Capacity] INT,
	[Venue_Location] NVARCHAR(1000),
	[Venue_Image] VARCHAR(MAX) NULL,
)

--Event Table--
CREATE TABLE [dbo].[Event]
(
	--Primary Key--
	[Event_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	--Foreign Key ( Sports Name )--
	[Sports_ID] INT NOT NULL FOREIGN KEY REFERENCES Sports(Sports_ID),
	--Foreign Key ( Tournament Name )--
	[Tournament_ID] INT NOT NULL FOREIGN KEY REFERENCES Tournamnet(Tournament_ID),
	--Foreign Key ( Team Name )--
	[First_Team] INT NOT NULL FOREIGN KEY REFERENCES Team(Team_ID),
	--Foreign Key ( Team Name )--
	[Second_Team] INT NOT NULL FOREIGN KEY REFERENCES Team(Team_ID),
	--Foreign Key ( Venue Name )--
	[Venue_ID] INT NOT NULL FOREIGN KEY REFERENCES Venue(Venue_ID),
	[Event_Date] DATE,
	[Event_Time] TIME,
	[Event-Details] NVARCHAR(1000),
)

--ContactUS Table--
CREATE TABLE [dbo].[Contact]
(
	[Contact_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[C_Name] NVARCHAR(100),
	[C_Email] NVARCHAR(100),
	[C_Message] NVARCHAR(500), 
)

--Coach Table--
CREATE TABLE [dbo].[Coach]
(
	[Coach_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	--Foreign Key ( Sports Name )--
	[Sports_ID] INT NOT NULL FOREIGN KEY REFERENCES Sports(Sports_ID),
	--Foreign Key ( Team Name )
	[Team_ID] INT NOT NULL FOREIGN KEY REFERENCES Team(Team_ID),
	[Coach_Name] NVARCHAR(100),
	[Coach_Age] INT, 
	[Coach_Nationality] Varchar(100),
	[Coach_Image] VARCHAR(MAX),
)

--Admin Table--
CREATE TABLE [dbo].[Admin]
(
	[Admin_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Admin_Name] NVARCHAR(100),
	[Admin_Email] NVARCHAR(100),
	[Admin_Password] NVARCHAR(100),
	[Admin_Image] VARCHAR(MAX) NULL,
	[Admin_Dexcription] NVARCHAR(200),
)

--Admin Table--
CREATE TABLE [dbo].[News]
(
	[News_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	--Foreign Key ( Team Name )--
	[Customer_ID] INT NOT NULL FOREIGN KEY REFERENCES Customer(Customer_ID),
	[News_Title] NVARCHAR(100),
	[News_Date] Date,
	[NewsTime] Time,
	[News_Image] VARCHAR(MAX) NULL,
	[News_Dexcription] NVARCHAR(200),
)