using Exiled.API.Interfaces;
using PlayerRoles;

namespace AutoBroadcastSystem
{
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		public bool Debug { get; set; } = false;

		public BroadcastSystem? JoinMessage { get; set; } = new()
		{
			Duration = 4,
			Message = "Welcome, %name%!",
			Override = false
		};

		public BroadCassie? ChaosAnnouncement { get; set; } = new()
		{
			Cassie = new()
			{
				Message = "Warning . Military Personnel has entered the facility . Designated as, Chaos Insurgency.",
				Translation = "Warning. Military Personnel has entered the facility. Designated as, <color=green>Chaos Insurgency</color>.",
				ShowSubtitles = false,
			},
		};

		public string NtfAnnouncementCassieNoScps { get; set; } = "DISABLED";
		public string NtfAnnouncementCassie { get; set; } = "DISABLED";

		public BroadCassie? RoundStart { get; set; } = new()
		{
			Broadcast = new()
			{
				Message = "Round has started!",
			},
			Cassie = new()
			{
				Message = "Containment breach detected",
			}
		};

		public Dictionary<int, BroadCassie> Delayed { get; set; } = new()
		{
		  {
			 60, new()
			 {
				Broadcast = new()
				{
				    Message = "60 seconds have passed!",
				}
			 }
		  }
		};
		public Dictionary<int, BroadCassie> Intervals { get; set; } = new()
		{
		  {
			 30, new()
			 {
				Broadcast = new()
				{
				    Message = "I show every 30 seconds!",
				    UseHints = true,
				}
			 }
		  },
	   };
		public Dictionary<RoleTypeId, BroadCassie> SpawnBroadcasts { get; set; } = new()
	   {
		  {
			 RoleTypeId.Spectator, new()
			 {
				Broadcast = new()
				{
				    Message = "You died!",
				    Duration = 5,
				    UseHints = true,
				}
			 }
		  },
		  {
			 RoleTypeId.Overwatch, new()
			 {
				Broadcast = new()
				{
				    Message = "Overwatch initiated.",
				    Duration = 6,
				}
			 }
		  }
	   };
	}
}