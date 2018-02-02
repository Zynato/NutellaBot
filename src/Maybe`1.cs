using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot
{
    public struct Maybe<T>
    {
        public readonly T Value;
        public readonly bool HasValue;

        public Maybe(T value) {
            this.Value = value;
            this.HasValue = true;
        }
    }
}
