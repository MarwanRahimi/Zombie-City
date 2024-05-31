using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unity.Profiling;
using Unity.Profiling.LowLevel.Unsafe;
using System.IO;

public class ProfilerCategoriesOutput {
    struct StatInfo {
        public ProfilerCategory Cat;
        public string Name;
        public ProfilerMarkerDataUnit Unit;
    }

    public static void EnumerateProfilerStats() {
        var availableStatHandles = new List<ProfilerRecorderHandle>();
        ProfilerRecorderHandle.GetAvailable(availableStatHandles);

        var availableStats = new List<StatInfo>(availableStatHandles.Count);
        foreach (var h in availableStatHandles) {
            var statDesc = ProfilerRecorderHandle.GetDescription(h);
            var statInfo = new StatInfo() {
                Cat = statDesc.Category,
                Name = statDesc.Name,
                Unit = statDesc.UnitType
            };
            availableStats.Add(statInfo);
        }
        availableStats.Sort((a, b) => {
            var result = string.Compare(a.Cat.ToString(), b.Cat.ToString());
            if (result != 0)
                return result;

            return string.Compare(a.Name, b.Name);
        });

        var sb = new StringBuilder("Available stats:\n");
        foreach (var s in availableStats) {
            sb.AppendLine($"{s.Cat}\t\t - {s.Name}\t\t - {s.Unit}");
        }

        Debug.Log(sb.ToString());
        //File.WriteAllLines(@"D:\Repos\Unity3D Course\Undead Elusion - Data\categories.txt", sb);
        File.WriteAllText(@"D:\Repos\Unity3D Course\Undead Elusion - Data\categories.txt", sb.ToString());
    }
}