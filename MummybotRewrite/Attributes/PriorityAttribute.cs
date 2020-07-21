using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Attributes
{
    public class InitilizerPriorityAttribute : Attribute
    {
        public int value;
        public InitilizerPriorityAttribute(int weight)
        {
            value = weight;
        }
    }
}
