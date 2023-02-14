using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation;

namespace L4D2PlayStats.FunctionApp.Errors;

public class ErrorResult
{
	private ErrorResult(ValidationException validationException)
	{
		StatusCode = HttpStatusCode.BadRequest;
		Message = validationException.Message;
		Errors = validationException.Errors?.Select(failure => new Error(failure)).ToList();
	}

	private ErrorResult(UnauthorizedAccessException unauthorizedAccessException)
	{
		StatusCode = HttpStatusCode.Unauthorized;
		Message = unauthorizedAccessException.Message;
	}

	private ErrorResult(Exception exception)
	{
		StatusCode = HttpStatusCode.InternalServerError;
		Message = exception.Message;
	}

	public HttpStatusCode StatusCode { get; }
	public string Message { get; }
	public List<Error> Errors { get; } = new();

	public static ErrorResult Build(Exception exception)
	{
		if (exception is AggregateException)
			exception = exception.InnerException;

		return exception switch
		{
			ValidationException validationException => new ErrorResult(validationException),
			UnauthorizedAccessException unauthorizedAccessException => new ErrorResult(unauthorizedAccessException),
			_ => new ErrorResult(exception)
		};
	}
}