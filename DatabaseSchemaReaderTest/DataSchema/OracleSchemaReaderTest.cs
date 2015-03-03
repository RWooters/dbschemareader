﻿using DatabaseSchemaReader.DataSchema;
using DatabaseSchemaReader.ProviderSchemaReaders;
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

namespace DatabaseSchemaReaderTest.DataSchema
{
    [TestClass]
    public class OracleSchemaReaderTest
    {
        [TestMethod]
        public void NoTriggers()
        {
            //arrange
            var osr = new OracleSchemaReader(ConnectionStrings.OracleHr, "System.Data.OracleClient");
            var dt = new DatabaseTable();
            dt
                .AddColumn("ID")
                .AddPrimaryKey()
                .AddColumn("NAME");

            //act
            osr.PostProcessing(dt);

            //assert
            Assert.IsFalse(dt.HasAutoNumberColumn);

        }

        [TestMethod]
        public void TriggerWithQuotes()
        {
            //arrange
            var osr = new OracleSchemaReader(ConnectionStrings.OracleHr, "System.Data.OracleClient");
            var dt = new DatabaseTable();
            dt
                .AddColumn("ID")
                .AddPrimaryKey()
                .AddColumn("NAME");
            dt.Triggers.Add(new DatabaseTrigger
            {
                //generated by SqlDeveloper
                TriggerBody = @"CREATE OR REPLACE TRIGGER ""DB"".""MYTRIGGER"" before insert on ""DB"".""TABLE1""    for each row begin     if inserting then       if :NEW.""ID"" is null then          select MY_SEQ.nextval into :NEW.""ID"" from dual;       end if;    end if; end;"
            });

            //act
            osr.PostProcessing(dt);

            //assert
            Assert.IsTrue(dt.HasAutoNumberColumn);
        }

        [TestMethod]
        public void TriggerWithNoQuotes()
        {
            //arrange
            var osr = new OracleSchemaReader(ConnectionStrings.OracleHr, "System.Data.OracleClient");
            var dt = new DatabaseTable();
            dt
                .AddColumn("ID")
                .AddPrimaryKey()
                .AddColumn("NAME");
            dt.Triggers.Add(new DatabaseTrigger
            {
                //with spaces, line breaks
                TriggerBody = @"BEGIN
    SELECT MY_SEQ.NEXTVAL
    INTO :NEW.ID
    FROM DUAL;
END;"
            });

            //act
            osr.PostProcessing(dt);

            //assert
            Assert.IsTrue(dt.HasAutoNumberColumn);
        }
    }
}
