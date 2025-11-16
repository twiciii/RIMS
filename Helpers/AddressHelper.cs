namespace RIMS.Helpers
{
    public static class AddressHelper
    {
        public static string FormatAddress(string? lotNo, string? blockNo, string? bldgNo, string? purok, string? street)
        {
            var parts = new List<string>();

            if (!string.IsNullOrEmpty(lotNo)) parts.Add($"Lot {lotNo}");
            if (!string.IsNullOrEmpty(blockNo)) parts.Add($"Block {blockNo}");
            if (!string.IsNullOrEmpty(bldgNo)) parts.Add($"Bldg {bldgNo}");
            if (!string.IsNullOrEmpty(purok)) parts.Add($"Purok {purok}");
            if (!string.IsNullOrEmpty(street)) parts.Add(street);

            return parts.Any() ? string.Join(", ", parts) : "Address not specified";
        }

        public static string NormalizePurok(string? purok)
        {
            if (string.IsNullOrWhiteSpace(purok))
                return "Unknown";

            return purok.Trim().ToUpper() switch
            {
                "1" or "PUROK 1" or "ONE" => "Purok 1",
                "2" or "PUROK 2" or "TWO" => "Purok 2",
                "3" or "PUROK 3" or "THREE" => "Purok 3",
                "4" or "PUROK 4" or "FOUR" => "Purok 4",
                "5" or "PUROK 5" or "FIVE" => "Purok 5",
                "6" or "PUROK 6" or "SIX" => "Purok 6",
                "7" or "PUROK 7" or "SEVEN" => "Purok 7",
                "8" or "PUROK 8" or "EIGHT" => "Purok 8",
                "9" or "PUROK 9" or "NINE" => "Purok 9",
                _ => purok.Trim()
            };
        }

        public static bool IsValidPhilippinePhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Philippine mobile numbers: 09XXXXXXXXX or +639XXXXXXXXX
            var pattern = @"^(09|\+639)\d{9}$";
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, pattern);
        }

        public static string ExtractPurokFromAddress(string? address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return "Unknown";

            var purokPatterns = new[]
            {
                "purok 1", "purok 2", "purok 3", "purok 4", "purok 5",
                "purok 6", "purok 7", "purok 8", "purok 9"
            };

            foreach (var pattern in purokPatterns)
            {
                if (address.ToLower().Contains(pattern))
                    return pattern.ToUpper();
            }

            return "Unknown";
        }

        public static List<string> GetPurokList()
        {
            return new List<string>
            {
                "Purok 1",
                "Purok 2",
                "Purok 3",
                "Purok 4",
                "Purok 5",
                "Purok 6",
                "Purok 7",
                "Purok 8",
                "Purok 9",
                "Unknown"
            };
        }
    }
}