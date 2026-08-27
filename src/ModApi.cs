using System.Reflection;
using HarmonyLib;

namespace ReadBookMarker
{
    public class ReadBookMarkerModApi : IModApi
    {
        public void InitMod(Mod _modInstance)
        {
            Log.Out("[ReadBookMarker] init");

            new Harmony("com.lyovi.readbookmarker").PatchAll(Assembly.GetExecutingAssembly());

            ModEvents.WorldShuttingDown.RegisterHandler(OnWorldShuttingDown);
        }

        static void OnWorldShuttingDown(ref ModEvents.SWorldShuttingDownData _data) => KnownBooks.Clear();
    }
}
