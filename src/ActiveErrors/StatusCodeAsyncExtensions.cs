// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ActiveErrors.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ActiveErrors
{
	public static class StatusCodeAsyncExtensions
	{
		public static Task<IActionResult> StatusCodeAsync(this ControllerBase controller, HttpStatusCode code)
		{
			return Task.FromResult((IActionResult) controller.StatusCode((int) code));
		}

		public static Task<IActionResult> StatusCodeAsync(this ControllerBase controller, HttpStatusCode code,
			object value)
		{
			return Task.FromResult((IActionResult) controller.StatusCode((int) code, value));
		}

		public static Task<IActionResult> NotModifiedAsync(this ControllerBase controller)
		{
			return controller.StatusCodeAsync(HttpStatusCode.NotModified);
		}

		public static Task<IActionResult> NotModifiedAsync(this ControllerBase controller, object value)
		{
			return controller.StatusCodeAsync(HttpStatusCode.NotModified, value);
		}

		public static Task<IActionResult> NotImplementedAsync(this ControllerBase controller)
		{
			return controller.StatusCodeAsync(HttpStatusCode.NotImplemented);
		}

		public static Task<IActionResult> NotImplementedAsync(this ControllerBase controller, object value)
		{
			return controller.StatusCodeAsync(HttpStatusCode.NotImplemented, value);
		}

		public static Task<IActionResult> InternalServerErrorAsync(this ControllerBase controller)
		{
			return controller.StatusCodeAsync(HttpStatusCode.InternalServerError);
		}

		public static Task<IActionResult> InternalServerErrorAsync(this ControllerBase controller, object value)
		{
			return controller.StatusCodeAsync(HttpStatusCode.InternalServerError, value);
		}

		public static Task<IActionResult> GoneAsync(this ControllerBase controller)
		{
			return controller.StatusCodeAsync(HttpStatusCode.Gone);
		}

		public static Task<IActionResult> GoneAsync(this ControllerBase controller, object value)
		{
			return controller.StatusCodeAsync(HttpStatusCode.Gone, value);
		}

		public static Task<IActionResult> UnsupportedMediaTypeAsync(this ControllerBase controller)
		{
			return controller.StatusCodeAsync(HttpStatusCode.UnsupportedMediaType);
		}

		public static Task<IActionResult> UnsupportedMediaTypeAsync(this ControllerBase controller, object value)
		{
			return controller.StatusCodeAsync(HttpStatusCode.UnsupportedMediaType, value);
		}

		public static Task<IActionResult> NotAcceptableAsync(this ControllerBase controller)
		{
			return controller.StatusCodeAsync(HttpStatusCode.NotAcceptable);
		}

		public static Task<IActionResult> NotAcceptableAsync(this ControllerBase controller, object value)
		{
			return controller.StatusCodeAsync(HttpStatusCode.NotAcceptable, value);
		}

		public static Task<IActionResult> SeeOtherAsync(this ControllerBase controller, string location)
		{
			return controller.SetHeader(HttpHeaders.Location, location).StatusCodeAsync(HttpStatusCode.SeeOther);
		}

		public static Task<IActionResult> SeeOtherAsync(this ControllerBase controller, string location, object value)
		{
			return controller.SetHeader(HttpHeaders.Location, location).StatusCodeAsync(HttpStatusCode.SeeOther, value);
		}

		public static Task<IActionResult> CreatedAsync(this ControllerBase controller, string location)
		{
			return controller.SetHeader(HttpHeaders.Location, location).StatusCodeAsync(HttpStatusCode.Created);
		}

		public static Task<IActionResult> CreatedAsync(this ControllerBase controller, string location, object value)
		{
			return controller.SetHeader(HttpHeaders.Location, location).StatusCodeAsync(HttpStatusCode.Created, value);
		}

		public static Task<IActionResult> NotFoundAsync(this ControllerBase controller)
		{
			return Task.FromResult((IActionResult) controller.NotFound());
		}

		public static Task<IActionResult> NotFoundAsync(this ControllerBase controller, object value)
		{
			return Task.FromResult((IActionResult) controller.NotFound(value));
		}

		public static Task<IActionResult> UnauthorizedAsync(this ControllerBase controller)
		{
			return Task.FromResult((IActionResult) controller.Unauthorized());
		}

		public static Task<IActionResult> UnauthorizedAsync(this ControllerBase controller, object value)
		{
			return Task.FromResult((IActionResult) controller.Unauthorized(value));
		}

		public static Task<IActionResult> UnprocessableEntityAsync(this ControllerBase controller)
		{
			return Task.FromResult((IActionResult) controller.UnprocessableEntity());
		}

		public static Task<IActionResult> UnprocessableEntityAsync(this ControllerBase controller, object value)
		{
			return Task.FromResult((IActionResult) controller.UnprocessableEntity(value));
		}

		public static Task<IActionResult> BadRequestAsync(this ControllerBase controller)
		{
			return Task.FromResult((IActionResult) controller.BadRequest());
		}

		public static Task<IActionResult> BadRequestAsync(this ControllerBase controller, object error)
		{
			return Task.FromResult((IActionResult) controller.BadRequest(error));
		}

		public static Task<IActionResult> BadRequestAsync(this ControllerBase controller,
			ModelStateDictionary modelState)
		{
			return Task.FromResult((IActionResult) controller.BadRequest(modelState));
		}

		public static Task<IActionResult> ForbiddenAsync(this ControllerBase controller,
			params string[] authenticationSchemes)
		{
			return Task.FromResult((IActionResult) controller.Forbid(authenticationSchemes));
		}

		public static Task<IActionResult> ForbiddenAsync(this ControllerBase controller,
			AuthenticationProperties properties, params string[] authenticationSchemes)
		{
			return Task.FromResult((IActionResult) controller.Forbid(properties, authenticationSchemes));
		}
	}
}