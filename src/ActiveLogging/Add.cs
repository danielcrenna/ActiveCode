// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using ActiveLogging.Internal;
using ActiveOptions;
using ActiveStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace ActiveLogging
{
	public static class Add
	{
		private static readonly ActionTraceListener ActionTraceListener = new ActionTraceListener(Console.Write, Console.WriteLine);

		public static IServiceCollection AddSafeLogging(this IServiceCollection services)
		{
			services.AddLogging();
			services.TryAddSingleton<ISafeLogger, SafeLogger>();
			services.TryAddSingleton(typeof(ISafeLogger<>), typeof(SafeLogger<>));
			return services;
		}

		public static ILoggingBuilder AddTraceLogging(this ILoggingBuilder builder)
		{
			lock (Trace.Listeners)
			{
				Trace.UseGlobalLock = true;
				if (!Trace.Listeners.Contains(ActionTraceListener))
					Trace.Listeners.Add(ActionTraceListener);
			}

			return builder;
		}

		public static IServiceCollection AddTraceLogging(this IServiceCollection services)
		{
			lock (Trace.Listeners)
			{
				Trace.UseGlobalLock = true;
				if (!Trace.Listeners.Contains(ActionTraceListener))
					Trace.Listeners.Add(ActionTraceListener);
			}

			return services;
		}

		public static ILoggingBuilder AddSafeLogging(this ILoggingBuilder builder)
		{
			builder.Services.AddSafeLogging();
			return builder;
		}

		public static ILoggingBuilder AddActiveStorage(this ILoggingBuilder builder, IConfiguration config) => 
			builder.AddActiveStorage(config.FastBind);

		public static ILoggingBuilder AddActiveStorage(this ILoggingBuilder builder, Action<ActiveStorageLoggerOptions> configureAction = null)
		{
			if (configureAction != null)
				builder.Services.Configure(configureAction);

			builder.AddConfiguration();
			
			builder.Services.AddHttpContextAccessor();
			builder.Services.TryAddSingleton<ILogReceiver, HttpContextLogReceiver>();
			builder.Services.TryAddTransient<IAsyncLogFlusher, ActiveStorageAsyncLogFlusher>();
			
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ActiveStorageLoggerProvider>());
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataMigratorInfoProvider, ActiveLoggingMigratorInfoProvider>());

			return builder;
		}
	}
}