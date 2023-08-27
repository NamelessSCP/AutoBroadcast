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
               if(!config.CassieRoundStart.Message.IsEmpty()) Cassie.MessageTranslated(config.CassieRoundStart.Message, config.CassieRoundStart.Translation, default, default, config.CassieRoundStart.ShowSubtitles);
               if (config.Broadcasts != null)
               {
                    foreach (KeyValuePair<int, BroadcastSystem> autoBroadcast in config.Broadcasts)
                    {
                         Timing.CallDelayed(autoBroadcast.Key, () =>
                         {
                              if (Round.IsStarted) Map.Broadcast(autoBroadcast.Value.Duration, autoBroadcast.Value.Message, default, autoBroadcast.Value.OverrideBroadcats);
                         });
                    }
               }
               if (config.BroadcastIntervals != null)
               {
                    foreach (KeyValuePair<int, BroadcastSystem> intervalBroadcast in config.BroadcastIntervals)
                    {
                         Timing.CallDelayed(intervalBroadcast.Key, () =>
                         {
                              CoroutineHandle coroutine = Timing.RunCoroutine(Interval(intervalBroadcast.Key, intervalBroadcast.Value));
                              IntervalCoroutines?.Add(coroutine);
                         });
                    };
               }
          }
          public void OnWaiting()
          {
               if(IntervalCoroutines == null) return;
               foreach(CoroutineHandle coroutine in IntervalCoroutines)
               {
                    if(coroutine.IsRunning) Timing.KillCoroutines(coroutine);
                    IntervalCoroutines.Remove(coroutine);
               }
          }
          public void OnVerified(VerifiedEventArgs ev)
          {
               if (config.JoinMessage.Duration > 0 && !config.JoinMessage.Message.IsEmpty())
               {
                    string message = config.JoinMessage.Message.Replace("%name%", ev.Player.Nickname);
                    ev.Player.Broadcast(config.JoinMessage.Duration, message, default, config.JoinMessage.OverrideBroadcats);
               };
          }
          public void OnRespawningTeam(RespawningTeamEventArgs ev)
          {
               if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
               {
                    if(!config.ChaosAnnouncement.Message.IsEmpty()) Cassie.MessageTranslated(config.ChaosAnnouncement.Message, config.ChaosAnnouncement.Translation, default, default, config.ChaosAnnouncement.ShowSubtitles);
               }
          }
          IEnumerator<float> Interval(int interval, BroadcastSystem broadcast)
          {
               while (Round.IsStarted)
               {
                    Map.Broadcast(broadcast.Duration, broadcast.Message, default, broadcast.OverrideBroadcats);
                    yield return Timing.WaitForSeconds(interval);
               }
          }
     }
}