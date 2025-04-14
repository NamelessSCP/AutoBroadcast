using AutoBroadcast.Objects;
using MEC;
using GameCore;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace AutoBroadcast;

public class Handler
{
	public static readonly List<CoroutineHandle> Coroutines = new();

	private static Config? Config => AutoBroadcast.Instance.Config;

	public Handler()
	{
		ServerEvents.RoundEnded += OnRoundEnded;
		ServerEvents.RoundStarted += OnRoundStart;
		PlayerEvents.Joined += OnJoined;
		PlayerEvents.Spawned += OnSpawned;
		PlayerEvents.Death += OnDied;

        if (Config?.NtfAnnouncementCassie != "DISABLED" && Config?.NtfAnnouncementCassieNoScps != "DISABLED")
			Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += OnAnnouncingNtf;
	}

	~Handler()
	{
		ServerEvents.RoundEnded -= OnRoundEnded;
		ServerEvents.RoundStarted -= OnRoundStart;
		PlayerEvents.Joined -= OnJoined;
		PlayerEvents.Spawned -= OnSpawned;
		PlayerEvents.Death -= OnDied;

        if (Config?.NtfAnnouncementCassie != "DISABLED" || Config?.NtfAnnouncementCassieNoScps != "DISABLED")
			Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= OnAnnouncingNtf;
	}
	
	public void OnRoundStart()
	{
		Logger.Debug("Round started");
		Config?.RoundStart?.Broadcast?.Show();
		Config?.RoundStart?.Cassie?.Send();

		foreach (KeyValuePair<int, BroadCassie> kvp in Config?.Delayed ?? new Dictionary<int, BroadCassie>())
		{
			Logger.Debug("Running coroutine");
			Coroutines.Add(Timing.CallDelayed(kvp.Key, delegate
			{
				kvp.Value.Broadcast?.Show();
				kvp.Value.Cassie?.Send();
			}));
		}

		foreach (KeyValuePair<int, BroadCassie> kvp in Config.Intervals)
		{
			Logger.Debug("Running coroutine");
			Coroutines.Add(Timing.RunCoroutine(DoIntervalBroadcast(kvp.Key, kvp.Value)));
		}
	}

	public void OnRoundEnded(RoundEndedEventArgs ev)
	{
		Logger.Debug("Killing coroutines");
		foreach(CoroutineHandle coroutine in Coroutines)
		{
			Timing.KillCoroutines(coroutine);
			Logger.Debug("Killed a coroutine successfully!");
		}
		
		Coroutines.Clear();
		Logger.Debug("Killed all coroutines");
	}

	public void OnJoined(PlayerJoinedEventArgs ev)
	{
		if (Config?.JoinMessage != null && Config.JoinMessage.Duration > 0 && !Config.JoinMessage.Message.IsEmpty())
		{
			Logger.Debug("Showing Verified message to " + ev.Player.Nickname);
			string message = Config.JoinMessage.Message.Replace("%name%", ev.Player.Nickname);
			Config.JoinMessage.Show(ev.Player);
		};
	}

	public void OnAnnouncingNtf(AnnouncingNtfEntranceEventArgs ev)
	{
		string cassieMessage = string.Empty;
		
		if (ev.ScpsLeft == 0 && Config.NtfAnnouncementCassieNoScps != "DISABLED")
		{
			Logger.Debug("No SCPs cassie");
			ev.IsAllowed = false;
			cassieMessage = Config.NtfAnnouncementCassieNoScps;
		}
		else if (ev.ScpsLeft >= 1 && Config.NtfAnnouncementCassie != "DISABLED")
		{
			Logger.Debug("1 or more SCPs cassie");
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

	public void OnSpawned(PlayerSpawnedEventArgs ev)
	{
		if (Config.SpawnBroadcasts.TryGetValue(ev.Player.Role, out BroadCassie broadcast))
		{
			Logger.Debug($"Spawn Broadcast showing for {ev.Player.Role}");
			broadcast.Broadcast?.Show(ev.Player);
			broadcast.Cassie?.Send();
		}
	}

	public void OnDied(PlayerDeathEventArgs ev)
	{
		if (ev.TargetOldRole.GetTeam() is not Team.SCPs and not Team.OtherAlive and not Team.Dead)
		{
            List<Player> playerTeam = Player.Get(ev.TargetOldRole.GetTeam()).ToList();
            if (playerTeam.Count == 1)
            {
				Logger.Debug($"{ev.TargetOldRole.GetTeam()} has 1 person remaining on their team, starting LastPlayerAlive Broadcast/Cassie");
                Config?.LastPlayerOnTeam?.Broadcast?.Show(playerTeam.FirstOrDefault());
                Config?.LastPlayerOnTeam?.Cassie?.Send(ev.TargetOldRole);
            }
        }
    }

	public IEnumerator<float> DoIntervalBroadcast(int interval, BroadCassie message)
	{
		while (Round.IsRoundStarted)
		{
			yield return Timing.WaitForSeconds(interval);

			message.Broadcast?.Show();
			message.Cassie?.Send();
		}
	}
}