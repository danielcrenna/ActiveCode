// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace ActiveLogging
{
	public static class Add
	{
		private static readonly ActionTraceListener ActionTraceListener =
			new ActionTraceListener(Console.Write, Console.WriteLine);

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
	}
}