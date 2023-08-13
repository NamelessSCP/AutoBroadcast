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
          readonly Config config = AutoBroadcast.Instance.Config;
          public void OnRoundStart()
          {
               if (config.Broadcasts != null)
               {
                    foreach (KeyValuePair<int, BroadcastSystem> autoBroadcast in config.Broadcasts)
                    {
                         Timing.CallDelayed(autoBroadcast.Key, () =>
                         {
                              if (Round.IsStarted) Map.Broadcast(autoBroadcast.Value.Duration, autoBroadcast.Value.BroadcastMessage);
                         });
                    }
               }
               if (config.Intervals != null)
               {
                    foreach (KeyValuePair<int, BroadcastSystem> intervalBroadcast in config.Intervals)
                    {
                         Timing.CallDelayed(intervalBroadcast.Key, () =>
                         {
                              CoroutineHandle coroutine = Timing.RunCoroutine(Interval(intervalBroadcast.Key, intervalBroadcast.Value));
                              IntervalCoroutines.Add(coroutine);
                         });
                    };
               }
          }
          public void OnWaiting()
          {
               if(IntervalCoroutines == null) return;
               foreach(CoroutineHandle coroutine in IntervalCoroutines)
               {
                    IntervalCoroutines.Remove(coroutine);
                    if(coroutine.IsRunning) Timing.KillCoroutines(coroutine);
               }
          }
          public void OnVerified(VerifiedEventArgs ev)
          {
               if (config.JoinMessage.Duration > 0)
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
               while (Round.IsStarted)
               {
                    Map.Broadcast(broadcast.Duration, broadcast.BroadcastMessage);
                    yield return Timing.WaitForSeconds(interval);
               }
          }
     }
}