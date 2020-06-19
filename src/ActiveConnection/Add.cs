// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using ActiveResolver;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveConnection
{
	public static class Add
	{
		private static readonly ConcurrentDictionary<string, DependencyContainer> Containers =
			new ConcurrentDictionary<string, DependencyContainer>();

		public static IServiceCollection AddDatabaseConnection<TScope, TConnectionFactory>(
			this IServiceCollection services, string connectionString,
			ConnectionScope scope = ConnectionScope.AlwaysNew)
			where TConnectionFactory : class, IDbConnectionFactory, new()
		{
			var slot = $"{typeof(TScope).FullName}";

			var factory = new TConnectionFactory {ConnectionString = connectionString};
			services.AddSingleton(factory);

			if (!Containers.TryGetValue(slot, out var container))
			{
				var serviceProvider = services.BuildServiceProvider();
				container = new DependencyContainer(serviceProvider);
				Containers.TryAdd(slot, container);

				if (!Containers.TryGetValue(slot, out container))
					throw new ArgumentException($"Could not initialize container with slot {slot}", slot);
			}

			container.Register(slot, factory);

			switch (scope)
			{
				case ConnectionScope.AlwaysNew:
					container.Register(slot, r => new DataContext(r.Resolve<TConnectionFactory>(slot)));
					container.Register<IDataConnection>(slot, r => new DataConnection(r.Resolve<DataContext>(slot)));
					container.Register<IDataConnection<TScope>>(r =>
						new DataConnection<TScope>(r.Resolve<DataContext>(slot)));
					break;
				case ConnectionScope.ByRequest:
					container.Register(slot, r => new DataContext(r.Resolve<TConnectionFactory>(slot)),
						InstanceIsUnique.PerHttpRequest);
					container.Register<IDataConnection>(slot, r => new DataConnection(r.Resolve<DataContext>(slot)),
						InstanceIsUnique.PerHttpRequest);
					container.Register<IDataConnection<TScope>>(
						r => new DataConnection<TScope>(r.Resolve<DataContext>(slot)), InstanceIsUnique.PerHttpRequest);
					break;
				case ConnectionScope.ByThread:
					container.Register(slot, r => new DataContext(r.Resolve<TConnectionFactory>(slot)),
						InstanceIsUnique.PerThread);
					container.Register<IDataConnection>(slot, r => new DataConnection(r.Resolve<DataContext>(slot)),
						InstanceIsUnique.PerThread);
					container.Register<IDataConnection<TScope>>(
						r => new DataConnection<TScope>(r.Resolve<DataContext>(slot)), InstanceIsUnique.PerThread);
					break;
				case ConnectionScope.KeepAlive:
					container.Register(slot, r => new DataContext(r.Resolve<TConnectionFactory>(slot)),
						InstanceIsUnique.PerProcess);
					container.Register<IDataConnection>(slot, r => new DataConnection(r.Resolve<DataContext>(slot)),
						InstanceIsUnique.PerProcess);
					container.Register<IDataConnection<TScope>>(
						r => new DataConnection<TScope>(r.Resolve<DataContext>(slot)), InstanceIsUnique.PerProcess);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
			}

			services.AddTransient(r => container.Resolve<IDataConnection<TScope>>());
			return services;
		}

		public static IServiceCollection AddDatabaseConnection<TConnectionFactory>(
			this IServiceCollection services, string slot, string connectionString,
			ConnectionScope scope = ConnectionScope.AlwaysNew)
			where TConnectionFactory : class, IDbConnectionFactory, new()
		{
			var factory = new TConnectionFactory {ConnectionString = connectionString};
			services.AddSingleton(factory);

			if (!Containers.TryGetValue(slot, out var container))
			{
				var serviceProvider = services.BuildServiceProvider();
				container = new DependencyContainer(serviceProvider);
				Containers.TryAdd(slot, container);

				if (!Containers.TryGetValue(slot, out container))
					throw new ArgumentException($"Could not initialize container with slot {slot}", slot);
			}

			container.Register(slot, factory);

			switch (scope)
			{
				case ConnectionScope.AlwaysNew:
					container.Register(slot, r => new DataContext(r.Resolve<TConnectionFactory>(slot)));
					container.Register<IDataConnection>(slot, r => new DataConnection(r.Resolve<DataContext>(slot)));
					container.Register<IDataConnection<object>>(r => new DataConnection<object>(r.Resolve<DataContext>(slot)));
					break;
				case ConnectionScope.ByRequest:
					container.Register(slot, r => new DataContext(r.Resolve<TConnectionFactory>(slot)),
						InstanceIsUnique.PerHttpRequest);
					container.Register<IDataConnection>(slot, r => new DataConnection(r.Resolve<DataContext>(slot)),
						InstanceIsUnique.PerHttpRequest);
					container.Register<IDataConnection<object>>(
						r => new DataConnection<object>(r.Resolve<DataContext>(slot)), InstanceIsUnique.PerHttpRequest);
					break;
				case ConnectionScope.ByThread:
					container.Register(slot, r => new DataContext(r.Resolve<TConnectionFactory>(slot)),
						InstanceIsUnique.PerThread);
					container.Register<IDataConnection>(slot, r => new DataConnection(r.Resolve<DataContext>(slot)),
						InstanceIsUnique.PerThread);
					container.Register<IDataConnection<object>>(
						r => new DataConnection<object>(r.Resolve<DataContext>(slot)), InstanceIsUnique.PerThread);
					break;
				case ConnectionScope.KeepAlive:
					container.Register(slot, r => new DataContext(r.Resolve<TConnectionFactory>(slot)),
						InstanceIsUnique.PerProcess);
					container.Register<IDataConnection>(slot, r => new DataConnection(r.Resolve<DataContext>(slot)),
						InstanceIsUnique.PerProcess);
					container.Register<IDataConnection<object>>(
						r => new DataConnection<object>(r.Resolve<DataContext>(slot)), InstanceIsUnique.PerProcess);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
			}

			services.AddTransient(r => container.Resolve<IDataConnection<object>>());
			return services;
		}
	}
}