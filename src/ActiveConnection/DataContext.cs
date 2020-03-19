// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data;

namespace ActiveConnection
{
	public class DataContext : IDisposable
	{
		private static readonly object Sync = new object();

		private readonly IDbConnectionFactory _dbConnectionFactory;

		private volatile IDbConnection _connection;

		public DataContext(IDbConnectionFactory dbConnectionFactory) => _dbConnectionFactory = dbConnectionFactory;

		public IDbConnection Connection => GetConnection();

		public void Dispose()
		{
			_connection?.Dispose();
			_connection = null;
		}

		private IDbConnection GetConnection()
		{
			PrimeConnection();
			return _connection;
		}

		protected void PrimeConnection()
		{
			if (_connection != null) return;
			lock (Sync)
			{
				if (_connection != null) return;
				var connection = _dbConnectionFactory.CreateConnection();
				connection.Open();
				_connection = connection;
			}
		}
	}
}