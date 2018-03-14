using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluentCoroutines
{
    public interface IFCInitializer
    {
        FCBuilder Do(Action action);

        FCBuilder Do(Func<IEnumerator> coroutine);

        FCBuilder WaitUntil(bool condition);

        FCBuilder WaitUntil(Func<bool> func);

        FCBuilder WaitWhile(bool condition);

        FCBuilder WaitWhile(Func<bool> func);

        FCBuilder WaitForSeconds(float seconds);

        FCBuilder WaitForSecondsRealtime(float seconds);

        FCBuilder WaitForFrame();

        FCBuilder WaitForFrames(uint count);

        FCBuilder Yield(YieldInstruction yieldInstruction);

        FCBuilder Yield(IEnumerator yieldInstruction);
    }
}