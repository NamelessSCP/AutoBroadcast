using Exiled.API.Interfaces;
using PlayerRoles;

namespace AutoBroadcast;

public class Config : IConfig
{
	public bool IsEnabled { get; set; } = true;
	public bool Debug { get; set; } = false;

	public BroadcastSystem? JoinMessage { get; set; } = new()
	{
		Duration = 4,
		Message = "Welcome, %name%!",
		Override = false
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
	
	public BroadCassie? LastPlayerOnTeam { get; set; } = new()
	{
		Broadcast = new()
		{
			Message = "You are the last remaining on your team!",
		},
		Cassie = new()
		{
			Message = "There is 1 {role} remaining!"
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