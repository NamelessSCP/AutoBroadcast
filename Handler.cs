namespace AutoBroadcastSystem.Events
{
	using Exiled.Events.EventArgs.Player;
	using Exiled.Events.EventArgs.Server;
	using AutoBroadcastSystem;
	using static CoroutinesHandler;
	using Exiled.API.Features;
	using MEC;
	using Exiled.Events.EventArgs.Map;

	public sealed class Handler
	{
		private static readonly Config config = AutoBroadcast.Instance.Config;

		public void OnRoundStart()
		{
			config.RoundStart.Broadcast?.Show();
			config.RoundStart.Cassie?.Send();

			foreach (var kvp in config.Delayed)
			{
				Coroutines.Add(Timing.CallDelayed(kvp.Key, delegate
				{
					kvp.Value.Broadcast?.Show();
					kvp.Value.Cassie?.Send();
				}));
			}

			foreach (var kvp in config.Intervals)
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

		public void OnAnnouncingMtf(AnnouncingNtfEntranceEventArgs ev)
		{
			string cassieMessage = string.Empty;
			if (ev.ScpsLeft == 0 && AutoBroadcast.Instance.Config.NtfAnnouncementCassieNoScps != "DISABLED")
			{
				ev.IsAllowed = false;
				cassieMessage = AutoBroadcast.Instance.Config.NtfAnnouncementCassieNoScps;
			}
			else if (ev.ScpsLeft >= 1 && AutoBroadcast.Instance.Config.NtfAnnouncementCassie != "DISABLED")
			{
				ev.IsAllowed = false;
				cassieMessage = AutoBroadcast.Instance.Config.NtfAnnouncementCassie;
			}

			cassieMessage = cassieMessage.Replace("%scps%", $"{ev.ScpsLeft} scpsubject");

			if (ev.ScpsLeft > 1)
				cassieMessage = cassieMessage.Replace("scpsubject", "scpsubjects");

			cassieMessage = cassieMessage.Replace("%designation%", $"nato_{ev.UnitName[0]} {ev.UnitNumber}");

			if (!string.IsNullOrEmpty(cassieMessage))
				Cassie.Message(cassieMessage);
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