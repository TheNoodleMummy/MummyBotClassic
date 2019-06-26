using Mummybot.Attributes;
using Mummybot.Exceptions;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mummybot.Extentions
{
    public partial class Extentions
    {
        public static CommandService AddTypeParsers(this CommandService commands, Assembly assembly)
        {
            var typeParserInterface = commands.GetType().Assembly.GetTypes()
                .FirstOrDefault(x => x.Name == "ITypeParser")?.GetTypeInfo();

            if (typeParserInterface is null)
                throw new QuahuRenamedException("ITypeParser");

            var parsers = assembly.GetTypes().Where(x => typeParserInterface.IsAssignableFrom(x) && x.GetCustomAttribute<DoNotAutoAddAttribute>() == null);

            var internalAddParser = commands.GetType().GetMethod("AddParserInternal",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (internalAddParser is null)
                throw new QuahuRenamedException("AddParserInternal");

            foreach (var parser in parsers)
            {

                var targetType = parser.BaseType.GetGenericArguments().First();

                internalAddParser.Invoke(commands, new[] { targetType, Activator.CreateInstance(parser), true });
            }

            return commands;
        }
    }
}
