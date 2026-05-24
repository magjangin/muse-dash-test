using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels.PnlMusicTag;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;

public static class MusicButtonCellInstanceTracker
{
    private sealed class CellSnapshot
    {
        public string Uid = "(unknown)";
        public string MusicName = "(unknown)";
        public string Author = "(unknown)";
        public string GameObjectName = "(null)";
        public int TabIndex;
        public IntPtr Pointer;
        public int SeenCount;
    }

    private static readonly Dictionary<IntPtr, CellSnapshot> CellsByPointer = new();

    public static void Register(MusicButtonCell cell, MusicInfo musicInfo, int tabIndex, string source)
    {
        if (cell == null || musicInfo == null)
            return;

        IntPtr pointer = cell.Pointer;
        string gameObjectName = cell.gameObject != null ? cell.gameObject.name : "(null)";

        if (!CellsByPointer.TryGetValue(pointer, out var snapshot))
        {
            snapshot = new CellSnapshot
            {
                Pointer = pointer,
                SeenCount = 0
            };
            CellsByPointer[pointer] = snapshot;
        }

        snapshot.Uid = musicInfo.uid ?? "(null)";
        snapshot.MusicName = musicInfo.name ?? "(null)";
        snapshot.Author = musicInfo.author ?? "(null)";
        snapshot.GameObjectName = gameObjectName;
        snapshot.TabIndex = tabIndex;
        snapshot.SeenCount++;

        MelonLogger.Msg($"[MusicButtonCell.InstanceTracker] {source}: registered=True | Ptr=0x{pointer.ToInt64():X} | Seen={snapshot.SeenCount} | Uid={snapshot.Uid} | Name={snapshot.MusicName} | Author={snapshot.Author} | GameObject={snapshot.GameObjectName} | TabIndex={snapshot.TabIndex} | UidInstanceCount={CountByUid(snapshot.Uid)}");
    }

    public static bool Contains(MusicButtonCell cell)
    {
        return cell != null && CellsByPointer.ContainsKey(cell.Pointer);
    }

    public static string Describe(MusicButtonCell cell)
    {
        if (cell == null)
            return "cell=(null)";

        IntPtr pointer = cell.Pointer;
        if (!CellsByPointer.TryGetValue(pointer, out var snapshot))
            return $"Ptr=0x{pointer.ToInt64():X} | tracked=False";

        return $"Ptr=0x{pointer.ToInt64():X} | tracked=True | Uid={snapshot.Uid} | Name={snapshot.MusicName} | GameObject={snapshot.GameObjectName} | Seen={snapshot.SeenCount}";
    }

    public static void LogSummary(string source, string uid = "0-0")
    {
        int total = CellsByPointer.Count;
        var matching = CellsByPointer.Values.Where(x => x.Uid == uid).ToList();
        MelonLogger.Msg($"[MusicButtonCell.InstanceTracker] {source}: totalInstances={total} | uid={uid} | uidInstances={matching.Count}");

        for (int i = 0; i < matching.Count; i++)
        {
            var snapshot = matching[i];
            MelonLogger.Msg($"[MusicButtonCell.InstanceTracker] {source}: uid[{i}] Ptr=0x{snapshot.Pointer.ToInt64():X} | Seen={snapshot.SeenCount} | GameObject={snapshot.GameObjectName} | Name={snapshot.MusicName} | Author={snapshot.Author} | TabIndex={snapshot.TabIndex}");
        }
    }

    private static int CountByUid(string uid)
    {
        return CellsByPointer.Values.Count(x => x.Uid == uid);
    }
}
