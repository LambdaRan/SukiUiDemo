namespace SukiUiDemo.AppInfo;

public interface IAppEnviroment
{
	// Gets or sets the name of the application. This property is automatically set by the host to the assembly containing
	// the application entry point.
	string ApplicationName { get; set; }

	// Gets or sets the absolute path to the directory that contains the application content files.
	string ContentRootPath { get; set; }
}
