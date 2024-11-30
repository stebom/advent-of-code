namespace AdventOfCode;

internal class Year2015_Day19
{
    static Random random = new();

    static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void Solve()
    {
        var replacements = new List<(string, string)> {
            ("Al", "ThF" ),
            ("Al", "ThRnFAr" ),
            ("B", "BCa" ),
            ("B", "TiB" ),
            ("B", "TiRnFAr" ),
            ("Ca", "CaCa" ),
            ("Ca", "PB" ),
            ("Ca", "PRnFAr" ),
            ("Ca", "SiRnFYFAr" ),
            ("Ca", "SiRnMgAr" ),
            ("Ca", "SiTh" ),
            ("F", "CaF" ),
            ("F", "PMg" ),
            ("F", "SiAl" ),
            ("H", "CRnAlAr" ),
            ("H", "CRnFYFYFAr" ),
            ("H", "CRnFYMgAr" ),
            ("H", "CRnMgYFAr" ),
            ("H", "HCa" ),
            ("H", "NRnFYFAr" ),
            ("H", "NRnMgAr" ),
            ("H", "NTh" ),
            ("H", "OB" ),
            ("H", "ORnFAr" ),
            ("Mg", "BF" ),
            ("Mg", "TiMg" ),
            ("N", "CRnFAr" ),
            ("N", "HSi" ),
            ("O", "CRnFYFAr" ),
            ("O", "CRnMgAr" ),
            ("O", "HP" ),
            ("O", "NRnFAr" ),
            ("O", "OTi" ),
            ("P", "CaP" ),
            ("P", "PTi" ),
            ("P", "SiRnFAr" ),
            ("Si", "CaSi" ),
            ("Th", "ThCa" ),
            ("Ti", "BP" ),
            ("Ti", "TiTi" ),
            ("e", "HF" ),
            ("e", "NAl" ),
            ("e", "OMg" ),
         };

        var mm = "ORnPBPMgArCaCaCaSiThCaCaSiThCaCaPBSiRnFArRnFArCaCaSiThCaCaSiThCaCaCaCaCaCaSiRnFYFArSiRnMgArCaSiRnPTiTiBFYPBFArSiRnCaSiRnTiRnFArSiAlArPTiBPTiRnCaSiAlArCaPTiTiBPMgYFArPTiRnFArSiRnCaCaFArRnCaFArCaSiRnSiRnMgArFYCaSiRnMgArCaCaSiThPRnFArPBCaSiRnMgArCaCaSiThCaSiRnTiMgArFArSiThSiThCaCaSiRnMgArCaCaSiRnFArTiBPTiRnCaSiAlArCaPTiRnFArPBPBCaCaSiThCaPBSiThPRnFArSiThCaSiThCaSiThCaPTiBSiRnFYFArCaCaPRnFArPBCaCaPBSiRnTiRnFArCaPRnFArSiRnCaCaCaSiThCaRnCaFArYCaSiRnFArBCaCaCaSiThFArPBFArCaSiRnFArRnCaCaCaFArSiRnFArTiRnPMgArF";

        { 
            var distinct = new HashSet<string>();
            var str = mm;

            foreach (var replacement in replacements)
            {
                var offset = str.IndexOf(replacement.Item1);
                while (offset != -1)
                {
                    distinct.Add(str[..offset] + replacement.Item2 + str[(offset + replacement.Item1.Length)..]);
                    offset = str.IndexOf(replacement.Item1, offset + replacement.Item1.Length);
                }
            }

            Console.WriteLine($"Part 1: {distinct.Count}");
        }

        int steps;
        int resets = 0;

        while (true) {
            var str = mm;
            steps = 0;

            Shuffle(replacements);

            while (str != "e")
            {
                var modified = str;
                foreach (var replacement in replacements)
                {
                    var (key, value) = replacement;
                    var offset = str.IndexOf(value);
                    if (offset != -1) modified = str[..offset] + key + str[(offset + value.Length)..];
                }

                if (str == modified) break;
                str = modified;

                steps++;
            }

            if (str == "e") break;
            resets++;
        }

        Console.WriteLine($"Part 2: {steps} (after {resets})");
    }
}