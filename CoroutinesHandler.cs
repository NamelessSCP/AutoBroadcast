namespace AutoBroadcastSystem;

using Exiled.API.Features;
using MEC;

public static class CoroutinesHandler
{
     public static List<CoroutineHandle>? Coroutines;
     public static void KillCoroutines()
     {
          Log.Debug("Killing coroutines");
          if(Coroutines == null) return;
          foreach(CoroutineHandle coroutine in Coroutines)
          {
               if(coroutine.IsRunning) Timing.KillCoroutines(coroutine);
               Log.Debug("Killed a coroutine successfully!");
          }
          Coroutines.Clear();
          Log.Debug("Killed all coroutines");
     }
}