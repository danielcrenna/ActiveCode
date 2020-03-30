using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ActiveConnection.Tests
{
	public class ConnectionScopeTests : DatabaseFixture
    {
        public class SomeService
        {
            private readonly IDataConnection<SomeService> _db;

            public SomeService(IDataConnection<SomeService> db)
            {
                _db = db;
            }

            public IDbConnection Connection => _db.Current;
        }

        public class SomeOtherService
        {
            private readonly IDataConnection<SomeOtherService> _db;

            public SomeOtherService(IDataConnection<SomeOtherService> db)
            {
                _db = db;
            }

            public IDbConnection Connection => _db.Current;
        }
		
		[Fact]
        public void Keep_alive()
        {
            var cs = CreateConnectionString();
            var services = new ServiceCollection();
            services.AddDatabaseConnection<ConnectionScopeTests, SqliteConnectionFactory>(cs, ConnectionScope.KeepAlive);

            var provider = services.BuildServiceProvider();
            var connection1 = provider.GetService<DataContext>();
            var connection2 = provider.GetService<DataContext>();
            Assert.Equal(connection1, connection2);
        }

        [Fact]
        public void Keep_alive_data_connection()
        {
            var cs = CreateConnectionString();
            var services = new ServiceCollection();
            services.AddDatabaseConnection<SomeService, SqliteConnectionFactory>(cs, ConnectionScope.KeepAlive);
            services.AddDatabaseConnection<SomeOtherService, SqliteConnectionFactory>(cs, ConnectionScope.KeepAlive);
            services.AddSingleton<SomeService>();
            services.AddSingleton<SomeOtherService>();

            var provider = services.BuildServiceProvider();
            var one = provider.GetService<SomeService>();
            var two = provider.GetService<SomeOtherService>();

            Assert.NotEqual(one.Connection, two.Connection);
        }
    }
}
