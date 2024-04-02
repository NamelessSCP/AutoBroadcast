using Exiled.API.Features;
using PlayerRoles;
using System.ComponentModel;
using AutoBroadcastSystem.Events;
using Exiled.API.Extensions;

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
	public string Message { get; set; } = string.Empty;
	[Description("Override broadcasts")]
	public bool Override { get; set; }
	[Description("Whether or not to use hints instead")]
	public bool UseHints { get; set; }

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
}

public class CassieBroadcast
{
	[Description("The CASSIE message to be sent")]
	public string Message { get; set; } = "";
	[Description("The text to be shown")]
	public string Translation { get; set; } = string.Empty;
	[Description("Whether or not to hide the subtitle for the cassie message")]
	public bool ShowSubtitles { get; set; } = true;

	public void Send()
	{
		Cassie.MessageTranslated(Message,Translation.IsEmpty() ? Message : Translation, isSubtitles: ShowSubtitles);
	}
	
	public void Send(Player player)
	{
		player.MessageTranslated(Message, Translation.IsEmpty() ? Message : Translation, isSubtitles: ShowSubtitles);
	}
	
    /// <summary>
    /// Sends the CASSIE to all players replacing "{role}" with <paramref name="role"/>'s full name.
    /// </summary>
    /// <remarks>Used by <see cref="Handler.OnDied"/></remarks>
    public void Send(RoleTypeId role)
	{
        Cassie.MessageTranslated(
            Message.Replace("{role}", RoleExtensions.GetTeam(role).ToString()),
            Translation.IsEmpty() ? Message.Replace("{role}", RoleExtensions.GetTeam(role).ToString()) : Translation.Replace("{role}", role.GetFullName()),
            isSubtitles: ShowSubtitles);
    }
}