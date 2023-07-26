using Exiled.API.Interfaces;
using Exiled.API.Enums;
using PlayerRoles;
using System.ComponentModel;

namespace AutoBroadcastSystem
{
     public sealed class Config : IConfig
     {
          public bool IsEnabled { get; set; } = true;
          public bool Debug { get; set; } = false;
          public Dictionary<int, BroadcastSystem> Broadcasts { get; set; } = new Dictionary<int, BroadcastSystem>
          {
               {
                    10, new BroadcastSystem
                    {
                         Duration = 5,
                         BroadcastMessage = "10 seconds have passed!"
                    }
               },
               {
                    60, new BroadcastSystem
                    {
                         Duration = 6,
                         BroadcastMessage = "60 seconds have passed!"
                    }
               }
          };
          public Dictionary<int, BroadcastSystem> Intervals { get; set; } = new Dictionary<int, BroadcastSystem>
          {
               {
                    15, new BroadcastSystem
                    {
                         Duration = 3,
                         BroadcastMessage = "Every 15 seconds!"
                    }
               },
               {
                    120, new BroadcastSystem
                    {
                         Duration = 4,
                         BroadcastMessage = "Every 120 seconds!"
                    }
               }
          };
     }
     public class BroadcastSystem
     {
          [Description("How long the hint/broadcast should show")]
          public ushort Duration { get; set; }
          [Description("The message shown on the broadcast")]
          public string BroadcastMessage { get; set; }
     }
}