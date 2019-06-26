﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Extentions
{
    static partial class Extentions
    {
        public static T Invoke<T>(this Action<T> action) where T : new()
        {
            var obj = new T();
            action.Invoke(obj);

            return obj;
        }
    }
}
