using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Converter
{
    public static class Dictionarys
    {
        public static readonly Dictionary<int, string> NurseNames = new()
        {
            { 0, "Anna" },
            { 1, "Ben" },
            { 2, "Clara" },
            { 3, "David" },
            {4, "Emil" },
            { 5, "Frederick" },
            { 6, "Gustav" },
            {7, "Hannah" },
            { 8, "Isa" },
            {9, "Jason" },
            { 10, "Klaus" },
            { 11, "Luis" },
            { 12, "Melanie" },
            { 13, "Norbert" },
            { 14, "Olaf" },
            { 15, "Paul" },
            { 16, "Quando" },
            { 17, "Ronaldo" },
            { 18, "Seb" },
            { 19, "Tanja" },
            { 20, "Uli" },
            { 21, "Vektor" },
            { 22, "Willi" },
            { 23, "Yakup" },
            { 24, "Zandra" },
            { 25, "Albert" },
            { 26, "Billi" },
            { 27, "Caesar" },
            { 28, "Dora" },
            { 29, "Eberhart" },
        };

        public static readonly Dictionary<int, string> DayNames = new()
        {
            { 0, "Montag" },
            { 1, "Dienstag" },
            { 2, "Mittwoch" }
        };

        public static readonly Dictionary<int, string> ShiftNames = new()
        {
            { 0, "Früh" },
            { 1, "Spät" },
            { 2, "Nacht" }
        };
    }
}
