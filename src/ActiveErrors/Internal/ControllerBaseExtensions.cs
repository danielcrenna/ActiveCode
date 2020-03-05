// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ActiveErrors.Internal
{
	internal static class ControllerBaseExtensions
	{
		internal static ControllerBase SetHeader(this ControllerBase controller, string name, StringValues values)
		{
			controller.Response.Headers[name] = values;
			return controller;
		}
	}
}