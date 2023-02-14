namespace L4D2PlayStats.Core.Modules.Campaigns;

public class Campaign
{
	public string? Name { get; set; }
	public List<string> Maps { get; set; } = new();
}