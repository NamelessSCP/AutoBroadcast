namespace AutoBroadcastSystem
{
     using Exiled.API.Features;
     using Exiled.API.Enums;
     using AutoBroadcastSystem.Events;
     using MEC;

     public class AutoBroadcast : Plugin<Config>
     {
          public override string Name => "AutoBroadcast";
          public override string Prefix => "AutoBroadcast";
          public override string Author => "@misfiy";
          public override PluginPriority Priority => PluginPriority.Default;
          private Handler eventHandler;
          public static AutoBroadcast Instance;
          private Config config;
          public override void OnEnabled()
          {
               Instance = this;
               config = Instance.Config;
               CoroutinesHandler.IntervalCoroutines = new();

               RegisterEvents();
               base.OnEnabled();
          }

          public override void OnDisabled()
          {
               UnregisterEvents();
               CoroutinesHandler.IntervalCoroutines = null;
               Instance = null!;
               base.OnDisabled();
          }
          public void RegisterEvents()
          {
               eventHandler = new Handler();
               Exiled.Events.Handlers.Server.WaitingForPlayers += eventHandler.OnWaiting;
               Exiled.Events.Handlers.Server.RoundStarted += eventHandler.OnRoundStart;
               Exiled.Events.Handlers.Server.RespawningTeam += eventHandler.OnRespawningTeam;
               Exiled.Events.Handlers.Player.Verified += eventHandler.OnVerified;
               Exiled.Events.Handlers.Player.Spawned += eventHandler.OnSpawned;

               Log.Debug("Events have been registered!");
          }
          public void UnregisterEvents()
          {
               Exiled.Events.Handlers.Server.WaitingForPlayers -= eventHandler.OnWaiting;
               Exiled.Events.Handlers.Server.RoundStarted -= eventHandler.OnRoundStart;
               Exiled.Events.Handlers.Server.RespawningTeam -= eventHandler.OnRespawningTeam;
               Exiled.Events.Handlers.Player.Verified -= eventHandler.OnVerified;
               Exiled.Events.Handlers.Player.Spawned -= eventHandler.OnSpawned;

               Timing.KillCoroutines();
               eventHandler = null!;
          }
     }
}