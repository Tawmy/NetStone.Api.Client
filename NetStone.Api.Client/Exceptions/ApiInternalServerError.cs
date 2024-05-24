namespace NetStone.Api.Client.Exceptions;

public class ApiInternalServerError(string exception) : Exception(exception);