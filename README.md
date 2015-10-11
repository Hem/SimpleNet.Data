#SimpleNet.Data

SimpleNet.Data is an implementation of a data access layer built on top of ADO.NET technologies with intention of simplifying your DAL/Repository Layers



# HOW TO 


1. Include the SimpleNet.Data in your application

2. Add a connection string with a provider name in your App.config or Web.config

```
     <add name="Simple"
          connectionString="Data Source=..."
          providerName="System.Data.SqlClient" />
```


3. New up an instance of SimpleDataAccess and execute a query
    
```
          var dal = new Simple.Data.Repository.SimpleDataAccess("connection name");
          
          var dataTable = dal.ReadSql("SELECT * FROM USERS", null);
          
          var dataTable = dal.ReadSql("SELECT * FROM Users u where u.UserId = @UserId", new []{
              dal.GetDbParameter("@UserId", 123)
          });
```






## Querying objects instead of DataTables


To simplify development I would recommend working with Objects instead of DataTable along with an IRepository<T> pattern.


1.   Create a BaseSqlRepository extending AbstractSqlRepository


```
    public class BaseSqlRepository : AbstractSimpleSqlRepository
    {
          public override sealed ISimpleDataAccess Database { get; set; }

         public BaseSqlRepository()
         {
               Database = new SimpleDataAccess("<Your_App_Connection_String_Name>");
         }
    }
```


2.   Create your Repository class by extending BaseSqlRepository

```
    public class StateRepository : BaseSqlRepository, IStateRepository
    {
         // Please review documentation from Enterprise Libary Db Accessors to Find out more about IRowMapper
         
         private static readonly IRowMapper<State> StateMapper = MapBuilder<State>.BuildAllProperties();

         // sample 2
         private static readonly IRowMapper<State> StateRowMapper = MapBuilder<State>.MapAllProperties().Build();

          // sample 3 for mapping
         private static readonly IRowMapper<State> StateRowMapper2 = MapBuilder<State>
                   .MapNoProperties()
                   .MapByName(x => x.Id)
                   .Map(x => x.Code).ToColumn("Code")
                   .Map(x => x.Name).WithFunc(x => x["Name"].ToString())
                   .Build();



		public State GetById(int id)
		{
				const string SQL = @"SELECT Id, Code, Name FROM STATE s where s.Id = @Id";

				return Read<State>(StateMapper, SQL, CommandType.Text, new[]
				{
					GetDbParameter("@Id", id)
				}).FirstOrDefault();
		}


		public State GetAll(int id)
         	{
			const string SQL = @"SELECT Id, Code, Name FROM STATE s  ORDER BY s.Name ";

			return Read<State>(StateMapper, SQL, CommandType.Text, null).FirstOrDefault();
         	}


		public DataTable ReadById(int id)
		{
			const string SQL = @"SELECT Id, Code, Name FROM STATE s where s.Id = @Id";

			return Read(SQL, CommandType.Text, new[]
			{
				GetDbParameter("@Id", id)
			});
		}


		public State Create(State state)
		{
			const string SQL = @"INSERT INTO STATE (Code, Name)
								 VALUES (@Code, @Name);
								 SELECT SCOPE_IDENTITY()
									 ";

			var id = ExecuteScalar(SQL, CommandType.Text, new[]
			{
				GetDbParameter("@Code", state.Code),
				GetDbParameter("@Name", state.Name)
			});

			state.Id = Convert.ToInt32(id);

			return state;
		}


		public State Update(State state)
		{
			const string SQL = @" UPDATE STATE
			SET Code = @Code,
			Name = @Name
			WHERE Id = @Id
			";

			var id = ExecuteNonQuery(SQL, CommandType.Text, new[]
			{
				GetDbParameter("@Code", state.Code),
				GetDbParameter("@Name", state.Name),
				GetDbParameter("@Id", state.Id)
			});

			return state;
		}

    }

```


## Working with database transactions



```
    using (var connection = dal.GetConnection())
    {
        using (var transaction = connection.BeginTransaction())
        {
            try{
        
                dal.ExecuteNonQuery(connection, ..... , transaction);
                ...     
                dal.ExecuteScalar(connection, ..... , transaction);
                ...
                transaction.Commit();

            }catch(){
                transaction.Rollback();
            }
        }
    }
```
