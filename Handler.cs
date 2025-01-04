using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.API.Features;
using MEC;
using Exiled.Events.EventArgs.Map;
using PlayerRoles;

namespace AutoBroadcast;

public class Handler
{
	public static List<CoroutineHandle> Coroutines = new();
		
	private static Config config => AutoBroadcast.Instance.Config;

	public Handler()
	{
		Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
		Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
		Exiled.Events.Handlers.Player.Verified += OnVerified;
		Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        Exiled.Events.Handlers.Player.Died += OnDied;

        if (AutoBroadcast.Instance.Config.NtfAnnouncementCassie != "DISABLED" && AutoBroadcast.Instance.Config.NtfAnnouncementCassieNoScps != "DISABLED")
			Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += OnAnnouncingNtf;
	}

	~Handler()
	{
		Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
		Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
		Exiled.Events.Handlers.Player.Verified -= OnVerified;
		Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
		Exiled.Events.Handlers.Player.Died -= OnDied;

        if (AutoBroadcast.Instance.Config.NtfAnnouncementCassie != "DISABLED" || AutoBroadcast.Instance.Config.NtfAnnouncementCassieNoScps != "DISABLED")
			Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= OnAnnouncingNtf;
	}
	
	public void OnRoundStart()
	{
		Log.Debug("Round started");
		config.RoundStart?.Broadcast?.Show();
		config.RoundStart?.Cassie?.Send();

		foreach (KeyValuePair<int, BroadCassie> kvp in config.Delayed)
		{
			Log.Debug("Running coroutine");
			Coroutines.Add(Timing.CallDelayed(kvp.Key, delegate
			{
				kvp.Value.Broadcast?.Show();
				kvp.Value.Cassie?.Send();
			}));
		}

		foreach (KeyValuePair<int, BroadCassie> kvp in config.Intervals)
		{
			Log.Debug("Running coroutine");
			Coroutines.Add(Timing.RunCoroutine(DoIntervalBroadcast(kvp.Key, kvp.Value)));
		}
	}

	public void OnRoundEnded(RoundEndedEventArgs ev)
	{
		Log.Debug("Killing coroutines");
		foreach(CoroutineHandle coroutine in Coroutines)
		{
			Timing.KillCoroutines(coroutine);
			Log.Debug("Killed a coroutine successfully!");
		}
		
		Coroutines.Clear();
		Log.Debug("Killed all coroutines");
	}

	public void OnVerified(VerifiedEventArgs ev)
	{
		if (config.JoinMessage != null && config.JoinMessage.Duration > 0 && !config.JoinMessage.Message.IsEmpty())
		{
			Log.Debug("Showing Verified message to " + ev.Player.Nickname);
			string message = config.JoinMessage.Message.Replace("%name%", ev.Player.Nickname);
			ev.Player.Broadcast(config.JoinMessage.Duration, message, default, config.JoinMessage.Override);
		};
	}

	public void OnAnnouncingNtf(AnnouncingNtfEntranceEventArgs ev)
	{
		string cassieMessage = string.Empty;
		
		if (ev.ScpsLeft == 0 && AutoBroadcast.Instance.Config.NtfAnnouncementCassieNoScps != "DISABLED")
		{
			Log.Debug("No SCPs cassie");
			ev.IsAllowed = false;
			cassieMessage = AutoBroadcast.Instance.Config.NtfAnnouncementCassieNoScps;
		}
		else if (ev.ScpsLeft >= 1 && AutoBroadcast.Instance.Config.NtfAnnouncementCassie != "DISABLED")
		{
			Log.Debug("1 or more SCPs cassie");
			ev.IsAllowed = false;
			cassieMessage = AutoBroadcast.Instance.Config.NtfAnnouncementCassie;
		}

		cassieMessage = cassieMessage.Replace("%scps%", $"{ev.ScpsLeft} scpsubject");

		if (ev.ScpsLeft > 1)
			cassieMessage = cassieMessage.Replace("scpsubject", "scpsubjects");

		cassieMessage = cassieMessage.Replace("%designation%", $"nato_{ev.UnitName[0]} {ev.UnitNumber}");

		if (!string.IsNullOrEmpty(cassieMessage))
			Cassie.Message(cassieMessage, isSubtitles: true);
	}

	public void OnSpawned(SpawnedEventArgs ev)
	{
		if (config.SpawnBroadcasts.TryGetValue(ev.Player.Role.Type, out BroadCassie broadcast))
		{
			Log.Debug($"Spawn Broadcast showing for {ev.Player.Role.Type}");
			broadcast.Broadcast?.Show(ev.Player);
			broadcast.Cassie?.Send();
		}
	}

	public void OnDied(DiedEventArgs ev)
	{
		if (ev.TargetOldRole.GetTeam() is not Team.SCPs and not Team.OtherAlive and not Team.Dead)
		{
            List<Player> playerTeam = Player.Get(ev.TargetOldRole.GetTeam()).ToList();
            if (playerTeam.Count == 1)
            {
				Log.Debug($"{ev.TargetOldRole.GetTeam()} has 1 person remaining on their team, starting LastPlayerAlive Broadcast/Cassie");
                config.LastPlayerOnTeam?.Broadcast?.Show(playerTeam.FirstOrDefault());
                config.LastPlayerOnTeam?.Cassie?.Send(ev.TargetOldRole);
            }
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