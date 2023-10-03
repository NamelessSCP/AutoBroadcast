using Exiled.API.Features;
using System.ComponentModel;

namespace AutoBroadcastSystem
{
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
			Cassie.MessageTranslated(Message, Translation.IsEmpty() ? Message : Translation, isSubtitles: ShowSubtitles);
		}
	}
}
