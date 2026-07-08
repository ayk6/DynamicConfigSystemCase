namespace DynamicConfig.Core.Interfaces
{
	public interface IConfigurationReader
	{
		object GetValue(string key);
	}
}
