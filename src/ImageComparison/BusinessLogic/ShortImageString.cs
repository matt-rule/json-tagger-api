using System;
using System.Linq;

namespace JsonTaggerApi.ImageComparison.BusinessLogic
{
    public static class ShortImageString
    {
        public const int PossibleValues = 16;

        public const string SisChars = "0123456789ABCDEF";

        public static char ValueToChar(int value) =>
            (value < 0)
            ? SisChars.First()
            :
                (value > SisChars.Length)
                ? SisChars.Last()
                : SisChars[value];

        public static int Index(char c) =>
            SisChars.IndexOf(c);

        public static Func<char, int> CharToValue = (c) => {
            int index = SisChars.IndexOf(c);
            return index == -1 ? 0 : index;
        };
    }
}