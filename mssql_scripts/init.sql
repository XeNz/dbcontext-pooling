
IF NOT EXISTS (SELECT name
               FROM sys.databases
               WHERE name = 'TestDb')
CREATE DATABASE [TestDb];