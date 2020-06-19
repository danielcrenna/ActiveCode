// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using ActiveErrors.Internal;
using Microsoft.AspNetCore.Mvc;

namespace ActiveErrors
{
	public static class StatusCodeExtensions
	{
		public static IActionResult StatusCode(this ControllerBase controller, HttpStatusCode code)
		{
			return controller.StatusCode((int) code);
		}

		public static IActionResult StatusCode(this ControllerBase controller, HttpStatusCode code, object value)
		{
			return controller.StatusCode((int) code, value);
		}

		public static IActionResult NotModified(this ControllerBase controller)
		{
			return controller.StatusCode(HttpStatusCode.NotModified);
		}

		public static IActionResult NotModified(this ControllerBase controller, object value)
		{
			return controller.StatusCode(HttpStatusCode.NotModified, value);
		}

		public static IActionResult NotImplemented(this ControllerBase controller)
		{
			return controller.StatusCode(HttpStatusCode.NotImplemented);
		}

		public static IActionResult NotImplemented(this ControllerBase controller, object value)
		{
			return controller.StatusCode(HttpStatusCode.NotImplemented, value);
		}

		public static IActionResult Gone(this ControllerBase controller)
		{
			return controller.StatusCode(HttpStatusCode.Gone);
		}

		public static IActionResult Gone(this ControllerBase controller, object value)
		{
			return controller.StatusCode(HttpStatusCode.Gone, value);
		}

		public static IActionResult UnsupportedMediaType(this ControllerBase controller)
		{
			return controller.StatusCode(HttpStatusCode.UnsupportedMediaType);
		}

		public static IActionResult UnsupportedMediaType(this ControllerBase controller, object value)
		{
			return controller.StatusCode(HttpStatusCode.UnsupportedMediaType, value);
		}

		public static IActionResult NotAcceptable(this ControllerBase controller)
		{
			return controller.StatusCode(HttpStatusCode.NotAcceptable);
		}

		public static IActionResult NotAcceptable(this ControllerBase controller, object value)
		{
			return controller.StatusCode(HttpStatusCode.NotAcceptable, value);
		}

		public static IActionResult SeeOther(this ControllerBase controller, string location)
		{
			return controller.SetHeader(HttpHeaders.Location, location).StatusCode(HttpStatusCode.SeeOther);
		}

		public static IActionResult SeeOther(this ControllerBase controller, string location, object value)
		{
			return controller.SetHeader(HttpHeaders.Location, location).StatusCode(HttpStatusCode.SeeOther, value);
		}

		public static IActionResult Created(this ControllerBase controller, string location)
		{
			return controller.SetHeader(HttpHeaders.Location, location).StatusCode(HttpStatusCode.Created);
		}

		public static IActionResult Created(this ControllerBase controller, string location, object value)
		{
			return controller.SetHeader(HttpHeaders.Location, location).StatusCode(HttpStatusCode.Created, value);
		}
	}
}