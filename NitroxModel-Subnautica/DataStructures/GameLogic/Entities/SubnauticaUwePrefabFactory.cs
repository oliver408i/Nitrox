using System;
using System.Collections.Generic;
using LitJson;
using NitroxModel.DataStructures.GameLogic.Entities;
using NitroxModel.Helper;
using static LootDistributionData;

namespace NitroxModel_Subnautica.DataStructures.GameLogic.Entities;

public class SubnauticaUwePrefabFactory : IUwePrefabFactory
{
    private readonly LootDistributionData lootDistributionData;
    private readonly Dictionary<string, List<UwePrefab>> cache = new();

    public SubnauticaUwePrefabFactory(string lootDistributionJson)
    {
        lootDistributionData = GetLootDistributionData(lootDistributionJson);
    }

    public bool TryGetPossiblePrefabs(string biome, out List<UwePrefab> prefabs)
    {
        if (biome == null)
        {
            prefabs = null;
            return false;
        }
        if (cache.TryGetValue(biome, out prefabs))
        {
            return true;
        }

        prefabs = new();
        BiomeType biomeType = (BiomeType)Enum.Parse(typeof(BiomeType), biome);
        if (lootDistributionData.GetBiomeLoot(biomeType, out DstData dstData))
        {
            foreach (PrefabData prefabData in dstData.prefabs)
            {
                if (lootDistributionData.srcDistribution.TryGetValue(prefabData.classId, out SrcData srcData))
                {
                    // Manually went through the list of those to make this "filter"
                    // You can verify this by looping through all of SrcData (e.g in LootDistributionData.Initialize)
                    // print the prefabPath and check the TechType related to the provided classId (WorldEntityDatabase.TryGetInfo) with PDAScanner.IsFragment
                    bool isFragment = srcData.prefabPath.Contains("Fragment") || srcData.prefabPath.Contains("BaseGlassDome");
                    prefabs.Add(new(prefabData.classId, prefabData.count, prefabData.probability, isFragment));
                }
            }
        }
        cache[biome] = prefabs;
        return true;
    }

    private LootDistributionData GetLootDistributionData(string lootDistributionJson)
    {
        // LitJson uses the computer's local CultureInfo when parsing the JSON files.
        // However, these json files were saved in en_US. Ensure that this is done for the current thread.
        CultureManager.ConfigureCultureInfo();

        JsonMapper.RegisterImporter((double value) => Convert.ToSingle(value));

        Dictionary<string, LootDistributionData.SrcData> result = JsonMapper.ToObject<Dictionary<string, LootDistributionData.SrcData>>(lootDistributionJson);

        LootDistributionData lootDistributionData = new LootDistributionData();
        lootDistributionData.Initialize(result);

        return lootDistributionData;
    }
}
