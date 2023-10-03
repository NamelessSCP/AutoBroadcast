namespace AutoBroadcastSystem;

using Exiled.API.Features;
using MEC;

public static class CoroutinesHandler
{
     public static List<CoroutineHandle> Coroutines = new();
     public static void KillCoroutines()
     {
          Log.Debug("Killing coroutines");
          foreach(CoroutineHandle coroutine in Coroutines)
          {
               Timing.KillCoroutines(coroutine);
               Log.Debug("Killed a coroutine successfully!");
          }
          Coroutines.Clear();
          Log.Debug("Killed all coroutines");
     }
}