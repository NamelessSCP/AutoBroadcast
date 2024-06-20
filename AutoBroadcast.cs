using Exiled.API.Enums;
using Exiled.API.Features;
using AutoBroadcastSystem.Events;

namespace AutoBroadcastSystem;

public class AutoBroadcast : Plugin<Config>
{ 
	public static AutoBroadcast Instance { get; set; } = null!;

	public override string Name => "AutoBroadcast";
	public override string Prefix => "AutoBroadcast";
	public override string Author => "@misfiy";
	public override Version RequiredExiledVersion => new(8, 8, 0);
	public override Version Version => new(1, 6, 2);
	public override PluginPriority Priority { get; } = PluginPriority.Last;

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