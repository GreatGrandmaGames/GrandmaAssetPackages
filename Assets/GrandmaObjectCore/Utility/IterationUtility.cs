using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class IterationUtility
    {
        public static void ForEach2D(int x, int y, Action<int, int> action)
        {
            for(int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    action(i, j);
                }
            }
        }
    }
}
