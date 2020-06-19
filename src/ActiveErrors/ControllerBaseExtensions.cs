// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ActiveErrors
{
	public static class ControllerBaseExtensions
	{
		public static bool TryValidateModelOrError(this ControllerBase controller, object instance, long eventId,
			string errorMessage, out ErrorObjectResult error,
			HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity, params object[] args)
		{
			if (!controller.TryValidateModel(instance))
			{
				var validationError = controller.ConvertModelStateToError(eventId, errorMessage, statusCode);
				error = new ErrorObjectResult(validationError, args);
				return false;
			}

			error = null;
			return true;
		}

		public static Error ConvertModelStateToError(this ControllerBase controller, long eventId, string errorMessage,
			HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
		{
			var errors = new HashSet<Error>();

			foreach (var modelState in controller.ModelState.Values)
			foreach (var error in modelState.Errors)
			{
				errors.Add(new Error(eventId, !string.IsNullOrWhiteSpace(error.ErrorMessage)
					? error.ErrorMessage
					: error.Exception.Message, statusCode));
			}

			var validationError = new Error(eventId, errorMessage, statusCode, errors);
			return validationError;
		}
	}
}