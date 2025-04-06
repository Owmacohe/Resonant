using System;
using System.Linq;

namespace Resonant.Runtime
{
    public static class ResonantUtilities
    {
        /// <summary>
        /// A method to quickly determine if an object has some attribute
        /// </summary>
        /// <param name="obj">The object being checked</param>
        /// <typeparam name="A">The attribute to check for</typeparam>
        /// <returns>Whether the attribute is found for the object's type</returns>
        public static bool HasAttribute<A>(object obj) where A : Attribute
        {
            try
            {
                string unused = ((A) obj.GetType().GetCustomAttributes(
                    typeof(A),
                    true
                ).FirstOrDefault())!.ToString();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}