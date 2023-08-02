namespace AutoBroadcastSystem.Events
{
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Server;
    using Exiled.API.Enums;
    using PlayerRoles;
    using AutoBroadcastSystem;
    using Exiled.API.Features;
    using MEC;
    public sealed class Handler
    {
        Config config = AutoBroadcast.Instance.Config;
        public void OnRoundStart()
        {
            foreach(KeyValuePair<int, BroadcastSystem> autoBroadcast in config.Broadcasts)
            {
                Timing.CallDelayed(autoBroadcast.Key, () => { 
                    if(Round.IsStarted) Map.Broadcast(autoBroadcast.Value.Duration, autoBroadcast.Value.BroadcastMessage);
                });
            }
            foreach(KeyValuePair<int, BroadcastSystem> intervalBroadcast in config.Intervals)
            {
                Timing.CallDelayed(intervalBroadcast.Key, () => Timing.RunCoroutine(Interval(intervalBroadcast.Key, intervalBroadcast.Value)));
            };
        }
        public void OnWaiting()
        {
            Timing.KillCoroutines();
        }
        public void OnJoin(JoinedEventArgs ev)
        {
            if(config.JoinMessage.BroadcastMessage.Length > 0)
            {
                string message = config.JoinMessage.BroadcastMessage.Replace("%name%", ev.Player.Nickname);
                ev.Player.Broadcast(config.JoinMessage.Duration, message);
            };
        }
        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
            {
                Map.Broadcast(config.ChaosAnnouncement.Duration, config.ChaosAnnouncement.BroadcastMessage);
                Cassie.Message(config.ChaosAnnouncement.CassieMessage, default, default, config.ChaosAnnouncement.ShowSubtitles);
            }
        }
        IEnumerator<float> Interval(int interval, BroadcastSystem broadcast)
        {
            while(Round.IsStarted)
            {
                Map.Broadcast(broadcast.Duration, broadcast.BroadcastMessage);
                yield return Timing.WaitForSeconds(interval);
            }
        }
    }
}