using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using ModLoader;

namespace ToggleLifeMeters
{
    [RunStaticConstructor]
    public static class ToggleLifeMeters
    {
        static AccessTools.FieldRef<bool> s_showlifemeters;
        static Func<int, int, bool> query_key;
        public static void Postfix(int __0)
        {
            try
            {
                if (query_key(__0, 10)) s_showlifemeters() = !s_showlifemeters();
            }
            catch (Exception e) { Logger.Exception(e); }
        }
        public static bool Prefix() => false;
        static ToggleLifeMeters()
        {
            //Harmony.DEBUG = true;
            var harmony = new Harmony(MethodBase.GetCurrentMethod().DeclaringType.FullName);

            var query_key_method = AccessTools.Method("#=zNoJhxHjJpHsj9Z3KaQ==:#=zZ_5uF_w=");
            query_key = AccessTools.MethodDelegate<Func<int, int, bool>>(query_key_method);

            var set_life_meters_method = AccessTools.Method("ZX.Components.CLife:#=zH0HA1Qhm$fjBNk9BsUqns9Y=");

            s_showlifemeters = AccessTools.StaticFieldRefAccess<bool>(AccessTools.DeclaredField(AccessTools.TypeByName("ZX.Components.CLife"), "#=zrokJ4RVo9sijLVJmDUenLk1_qedv1EZNFQ=="));
            harmony.Patch(set_life_meters_method, prefix: new HarmonyMethod(((Func<bool>)Prefix).Method));
            harmony.Patch(AccessTools.Method("#=zS8fSyrdcUxJ8jMft0Tv6j$s=:OnKeyDown"), postfix: new HarmonyMethod(((Action<int>)Postfix).Method));
        }
    }
}
