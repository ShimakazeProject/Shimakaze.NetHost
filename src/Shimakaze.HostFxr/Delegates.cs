namespace Shimakaze;

public delegate void ErrorWriter(string? message);

public delegate void GetDotnetEnvironmentInfoResult(DotNetEnvironmentInfo info, object? resultContext);
