using Exiled.API.Enums;
using Exiled.API.Features;
using AutoBroadcastSystem.Events;

namespace AutoBroadcastSystem;

public class AutoBroadcast : Plugin<Config>
{ 
	public override string Name => "AutoBroadcast";
	public override string Prefix => "AutoBroadcast";
	public override string Author => "@misfiy";
	public override Version RequiredExiledVersion => new(8, 4, 3);
	public override Version Version => new(1, 5, 3);
	public override PluginPriority Priority { get; } = PluginPriority.Last;

	private Handler eventHandler { get; set; } = null!;
	public static AutoBroadcast Instance { get; set; } = null!;

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