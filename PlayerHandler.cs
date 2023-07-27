namespace AutoBroadcastSystem.Events
{
    using Exiled.Events.EventArgs.Player;
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
            string message = config.JoinMessage.BroadcastMessage.Replace("%name%", ev.Player.Nickname);
            if(message.Length > 0)
            {
                ev.Player.Broadcast(config.JoinMessage.Duration, message);
            };
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