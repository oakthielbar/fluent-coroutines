using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluentCoroutines.Instructions;

namespace FluentCoroutines
{
    public class FCBuilder : IFCInitializer
    {
        /// <summary>
        /// The MonoBehaviour that will be used for starting and stopping coroutines.
        /// </summary>
        public MonoBehaviour ExecutionContext { get; private set; }

        /// <summary>
        /// A list of instructions to be executed for this FluentCoroutine.
        /// </summary>
        public List<FCInstruction> Instructions { get; private set; }

        /// <summary>
        /// Creates and returns an FCBuilder as an IFCInitializer
        /// </summary>
        /// <param name="_executionContext"></param>
        /// <returns></returns>
        public static IFCInitializer Initialize(MonoBehaviour _executionContext)
        {
            FCBuilder builder = new FCBuilder();
            builder.ExecutionContext = _executionContext;
            return builder as IFCInitializer;
        }

        /// <summary>
        /// Basic constructor.
        /// </summary>
        private FCBuilder()
        {
            Instructions = new List<FCInstruction>();
        }

        /// <summary>
        /// This will create and return a FluentCoroutine containing the current instruction set.
        /// </summary>
        /// <returns>Finalized form of the FluentCoroutine.</returns>
        public FluentCoroutine Finalize()
        {
            return FluentCoroutine.Finalize(this);
        }

        /// <summary>
        /// Set the next instruction to <paramref name="action"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// When executing, the FluentCoroutine will wait for the action provided in the argument to
        /// finish execution before proceeding to the next instruction.
        /// </remarks>
        public FCBuilder Do(Action action)
        {
            Instructions.Add(FCInstruction.Create(action));
            return this;
        }

        /// <summary>
        /// Set the next instruction to <paramref name="coroutine"/>.
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// When executing, the FluentCoroutine will wait for the coroutine provided in the argument to
        /// finish execution before proceeding to the next instruction.
        /// </remarks>
        public FCBuilder Do(Func<IEnumerator> coroutine)
        {
            Instructions.Add(FCInstruction.Create(coroutine));
            return this;
        }

        /// <summary>
        /// Set the next instruction to be a WaitUntil yield instruction.
        /// </summary>
        /// <param name="condition">Should only ever be a property.</param>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// See the Unity Scripting API documentation on WaitUntil for more information.
        /// </remarks>
        // TODO: Evaluate this for removal based on risk of misuse
        public FCBuilder WaitUntil(bool condition)
        {
            return WaitUntil(() => condition);
        }

        /// <summary>
        /// Set the next instruction to be a WaitUntil yield instruction.
        /// </summary>
        /// <param name="func"></param>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// See the Unity Scripting API documentation on WaitUntil for more information.
        /// </remarks>
        public FCBuilder WaitUntil(Func<bool> func)
        {
            CustomYieldInstruction yieldInstruction = new UnityEngine.WaitUntil(func);
            return Yield(yieldInstruction);
        }

        /// <summary>
        /// Set the next instruction to be a WaitWhile yield instruction.
        /// </summary>
        /// <param name="condition">Should only ever be a property.</param>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// See the Unity Scripting API documentation on WaitWhile for more information.
        /// </remarks>
        // TODO: Evaluate this for removal based on risk of misuse
        public FCBuilder WaitWhile(bool condition)
        {
            return WaitWhile(() => condition);
        }

        /// <summary>
        /// Set the next instruction to be a WaitWhile yield instruction.
        /// </summary>
        /// <param name="func"></param>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// See the Unity Scripting API documentation on WaitWhile for more information.
        /// </remarks>
        public FCBuilder WaitWhile(Func<bool> func)
        {
            CustomYieldInstruction yieldInstruction = new UnityEngine.WaitWhile(func);
            return Yield(yieldInstruction);
        }

        /// <summary>
        /// Set the next instruction to be a WaitWForSeconds yield instruction.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// See the Unity Scripting API documentation on WaitForSeconds for more information.
        /// </remarks>
        public FCBuilder WaitForSeconds(float seconds)
        {
            WaitForSeconds wfs = new WaitForSeconds(seconds);
            return Yield(wfs);
        }

        /// <summary>
        /// Set the next instruction to be a WaitForSecondsRealtime yield instruction.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// See the Unity Scripting API documentation on WaitForSecondsRealtime for more information.
        /// </remarks>
        public FCBuilder WaitForSecondsRealtime(float seconds)
        {
            WaitForSecondsRealtime wfsrt = new WaitForSecondsRealtime(seconds);
            return Yield(wfsrt);
        }

        /// <summary>
        /// Set the next instruction to wait for one frame.
        /// </summary>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// This is equivalent to using <c>yield return null;</c> in a coroutine.
        /// </remarks>
        public FCBuilder WaitForFrame()
        {
            Instructions.Add(FCInstruction.Create(null as YieldInstruction));
            return this;
        }

        /// <summary>
        /// Set the next instruction to wait for the given number of frames.
        /// </summary>
        /// <param name="count">The number of frames to wait.</param>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// This adds a number of <see cref="WaitForFrame"/> instructions equal to the <paramref name="count"/>.
        /// </remarks>
        public FCBuilder WaitForFrames(uint count)
        {
            for (int i = 0; i < count; ++i)
            {
                WaitForFrame();
            }
            return this;
        }

        /// <summary>
        /// Set the next instruction to <paramref name="yieldInstruction"/>.
        /// </summary>
        /// <param name="yieldInstruction"></param>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// See the Unity Scripting API documentation on YieldInstruction for more information.
        /// </remarks>
        public FCBuilder Yield(YieldInstruction yieldInstruction)
        {
            Instructions.Add(FCInstruction.Create(yieldInstruction));
            return this;
        }

        /// <summary>
        /// Set the next instruction to <paramref name="customYieldInstruction"/>.
        /// </summary>
        /// <param name="customYieldInstruction"></param>
        /// <returns>The FCBuilder on which this was called.</returns>
        /// <remarks>
        /// See the Unity Scripting API documentation on CustomYieldInstruction for more information.
        /// </remarks>
        public FCBuilder Yield(IEnumerator yieldInstruction)
        {
            Instructions.Add(FCInstruction.Create(yieldInstruction));
            return this;
        }
    }
}