using System.Reflection;
using HarmonyLib;
using NitroxClient.GameLogic;
using NitroxModel.DataStructures;
using NitroxModel.Helper;
using static Rocket;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Rocket_ElevatorControlButtonActivate_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo TARGET_METHOD = Reflect.Method((Rocket t) => t.ElevatorControlButtonActivate());

        public static void Prefix(Rocket __instance, out RocketElevatorStates __state)
        {
            __state = __instance.elevatorState;
        }

        public static void Postfix(Rocket __instance, RocketElevatorStates __state)
        {
            if (__state != __instance.elevatorState && __instance.TryGetIdOrWarn(out NitroxId id))
            {
                Resolve<Entities>().EntityMetadataChanged(__instance, id);
            }
        }

        public override void Patch(Harmony harmony)
        {
            PatchMultiple(harmony, TARGET_METHOD, prefix:true, postfix:true);
        }
    }
}
