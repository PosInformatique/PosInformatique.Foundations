//-----------------------------------------------------------------------
// <copyright file="NameTestData.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique
{
    public static class NameTestData
    {
        public static TheoryData<string, string> ValidFirstNames { get; } = new()
        {
            { "Jean", "Jean" },
            { "JEAN", "Jean" },
            { "  jean  ", "Jean" },
            { "  jean-patrick  ", "Jean-Patrick" },
            { "  jean- patrick  ", "Jean-Patrick" },
            { "  jean- -patrick  ", "Jean-Patrick" },
            { "                    jean- -patrick                              ", "Jean-Patrick" },
            { "  émile  ", "Émile" },
            { "  jEAN-éMILE  ", "Jean-Émile" },
        };

        public static TheoryData<string> InvalidFirstNames { get; } = new()
        {
            null,
            "$$Jean",
            "Jean@$+Patrick",
            "Jean-Patrick.",
            string.Empty,
            "    ",
            "                                                            ",
        };

        public static TheoryData<string, string> ValidLastNames { get; } = new()
        {
            { "dupont", "DUPONT" },
            { "DUPONT", "DUPONT" },
            { "  Dupont  ", "DUPONT" },
            { "  Du  pont  ", "DU PONT" },
            { "  Du-pont  ", "DU-PONT" },
            { "  émile  ", "ÉMILE" },
            { "                    Du pont                                ", "DU PONT" },
        };

        public static TheoryData<string> InvalidLastNames { get; } = new()
        {
            null,
            "$$Dupont",
            "Du@$+Pont",
            "Du-pont.",
            string.Empty,
            "   ",
        };
    }
}
