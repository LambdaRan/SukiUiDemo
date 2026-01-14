namespace SukiUiDemo.AppInfo;

public class AppEnviroment : IAppEnviroment
{
	public string ApplicationName { get; set; } = string.Empty;
	public string ContentRootPath { get; set; } = string.Empty;
}
