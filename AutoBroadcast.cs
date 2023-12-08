namespace AutoBroadcastSystem
{
	using Exiled.API.Features;
	using AutoBroadcastSystem.Events;

	public class AutoBroadcast : Plugin<Config>
	{
		public override string Name => "AutoBroadcast";
		public override string Prefix => "AutoBroadcast";
		public override string Author => "@misfiy";
		public override Version RequiredExiledVersion => new(8, 2, 1);
		public override Version Version => new(1, 5, 1);
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
			Log.Debug("Disabling plugin..");
			UnregisterEvents();
			CoroutinesHandler.KillCoroutines();
			Instance = null!;
			base.OnDisabled();
		}
		public void RegisterEvents()
		{
			eventHandler = new Handler();
			Exiled.Events.Handlers.Server.RoundEnded += eventHandler.OnEnd;
			Exiled.Events.Handlers.Server.RoundStarted += eventHandler.OnRoundStart;
			Exiled.Events.Handlers.Server.RespawningTeam += eventHandler.OnRespawningTeam;
			Exiled.Events.Handlers.Player.Verified += eventHandler.OnVerified;
			Exiled.Events.Handlers.Player.Spawned += eventHandler.OnSpawned;

			if(Instance.Config.NtfAnnouncementCassie != "DISABLED" && Instance.Config.NtfAnnouncementCassieNoScps != "DISABLED")
				Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += eventHandler.OnAnnouncingMtf;

			Log.Debug("Events have been registered!");
		}
		public void UnregisterEvents()
		{
			Exiled.Events.Handlers.Server.RoundEnded -= eventHandler.OnEnd;
			Exiled.Events.Handlers.Server.RoundStarted -= eventHandler.OnRoundStart;
			Exiled.Events.Handlers.Server.RespawningTeam -= eventHandler.OnRespawningTeam;
			Exiled.Events.Handlers.Player.Verified -= eventHandler.OnVerified;
			Exiled.Events.Handlers.Player.Spawned -= eventHandler.OnSpawned;

			if(Instance.Config.NtfAnnouncementCassie != "DISABLED" && Instance.Config.NtfAnnouncementCassieNoScps != "DISABLED")
				Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= eventHandler.OnAnnouncingMtf;

			eventHandler = null!;
		}
	}
}