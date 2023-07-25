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
                Timing.RunCoroutine((IEnumerator<float>)Time(autoBroadcast.Key, autoBroadcast.Value));
            }
        }
        IEnumerable<float> Time(int key, BroadcastSystem broadcast)
        {
            while(Round.IsStarted)
            {
                if((int)Round.ElapsedTime.TotalSeconds == key)
                {
                    Map.Broadcast(broadcast.Duration, broadcast.BroadcastMessage);
                    yield break;
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}