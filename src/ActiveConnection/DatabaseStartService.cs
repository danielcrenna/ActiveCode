// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ActiveConnection
{
	internal sealed class DatabaseStartService<TMigrator, TMigrationInfo, TOptions> : IHostedService 
		where TMigrator : DbMigrationRunner<TOptions> 
		where TOptions : class, IDbConnectionOptions, new()
	{
		private readonly TMigrator _migrator;
		public DatabaseStartService(TMigrator migrator) => _migrator = migrator;
		public async Task StartAsync(CancellationToken cancellationToken) => await _migrator.OnStartAsync<TMigrationInfo>();
		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}