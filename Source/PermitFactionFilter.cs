using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace PermitFactionFilter
{
    [StaticConstructorOnStartup]
    public static class Startup
    {
        static Startup()
        {
            new Harmony("vincent.permitfactionfilter").PatchAll();
        }
    }

    // Replace FactionManager.AllFactionsVisibleInViewOrder by a filtered version
    // in menu "Change faction" of permit tab.
    [HarmonyPatch(typeof(PermitsCardUtility), nameof(PermitsCardUtility.DrawRecordsCard))]
    public static class PermitsCardUtility_DrawRecordsCard_Patch
    {
        static readonly MethodInfo Original = AccessTools.PropertyGetter(typeof(FactionManager), nameof(FactionManager.AllFactionsVisibleInViewOrder));
        static readonly MethodInfo Replacement = AccessTools.Method(typeof(PermitsCardUtility_DrawRecordsCard_Patch), nameof(FilteredFactions));

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            foreach (var ci in instructions)
            {
                if (ci.Calls(Original))
                {
                    ci.opcode = OpCodes.Call;
                    ci.operand = Replacement;
                    patched = true;
                }
                yield return ci;
            }
            if (!patched)
                Log.Warning("[PermitFactionFilter] Call to AllFactionsVisibleInViewOrder not found : inactive patch.");
        }

        public static IEnumerable<Faction> FilteredFactions(FactionManager manager)
        {
            var withPermits = new HashSet<FactionDef>(
                DefDatabase<RoyalTitlePermitDef>.AllDefs.Where(p => p.faction != null).Select(p => p.faction));
            return manager.AllFactionsVisibleInViewOrder.Where(f => withPermits.Contains(f.def));
        }
    }
}
