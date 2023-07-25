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

               RegisterEvents();
               base.OnEnabled();
          }

          public override void OnDisabled()
          {
               UnregisterEvents();
               Instance = null;
               base.OnDisabled();
          }
          public void RegisterEvents()
          {
               eventHandler = new Handler();
               Exiled.Events.Handlers.Server.RoundStarted += eventHandler.OnRoundStart;

               Log.Debug("Events have been registered!");
          }
          public void UnregisterEvents()
          {
               Exiled.Events.Handlers.Server.RoundStarted -= eventHandler.OnRoundStart;
               Timing.KillCoroutines();
               eventHandler = null;
          }
     }
}

// player.TryAddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);