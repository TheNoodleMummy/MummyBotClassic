using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Commands.TypeReaders
{
    public struct TypeReaderValue
    {
        public object Value { get; }
        public float Score { get; }

        public TypeReaderValue(object value, float score)
        {
            Value = value;
            Score = score;
        }

        public override string ToString() => Value?.ToString();
        private string DebuggerDisplay => $"[{Value}, {Math.Round(Score, 2)}]";
    }
}
