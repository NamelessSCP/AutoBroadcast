using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.ComponentModel;

namespace AutoBroadcastSystem
{
     public sealed class Config : IConfig
     {
          public bool IsEnabled { get; set; } = true;
          public bool Debug { get; set; } = false;

          public BroadcastSystem JoinMessage { get; set; } = new()
          {
               Duration = 4,
               Message = "Welcome, %name%!",
               OverrideBroadcasts = false
          };

          public CassieBroadcastSystem ChaosAnnouncement { get; set; } = new()
          {
               Message = "Warning . Military Personnel has entered the facility . Designated as, Chaos Insurgency.",
               Translation = "Warning. Military Personnel has entered the facility. Designated as, Chaos Insurgency.",
               ShowSubtitles = false,
          };

          public CassieBroadcastSystem CassieRoundStart { get; set; } = new()
          {
               Message = "Containment breach",
               Translation = "Containment breach!",
               ShowSubtitles = false,
          };

          public Dictionary<int, BroadcastSystem> Broadcasts { get; set; } = new Dictionary<int, BroadcastSystem>
          {
               {
                    10, new BroadcastSystem
                    {
                         Duration = 5,
                         Message = "10 seconds have passed!"
                    }
               },
               {
                    60, new BroadcastSystem
                    {
                         Duration = 6,
                         Message = "60 seconds have passed!"
                    }
               }
          };
          public Dictionary<int, BroadcastSystem> BroadcastIntervals { get; set; } = new Dictionary<int, BroadcastSystem>
          {
               {
                    15, new BroadcastSystem
                    {
                         Duration = 3,
                         Message = "Every 15 seconds!"
                    }
               },
               {
                    120, new BroadcastSystem
                    {
                         Duration = 4,
                         Message = "Every 120 seconds!"
                    }
               }
          };
     }
     public class BroadcastSystem
     {
          [Description("How long the hint/broadcast should show")]
          public ushort Duration { get; set; }
          [Description("The message shown on the broadcast")]
          public string Message { get; set; }
          [Description("Override broadcasts")]
          public bool OverrideBroadcasts { get; set; } = false;
     }
     public class CassieBroadcastSystem
     {
          [Description("The CASSIE message to be sent")]
          public string Message { get; set; }
          [Description("The text to be shown")]
          public string Translation { get; set; }
          [Description("Whether or not to hide the subtitle for the cassie message")]
          public bool ShowSubtitles { get; set; }
     }
}