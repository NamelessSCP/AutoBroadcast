namespace AutoBroadcastSystem.Events
{
	using Exiled.Events.EventArgs.Player;
	using Exiled.Events.EventArgs.Server;
	using AutoBroadcastSystem;
	using static AutoBroadcastSystem.CoroutinesHandler;
	using Exiled.API.Features;
	using MEC;

	public sealed class Handler
	{
		private static readonly Config config = AutoBroadcast.Instance.Config;

		public void OnRoundStart()
		{
			config.RoundStart.Broadcast?.Show();
			config.RoundStart.Cassie?.Send();

			foreach (var kvp in config.Broadcasts)
			{
				Coroutines.Add(Timing.CallDelayed(kvp.Key, delegate
				{
					kvp.Value.Broadcast?.Show();
					kvp.Value.Cassie?.Send();
				}));
			}

			foreach (var kvp in config.BroadcastIntervals)
			{
				Coroutines.Add(Timing.RunCoroutine(DoIntervalBroadcast(kvp.Key, kvp.Value)));
			}
		}

		public void OnEnd(RoundEndedEventArgs ev) => KillCoroutines();

		public void OnVerified(VerifiedEventArgs ev)
		{
			if (config.JoinMessage.Duration > 0 && !config.JoinMessage.Message.IsEmpty())
			{
				string message = config.JoinMessage.Message.Replace("%name%", ev.Player.Nickname);
				ev.Player.Broadcast(config.JoinMessage.Duration, message, default, config.JoinMessage.Override);
			};
		}

		public void OnRespawningTeam(RespawningTeamEventArgs ev)
		{
			if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
			{
				config.ChaosAnnouncement.Cassie?.Send();
				config.ChaosAnnouncement.Broadcast?.Show();
			}
		}

		public void OnSpawned(SpawnedEventArgs ev)
		{
			if (config.SpawnBroadcasts.TryGetValue(ev.Player.Role.Type, out BroadCassie broadcast))
			{
				broadcast.Broadcast?.Show(ev.Player);
				broadcast.Cassie?.Send();
			}
		}

		public IEnumerator<float> DoIntervalBroadcast(int interval, BroadCassie message)
		{
			while (Round.IsStarted)
			{
				yield return Timing.WaitForSeconds(interval);

				message.Broadcast?.Show();
				message.Cassie?.Send();
			}
		}
	}
}