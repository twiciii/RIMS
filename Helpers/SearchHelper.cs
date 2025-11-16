using System.Text;

namespace RIMS.Helpers
{
    public static class SearchHelper
    {
        public static string NormalizeSearchTerm(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return string.Empty;

            // Remove extra spaces and convert to lowercase
            var normalized = searchTerm.Trim().ToLower();

            // Remove common special characters but keep spaces
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static bool IsRelevantSearch(string searchTerm, string target, double threshold = 0.7)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(target))
                return false;

            var normalizedSearch = NormalizeSearchTerm(searchTerm);
            var normalizedTarget = NormalizeSearchTerm(target);

            // Exact match
            if (normalizedTarget.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase))
                return true;

            // Partial word matches
            var searchWords = normalizedSearch.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var targetWords = normalizedTarget.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var matchCount = searchWords.Count(searchWord =>
                targetWords.Any(targetWord => targetWord.StartsWith(searchWord, StringComparison.OrdinalIgnoreCase)));

            var relevance = (double)matchCount / searchWords.Length;
            return relevance >= threshold;
        }

        public static List<string> GenerateSearchPermutations(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<string>();

            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var permutations = new List<string>();

            if (parts.Length >= 1)
                permutations.Add(parts[0]); // First name only

            if (parts.Length >= 2)
            {
                permutations.Add(parts[1]); // Last name only
                permutations.Add($"{parts[0]} {parts[1]}"); // First + Last
            }

            if (parts.Length >= 3)
            {
                permutations.Add($"{parts[0]} {parts[2]}"); // First + Middle
                permutations.Add($"{parts[0]} {parts[1]} {parts[2]}"); // Full name
                permutations.Add($"{parts[1]}, {parts[0]} {parts[2]}"); // Last, First Middle
            }

            // FIXED: Return the permutations
            return permutations;
        }

        public static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }

            return value;
        }
    }
}