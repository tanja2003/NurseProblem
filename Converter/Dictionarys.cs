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
            {9, "Jason" }
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
