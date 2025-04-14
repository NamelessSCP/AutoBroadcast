using System.ComponentModel;
using LabApi.Features.Extensions;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace AutoBroadcast.Objects;

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
        Cassie.Message(Message, isSubtitles: ShowSubtitles, customSubtitles: Translation);
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
        Cassie.Message(
            Message.Replace("{role}", role.GetTeam().ToString()),
            isSubtitles: ShowSubtitles,
            customSubtitles: Translation.Replace("{role}", role.GetFullName()));
    }
}