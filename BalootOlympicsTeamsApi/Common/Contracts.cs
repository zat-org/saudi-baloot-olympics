﻿namespace BalootOlympicsTeamsApi.Common;
public sealed record SuccessResponse(object Data, string Message);
public sealed record SuccessResponse<T>(T Data, string Message);

public sealed record ErrorResponse(
  ErrorType Code,
  string Message,
  string TraceId);

public sealed record ValidationErrorResponse(
    ErrorType Code,
    string Message,
    IDictionary<string, List<string>> Errors);