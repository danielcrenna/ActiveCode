// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ActiveConnection
{
	public abstract class DbMigrationRunner<TOptions> where TOptions : class, IDbConnectionOptions, new()
	{
		private readonly IOptions<TOptions> _options;

		public string ConnectionString { get; }

		protected DbMigrationRunner(string connectionString, IOptions<TOptions> options)
		{
			ConnectionString = connectionString;
			_options = options;
		}
		
		public abstract Task CreateDatabaseIfNotExistsAsync();
		public abstract void ConfigureRunner(IMigrationRunnerBuilder builder);

		public async Task OnStartAsync<TMigrationInfo>()
		{
			if (_options.Value.CreateIfNotExists)
				await CreateDatabaseIfNotExistsAsync();

			if (_options.Value.MigrateOnStartup)
				MigrateUp<TMigrationInfo>();
		}

		public void MigrateUp<TMigrationInfo>()
		{
			MigrateUp(typeof(TMigrationInfo).Assembly, typeof(TMigrationInfo).Namespace);
		}

		public void MigrateTo<TMigrationInfo>(long version)
		{
			MigrateTo(typeof(TMigrationInfo).Assembly, typeof(TMigrationInfo).Namespace, version);
		}

		public void MigrateDown<TMigrationInfo>(long version)
		{
			MigrateDown(typeof(TMigrationInfo).Assembly, typeof(TMigrationInfo).Namespace, version);
		}

		public void MigrateUp(Assembly assembly, string ns)
		{
			var runner = CreateRunnerInstance(assembly, ns);

			runner.MigrateUp();
		}

		public void MigrateTo(Assembly assembly, string ns, long version)
		{
			var runner = CreateRunnerInstance(assembly, ns);

			runner.MigrateUp(version);
		}

		public void MigrateDown(Assembly assembly, string ns, long version)
		{
			var runner = CreateRunnerInstance(assembly, ns);

			runner.MigrateDown(version);
		}

		private IMigrationRunner CreateRunnerInstance(Assembly assembly, string ns)
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
			if (runner is MigrationRunner defaultRunner && defaultRunner.MigrationLoader is DefaultMigrationInformationLoader defaultLoader)
			{
				var source = container.GetRequiredService<IFilteringMigrationSource>();
				defaultRunner.MigrationLoader = new NamespaceMigrationInformationLoader(ns, source, defaultLoader);
			}

			return runner;
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