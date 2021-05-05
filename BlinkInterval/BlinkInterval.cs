using HarmonyLib;
using ModLoader;
using System;
using System.Reflection;

namespace BlinkInterval
{
    [RunStaticConstructor]
    public static class BlinkInterval
    {
        static BlinkInterval()
        {
            //Harmony.DEBUG = true;
            var harmony = new Harmony(MethodBase.GetCurrentMethod().DeclaringType.FullName);

            foreach (var difficulty in new string[] { "Easy", "Accessible", "Challenging", "Hard", "Brutal", "Nightmare", "Apocalypse" })
            {
                var method = AccessTools.Method("ZX.ZXCampaignDifficulty_" + difficulty + ":get_NBlinkIntervalSecondsForInteractiveObjects");
                harmony.Patch(method, postfix: new HarmonyMethod(((Func<int, int>)Postfix).Method));
            }
        }
        public static int Postfix(int value) => 1;
    }
}
