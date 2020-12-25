using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day25
{
    public class Problem
    {
        private static readonly long _initialSubjectNumber = 7;
        private static readonly long _transform = 20201227;
        public static void Solve()
        {
            (long, long) actual = (13233401, 6552760);
            (long, long) test = (5764801, 17807724);
            var cardLoopSize = GetLoopSize(actual.Item1);
            var doorLoopSize = GetLoopSize(actual.Item2);
            var cardEncryptionKey = GetEncryptionKey(actual.Item1, doorLoopSize);
            var doorEncryptionKey = GetEncryptionKey(actual.Item2, cardLoopSize);

            if (cardEncryptionKey.Equals(doorEncryptionKey))
                Console.WriteLine(cardEncryptionKey);
            else
                Console.WriteLine("Encryption keys do not match");

        }

        private static long Transform(long value, long seed)
        {
            value *= seed;
            value %= _transform;
            return value;
        }

        private static int GetLoopSize(long publicKey)
        {
            long result = 1;
            int loopSize = 0;
            while (result != publicKey)
            {
                loopSize++;
                result = Transform(result, _initialSubjectNumber);
            }
            return loopSize;
        }

        private static long GetEncryptionKey(long publicKey, int loopSize)
        {
            long encryptionKey = 1;

            for (int i = 0; i < loopSize; i++)
                encryptionKey = Transform(encryptionKey, publicKey);

            return encryptionKey;
        }
    }
}
