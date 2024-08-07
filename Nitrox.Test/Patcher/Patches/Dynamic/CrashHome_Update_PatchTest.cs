using HarmonyLib;
using NitroxTest.Patcher;

namespace NitroxPatcher.Patches.Dynamic;

[TestClass]
public class CrashHome_Update_PatchTest
{
    [TestMethod]
    public void Sanity()
    {
        IEnumerable<CodeInstruction> originalIl = PatchTestHelper.GetInstructionsFromMethod(CrashHome_Update_Patch.TARGET_METHOD);
        IEnumerable<CodeInstruction> transformedIl = CrashHome_Update_Patch.Transpiler(originalIl);
        transformedIl.Count().Should().Be(originalIl.Count() - 5);
    }
}
