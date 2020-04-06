// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace ActiveResolver.Api
{
	internal sealed class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
	{
		private readonly ILogger<MvcOptions> _logger;
		private readonly ObjectPoolProvider _objectPoolProvider;

		public ConfigureMvcOptions(ILogger<MvcOptions> logger, ObjectPoolProvider objectPoolProvider)
		{
			_logger = logger;
			_objectPoolProvider = objectPoolProvider;
		}

		public void Configure(MvcOptions options)
		{
			options.OutputFormatters.Add((IOutputFormatter) new GraphVizOutputFormatter());
		}
	}
}