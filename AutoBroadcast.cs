using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;

namespace AutoBroadcast;

public class AutoBroadcast : Plugin<Config>
{ 
	private Handler? _eventHandler;

	public static AutoBroadcast Instance { get; private set; } = null!;

	public override string Name => "AutoBroadcast";
	public override string Description => "Auto-Broadcast plugin";

	public override string Author => "@misfiy";
	public override Version Version => new(1, 6, 3);
	public override Version RequiredApiVersion { get; } = new(LabApi.Features.LabApiProperties.CompiledVersion);
	public override LoadPriority Priority => LoadPriority.Lowest;

	public override void Enable()
	{
		Instance = this;
		_eventHandler = new Handler();
	}

	public override void Disable()
	{
		_eventHandler = null!;
		Instance = null!;
	}
}