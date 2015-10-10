using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Net.Core.Data.UnitTest.Dto;
using SimpleNet.Data.Mapper;

namespace SampleNet.Data.UnitTest
{
    [TestClass]
    public class DatabaseConnectionAndTransactionUnitTest
    {

        static DatabaseConnectionAndTransactionUnitTest()
        {
            var path = Path.GetFullPath(@"..\..\App_Data");

            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }

        [TestMethod]
        public void TestDatabaseConnectionSharing()
        {

            var mapper = MapBuilder<State>.BuildAllProperties();
            const string SQL = "SELECT * FROM State s WHERE s.Name = @Name";

            var dal = new SimpleNet.Data.Repository.SimpleDataAccess("Simple");

            using (var connection = dal.GetConnection())
            {
                var alabama = dal.Read(connection, mapper, SQL, CommandType.Text, new[]
                 {
                    dal.GetDbParameter("@Name", "Alabama")
                }).FirstOrDefault();

                Assert.IsNotNull(alabama);
                Assert.IsTrue(alabama.Name.Equals("Alabama"));


                var georgia = dal.Read(connection, mapper, SQL, CommandType.Text, new[]
                {
                    dal.GetDbParameter("@Name", "georgia")
                }).FirstOrDefault();

                Assert.IsNotNull(georgia);
                Assert.IsTrue(georgia.Name.Equals("Georgia"));
            }
        }






        [TestMethod]
        public void TestTransaction()
        {

            var mapper = MapBuilder<State>.BuildAllProperties();
            const string SQL = "SELECT * FROM State s WHERE s.Name = @Name";

            var dal = new SimpleNet.Data.Repository.SimpleDataAccess("Simple");

            using (var connection = dal.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var alabama = dal.Read(connection, mapper, SQL, CommandType.Text, new[]
                    {
                        dal.GetDbParameter("@Name", "Alabama")
                    }, transaction).FirstOrDefault();

                    Assert.IsNotNull(alabama);
                    Assert.IsTrue(alabama.Name.Equals("Alabama"));

                    dal.ExecuteNonQuery(connection,
                        "UPDATE STATE Set Name = @Name WHERE Id=@Id",
                        CommandType.Text, 
                        new[]
                        {
                            dal.GetDbParameter("@Name", "Alabama2"),
                            dal.GetDbParameter("@Id", alabama.Id)
                        },
                        transaction);
                    
                    var alabama2 = dal.Read(connection, mapper, SQL, CommandType.Text, new[]
                    {
                        dal.GetDbParameter("@Name", "Alabama2")
                    }, transaction).FirstOrDefault();

                    Assert.IsNotNull(alabama2);
                    Assert.IsTrue(alabama2.Name.Equals("Alabama2"));


                    // Roll back the transaction
                    transaction.Rollback();
                    

                    alabama = dal.Read(connection, mapper, SQL, CommandType.Text, new[]
                    {
                        dal.GetDbParameter("@Name", "Alabama")
                    }, transaction).FirstOrDefault();

                    Assert.IsNotNull(alabama);
                    Assert.IsTrue(alabama.Name.Equals("Alabama"));


                }


            }
        }




    }
}
