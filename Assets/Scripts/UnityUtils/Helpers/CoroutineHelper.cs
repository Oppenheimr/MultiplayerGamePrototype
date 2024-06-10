using System;
using System.Collections;
using UnityEngine;

namespace UnityUtils.Helpers
{
    public static class CoroutineHelper
    {
        public static IEnumerator WaitCondition(Func<bool> condition)
        {
            while (!condition())
                yield return new WaitForEndOfFrame();
        }
        
        public static IEnumerator WaitCondition(Func<bool> condition, int repeat, IEnumerator reAction)
        {
            var counter = 0;
            while (!condition())
            {
                if (counter == repeat)
                {
                    counter = 0;
                    yield return reAction;
                }
                counter++;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}