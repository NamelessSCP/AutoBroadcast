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
          private readonly Config config = AutoBroadcast.Instance.Config;
          public void OnRoundStart()
          {
               if (!config.CassieRoundStart.Message.IsEmpty()) Cassie.MessageTranslated(config.CassieRoundStart.Message, config.CassieRoundStart.Translation, default, default, config.CassieRoundStart.ShowSubtitles);
               foreach (KeyValuePair<int, BroadcastSystem> autoBroadcast in config.Broadcasts)
               {
                    Timing.CallDelayed(autoBroadcast.Key, () =>
                    {
                         if (Round.IsStarted) Map.Broadcast(autoBroadcast.Value.Duration, autoBroadcast.Value.Message, default, autoBroadcast.Value.OverrideBroadcasts);
                    });
               }
               foreach (KeyValuePair<int, BroadcastSystem> intervalBroadcast in config.BroadcastIntervals)
               {
                    CoroutineHandle coroutine = Timing.RunCoroutine(IntervalBroadcast(intervalBroadcast.Key, intervalBroadcast.Value));
                    Coroutines?.Add(coroutine);
               };
          }
          public void OnWaiting() => KillCoroutines();
          public void OnVerified(VerifiedEventArgs ev)
          {
               if (config.JoinMessage.Duration > 0 && !config.JoinMessage.Message.IsEmpty())
               {
                    string message = config.JoinMessage.Message.Replace("%name%", ev.Player.Nickname);
                    ev.Player.Broadcast(config.JoinMessage.Duration, message, default, config.JoinMessage.OverrideBroadcasts);
               };
          }
          public void OnRespawningTeam(RespawningTeamEventArgs ev)
          {
               if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
               {
                    if (!config.ChaosAnnouncement.Message.IsEmpty()) Cassie.MessageTranslated(config.ChaosAnnouncement.Message, config.ChaosAnnouncement.Translation, default, default, config.ChaosAnnouncement.ShowSubtitles);
               }
          }

          public void OnSpawned(SpawnedEventArgs ev)
          {
               if (config.SpawnBroadcasts.ContainsKey(ev.Player.Role.Type))
               {
                    SpawningBroadcasts broadcast = config.SpawnBroadcasts[ev.Player.Role.Type];
                    if (broadcast.UseHints) ev.Player.ShowHint(broadcast.Message, broadcast.Duration);
                    else ev.Player.Broadcast(broadcast.Duration, broadcast.Message, default, broadcast.Override);
               }
          }

          public IEnumerator<float> IntervalBroadcast(int interval, BroadcastSystem broadcast)
          {
               while (Round.IsStarted)
               {
                    yield return Timing.WaitForSeconds(interval);
                    Map.Broadcast(broadcast.Duration, broadcast.Message, default, broadcast.OverrideBroadcasts);
               }
          }
     }
}