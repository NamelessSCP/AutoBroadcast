using Exiled.API.Features;
using PlayerRoles;
using System.ComponentModel;

namespace AutoBroadcastSystem;

public class BroadCassie
{
	public BroadcastSystem? Broadcast { get; set; }
	public CassieBroadcast? Cassie { get; set; }
}

public class BroadcastSystem
{
	[Description("How long the hint/broadcast should show")]
	public ushort Duration { get; set; } = 3;
	[Description("The message shown on the broadcast")]
	public string Message { get; set; } = "";
	[Description("Override broadcasts")]
	public bool Override { get; set; } = false;
	[Description("Whether or not to use hints instead")]
	public bool UseHints { get; set; } = false;

	public void Show()
	{
		if (UseHints)
			Map.ShowHint(Message, Duration);
		else
			Map.Broadcast(Duration, Message, default, Override);
	}
		
	public void Show(Player player)
	{
		if (UseHints)
			player.ShowHint(Message, Duration);
		else 
			player.Broadcast(Duration, Message, default, Override); 
	}

    // Overload for displaying player role (Last Player Alive)
    public void Show(RoleTypeId role)
	{
        if (UseHints)
            Map.ShowHint(Message.Replace("{role}", HelperMethods.CassieRoleTranslations(role)), Duration);
        else
            Map.Broadcast(Duration, Message.Replace("{role}", HelperMethods.CassieRoleTranslations(role)), default, Override);
    }
}

public class CassieBroadcast
{
	[Description("The CASSIE message to be sent")]
	public string Message { get; set; } = "";
	[Description("The text to be shown")]
	public string Translation { get; set; } = "";
	[Description("Whether or not to hide the subtitle for the cassie message")]
	public bool ShowSubtitles { get; set; } = true;

	public void Send()
	{
		Cassie.MessageTranslated(
			Message,
			Translation.IsEmpty() ? Message : Translation,
			isSubtitles: ShowSubtitles);
	}

    // Overload for displaying player role (Last Player Alive)
    public void Send(RoleTypeId role)
	{
        Cassie.MessageTranslated(
            Message.Replace("{role}", HelperMethods.CassieRoleTranslations(role)),
            Translation.IsEmpty() ? Message.Replace("{role}", HelperMethods.CassieRoleTranslations(role)) : Translation.Replace("{role}", HelperMethods.CassieRoleTranslations(role)),
            isSubtitles: ShowSubtitles);
    }
}

public static class HelperMethods
{
    public static string CassieRoleTranslations(RoleTypeId role)
    {
        switch (role)
        {
            case RoleTypeId.ClassD:
                return "ClassD";
            case RoleTypeId.Scientist:
                return "Scientist";
            case RoleTypeId.FacilityGuard:
                return "Facility Guard";
            case RoleTypeId.NtfCaptain:
            case RoleTypeId.NtfSpecialist:
            case RoleTypeId.NtfPrivate:
            case RoleTypeId.NtfSergeant:
                return "NineTailedFox";
            case RoleTypeId.ChaosMarauder:
            case RoleTypeId.ChaosConscript:
            case RoleTypeId.ChaosRepressor:
            case RoleTypeId.ChaosRifleman:
                return "ChaosInsurgency";
            default: // shouldnt really happen but it's there just in case
                return "Not found";
        }
    }
}
