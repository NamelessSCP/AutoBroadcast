using Exiled.API.Enums;
using Exiled.API.Features;

namespace AutoBroadcast;

public class AutoBroadcast : Plugin<Config>
{ 
	public static AutoBroadcast Instance { get; set; } = null!;

	public override string Name => "AutoBroadcast";
	public override string Prefix => "AutoBroadcast";
	public override string Author => "@misfiy";
	public override Version RequiredExiledVersion => new(9, 2, 1);
	public override Version Version => new(1, 6, 3);
	public override PluginPriority Priority => PluginPriority.Last;

	private Handler eventHandler { get; set; } = null!;

	public override void OnEnabled()
	{
		Instance = this;
		eventHandler = new();

		base.OnEnabled();
	}

	public override void OnDisabled()
	{
		eventHandler = null!;
		Instance = null!;

		base.OnDisabled();
	}
}