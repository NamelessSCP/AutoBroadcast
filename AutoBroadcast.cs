using Exiled.API.Enums;

namespace AutoBroadcastSystem
{
	using Exiled.API.Features;
	using AutoBroadcastSystem.Events;

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

			RegisterEvents();

			base.OnEnabled();
		}

		public override void OnDisabled()
		{
			UnregisterEvents();

			Instance = null!;

			base.OnDisabled();
		}

		public void RegisterEvents()
		{
			eventHandler = new Handler();
			Exiled.Events.Handlers.Server.RoundEnded += eventHandler.OnRoundEnded;
			Exiled.Events.Handlers.Server.RoundStarted += eventHandler.OnRoundStart;
			Exiled.Events.Handlers.Server.RespawningTeam += eventHandler.OnRespawningTeam;
			Exiled.Events.Handlers.Player.Verified += eventHandler.OnVerified;
			Exiled.Events.Handlers.Player.Spawned += eventHandler.OnSpawned;

			if(Instance.Config.NtfAnnouncementCassie != "DISABLED" && Instance.Config.NtfAnnouncementCassieNoScps != "DISABLED")
				Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += eventHandler.OnAnnouncingNtf;

			Log.Debug("Events have been registered!");
		}

		public void UnregisterEvents()
		{
			Exiled.Events.Handlers.Server.RoundEnded -= eventHandler.OnRoundEnded;
			Exiled.Events.Handlers.Server.RoundStarted -= eventHandler.OnRoundStart;
			Exiled.Events.Handlers.Server.RespawningTeam -= eventHandler.OnRespawningTeam;
			Exiled.Events.Handlers.Player.Verified -= eventHandler.OnVerified;
			Exiled.Events.Handlers.Player.Spawned -= eventHandler.OnSpawned;

			if(Instance.Config.NtfAnnouncementCassie != "DISABLED" && Instance.Config.NtfAnnouncementCassieNoScps != "DISABLED")
				Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= eventHandler.OnAnnouncingNtf;

			eventHandler = null!;
		}
	}
}