using System.ComponentModel;
using LabApi.Features.Wrappers;

namespace AutoBroadcast.Objects;

public class BroadcastSystem
{
    [Description("How long the hint/broadcast should show")]
    public ushort Duration { get; set; } = 3;
        
    [Description("The message shown on the broadcast")]
    public string Message { get; set; } = string.Empty;

    [Description("Whether it should override - Only works for Broadcast")]
    public bool Override { get; set; }

    [Description("Whether or not to use hints instead")]
    public bool UseHints { get; set; }

    public void Show()
    {
        foreach (Player player in Player.List)
            Show(player);
    }

    public void Show(Player player)
    {
        if (UseHints)
            player.SendHint(Message, Duration);
        else 
            player.SendBroadcast(Message, Duration, default, Override);
    }
}