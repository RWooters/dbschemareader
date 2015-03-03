﻿using DatabaseSchemaReader.ProviderSchemaReaders;
using DatabaseSchemaReaderTest.IntegrationTests;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestContext = System.Object;
#endif

namespace DatabaseSchemaReaderTest.ProviderSchemaReaders
{

    /// <summary>
    ///     These are INTEGRATION tests using databases.
    /// </summary>
    [TestClass]
    public class SqlAzureOrSqlServerSchemaReaderTests
    {
        /// <summary>
        /// Test whether IsAzureSqlDatabase is correctly set to true when connecting to an SQL Database instance on Azure 
        /// </summary>
        [TestMethod]
        public void DetectsSqlAzure()
        {
            const string providername = "System.Data.SqlClient";
            const string connectionString = @"Server=tcp:SERVERNAME.database.windows.net,1433;Database=DBNAME;User ID=USERNAME@SERVERNAME;Password=PASSWORD;Trusted_Connection=False;Encrypt=True;";
            ProviderChecker.Check(providername, connectionString);
            var target = new SqlAzureOrSqlServerSchemaReader(connectionString, "System.Data.SqlClient");

            var actual = target.IsAzureSqlDatabase;

            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Test whether IsAzureSqlDatabase is correctly set to false when connecting to regular SQL Server
        /// </summary>
        [TestMethod]
        public void DetectsSqlServer()
        {
            const string providername = "System.Data.SqlClient";
            const string connectionString = ConnectionStrings.Northwind;
            ProviderChecker.Check(providername, connectionString);
            var target = new SqlAzureOrSqlServerSchemaReader(connectionString, providername);

            var actual = target.IsAzureSqlDatabase;

            Assert.IsFalse(actual);
        }
    }
}