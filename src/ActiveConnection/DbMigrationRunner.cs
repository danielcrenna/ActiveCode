// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveConnection
{
	public abstract class DbMigrationRunner
	{
		protected DbMigrationRunner(string connectionString) => ConnectionString = connectionString;
		public string ConnectionString { get; }

		public abstract void CreateDatabaseIfNotExists();
		public abstract void ConfigureRunner(IMigrationRunnerBuilder builder);

		public void MigrateUp<T>()
		{
			MigrateUp(typeof(T).Assembly, typeof(T).Namespace);
		}

		public void MigrateUp(Assembly assembly, string ns)
		{
			var container = new ServiceCollection()
				.AddFluentMigratorCore()
				.ConfigureRunner(
					builder =>
					{
						ConfigureRunner(builder);
						builder
							.WithGlobalConnectionString(ConnectionString)
							.ScanIn(assembly).For.Migrations();
					})
				.BuildServiceProvider();

			var runner = container.GetRequiredService<IMigrationRunner>();
			if (runner is MigrationRunner defaultRunner &&
			    defaultRunner.MigrationLoader is DefaultMigrationInformationLoader defaultLoader)
			{
				var source = container.GetRequiredService<IFilteringMigrationSource>();
				defaultRunner.MigrationLoader = new NamespaceMigrationInformationLoader(ns, source, defaultLoader);
			}

			runner.MigrateUp();
		}

		private class NamespaceMigrationInformationLoader : IMigrationInformationLoader
		{
			private readonly DefaultMigrationInformationLoader _inner;
			private readonly string _namespace;
			private readonly IFilteringMigrationSource _source;

			public NamespaceMigrationInformationLoader(string @namespace,
				IFilteringMigrationSource source, DefaultMigrationInformationLoader inner)
			{
				_namespace = @namespace;
				_source = source;
				_inner = inner;
			}

			public SortedList<long, IMigrationInfo> LoadMigrations()
			{
				var migrations =
					_source.GetMigrations(type => type.Namespace == _namespace)
						.Select(_inner.Conventions.GetMigrationInfoForMigration);

				var list = new SortedList<long, IMigrationInfo>();
				foreach (var entry in migrations)
				{
					list.Add(entry.Version, entry);
				}

				return list;
			}
		}
	}
}