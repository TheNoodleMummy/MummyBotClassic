using Mummybot.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Services
{
    public class SnowFlakeGeneratorService : BaseService
    {
        internal const int NumberOfTimeBits = 42;

        internal const int NumberOfGeneratorIdBits = 9;

        internal const int NumberOfSequenceBits = 64 - NumberOfTimeBits - NumberOfGeneratorIdBits;

        private readonly byte[] _buffer = new byte[8];
        private readonly byte[] _timeBytes = new byte[8];
        private readonly byte[] _idBytes = new byte[2];
        private readonly byte[] _sequenceBytes = new byte[2];
        private readonly int _maxSequence = (int)Math.Pow(2, NumberOfSequenceBits) - 1;
        private readonly DateTime _start;

        private short _sequence;
        private long _previousTime;

        private readonly LogService LogService;

        public SnowFlakeGeneratorService(LogService logService)
        {
            CalculateIdBytes(2);
            _start = DateTime.UtcNow;
            LogService = logService;
        }

        public string Next()
        {
            SpinToNextSequence();
            WriteValuesToByteArray(_buffer, _previousTime, _sequence);

            return Convert.ToBase64String(_buffer);
        }

        public ulong NextLong()
        {            
            SpinToNextSequence();
            WriteValuesToByteArray(_buffer, _previousTime, _sequence);

            Array.Reverse(_buffer);
            var id = BitConverter.ToUInt64(_buffer, 0);
            LogService.LogInformation($"Generated new snowflake: {id}", LogSource.SnowFlakeGenerator);
            return id;
        }

        internal unsafe void WriteValuesToByteArray(byte[] target, long time, short sequence)
        {
            fixed (byte* arrayPointer = target)
            {
                *(long*)arrayPointer = 0;
            }

            fixed (byte* arrayPointer = _timeBytes)
            {
                *(long*)arrayPointer = time << (64 - NumberOfTimeBits);
            }

            fixed (byte* arrayPointer = _sequenceBytes)
            {
                *(short*)arrayPointer = sequence;
            }

            WriteValuesToByteArray(target, _timeBytes, _idBytes, _sequenceBytes);
        }

        private unsafe void CalculateIdBytes(short id)
        {
            fixed (byte* arrayPointer = _idBytes)
            {
                *(short*)arrayPointer = (short)(id << (8 - ((64 - NumberOfSequenceBits) % 8)));
            }
        }

        private void WriteValuesToByteArray(byte[] target, byte[] time, byte[] id, byte[] sequence)
        {
            target[0] = (byte)(target[0] | time[7]);
            target[1] = (byte)(target[1] | time[6]);
            target[2] = (byte)(target[2] | time[5]);
            target[3] = (byte)(target[3] | time[4]);
            target[4] = (byte)(target[4] | time[3]);
            target[5] = (byte)(target[5] | time[2]);
            target[6] = (byte)(target[6] | time[1]);
            target[7] = (byte)(target[7] | time[0]);

            target[5] = (byte)(target[5] | id[1]);
            target[6] = (byte)(target[6] | id[0]);

            target[6] = (byte)(target[6] | sequence[1]);
            target[7] = (byte)(target[7] | sequence[0]);
        }

        private void SpinToNextSequence()
        {
            var time = GetTime();

            while (time == _previousTime && _sequence >= _maxSequence)
            {
                time = GetTime();
            }

            _sequence = time == _previousTime ? (short)(_sequence + 1) : (short)0;
            _previousTime = time;
        }

        private long GetTime()
        {
            return (long)(DateTime.UtcNow - _start).TotalMilliseconds;
        }
    }
}
