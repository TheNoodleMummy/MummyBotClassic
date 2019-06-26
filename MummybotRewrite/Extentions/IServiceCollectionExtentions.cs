using Microsoft.Extensions.DependencyInjection;
using Mummybot.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mummybot.Extentions
{
    public partial class Extentions
    {
        public static IServiceCollection AddServices(this IServiceCollection collection, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                collection.AddSingleton(type);
            }

            return collection;
        }
    }
}
