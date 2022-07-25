using loofer.Iw4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace loofer.Utils
{
    class Utility
    {
        public static bool isValidWeapon(string Weapon)
        {
            if (Enum.IsDefined(typeof(Iw4Structs.weaponIndex), Weapon))
                return true;

            return false;
        }
        public static bool isValidAnimation(string Animation)
        {
            if (Enum.IsDefined(typeof(Iw4Structs.animIndex), Animation))
                return true;

            return false;
        }
        public static bool IsNumeric(object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is short || Expression is int || Expression is long || Expression is decimal || Expression is float || Expression is double || Expression is bool)
                return true;

            try
            {
                if (Expression is string)
                    double.Parse(Expression as string);
                else
                    double.Parse(Expression.ToString());
                return true;
            }
            catch { }
            return false;
        }
        public static bool iCompare(string string1, string string2)
        {
            if (string1.ToUpper().Contains(string2.ToUpper()))
                return true;

            return false;
        }
        public static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}
