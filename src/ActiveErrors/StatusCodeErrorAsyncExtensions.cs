// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ActiveErrors
{
	public static class StatusCodeErrorAsyncExtensions
	{
		public static IActionResult StatusCodeError(this ControllerBase controller, long eventId, string errorMessage,
			HttpStatusCode statusCode, params object[] args)
		{
			return new ErrorObjectResult(new Error(eventId, errorMessage, statusCode), args);
		}

		public static IActionResult BadRequestError(this ControllerBase controller, long eventId, string errorMessage,
			params object[] args)
		{
			return controller.StatusCodeError(eventId, errorMessage, HttpStatusCode.BadRequest, args);
		}

		public static IActionResult NotAcceptableError(this ControllerBase controller, long eventId,
			string errorMessage, params object[] args)
		{
			return controller.StatusCodeError(eventId, errorMessage, HttpStatusCode.NotAcceptable, args);
		}

		public static IActionResult NotFoundError(this ControllerBase controller, long eventId, string errorMessage,
			params object[] args)
		{
			return controller.StatusCodeError(eventId, errorMessage, HttpStatusCode.NotFound, args);
		}

		public static IActionResult InternalServerError(this ControllerBase controller, long eventId,
			string errorMessage, params object[] args)
		{
			return controller.StatusCodeError(eventId, errorMessage, HttpStatusCode.InternalServerError, args);
		}

		public static IActionResult UnprocessableEntityError(this ControllerBase controller, long eventId,
			string errorMessage, params object[] args)
		{
			return controller.StatusCodeError(eventId, errorMessage, HttpStatusCode.UnprocessableEntity, args);
		}

		public static IActionResult Error(this ControllerBase controller, Error error, params object[] args)
		{
			return new ErrorObjectResult(error, args);
		}
	}
}