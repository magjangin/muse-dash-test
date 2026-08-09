using System.Collections.Generic;

namespace muse_dash_test
{
    internal static class ScenePatchHelpers
    {
        public static bool IsSixDigitUid(string uid)
        {
            if (string.IsNullOrEmpty(uid) || uid.Length != 6) return false;
            for (int i = 0; i < uid.Length; i++)
            {
                if (uid[i] < '0' || uid[i] > '9') return false;
            }

            return true;
        }

        public static void CountZz(SortedDictionary<string, int> counts, string uid)
        {
            if (string.IsNullOrEmpty(uid) || uid.Length < 2) return;
            string zz = uid.Substring(0, 2);
            counts[zz] = counts.TryGetValue(zz, out int count) ? count + 1 : 1;
        }

        public static string FormatZzCounts(SortedDictionary<string, int> counts)
        {
            if (counts == null || counts.Count == 0) return "{}";

            var parts = new List<string>();
            foreach (var pair in counts)
            {
                parts.Add($"{pair.Key}:{pair.Value}");
            }

            return "{" + string.Join(", ", parts) + "}";
        }
    }
}
