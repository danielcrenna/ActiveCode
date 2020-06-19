// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ActiveResolver.Api
{
	public static class Add
	{
		public static IServiceCollection AddGraphViz(this IServiceCollection services)
		{
			services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureMvcOptions>();
			return services;
		}
	}
}