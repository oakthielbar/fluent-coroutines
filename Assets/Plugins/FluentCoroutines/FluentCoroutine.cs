using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using FluentCoroutines.Instructions;

namespace FluentCoroutines
{
    /// <summary>
    /// FluentCoroutine can be used like coroutines, but they are built using a fluent interface in order
    /// to facilitate a more semantic coding style. They also remove the need for special coroutine
    /// functions filled with unwieldy yield stxatements, but they also support those functions for
    /// users who want to use them (or who want to integrate their existing coroutines with FluentCoroutine).
    /// FluentCoroutine is great for composing asyncronous behaviors by stitching multiple smaller syncronous
    /// functions together.
    /// </summary>
    public class FluentCoroutine : IEnumerable
    {
        public static FluentCoroutine Finalize(FCBuilder builder)
        {
            FluentCoroutine fluentCoroutine;
            fluentCoroutine = new FluentCoroutine();
            fluentCoroutine.executionContext = builder.ExecutionContext;
            fluentCoroutine.instructions.AddRange(builder.Instructions);
            return fluentCoroutine;
        }

        /// <summary>
        /// The MonoBehaviour that will be used for starting and stopping coroutines.
        /// </summary>
        private MonoBehaviour executionContext;

        /// <summary>
        /// A list of instructions to be executed for this FluentCoroutine.
        /// </summary>
        private List<FCInstruction> instructions = new List<FCInstruction>();

        /// <summary>
        /// A stack of coroutines used for stopping nested coroutine calls.
        /// </summary>
        /// <seealso cref="Stop"/>
        private Stack<Coroutine> coroutines = new Stack<Coroutine>();

        /// <summary>
        /// Tracks whether this FluentCoroutine is currently executing.
        /// </summary>
        public bool IsExecuting
        {
            get { return coroutines.Count > 0; }
        }

        /// <summary>
        /// Gets an IEnumerator for this FluentCoroutine. If used in a yield statement,
        /// it will wait until the current execution is complete.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return new WaitWhile(() => IsExecuting);
        }

        /// <summary>
        /// Creates an empty FluentCoroutine.
        /// </summary>
        /// <param name="executionContext">The MonoBehaviour that will be used for starting and stopping coroutines.</param>
        /// <returns>An empty FluentCoroutine.</returns>
        /// <remarks>
        /// The <see cref="executionContext"/> MonoBehaviour must be enabled and its 
        /// GameObject must be active when calling <see cref="Execute"/>.
        /// </remarks> 
        public static FluentCoroutine Create(MonoBehaviour executionContext)
        {
            FluentCoroutine fluentCoroutine;
            fluentCoroutine = new FluentCoroutine();
            fluentCoroutine.executionContext = executionContext;
            return fluentCoroutine;
        }

        /// <summary>
        /// Basic constructor. Initializes collections.
        /// </summary>
        /// <param name="_executionContext"></param>
        private FluentCoroutine()
        {
            instructions = new List<FCInstruction>();
            coroutines = new Stack<Coroutine>();
        }

        /// <summary>
        /// Executes the FluentCoroutine's instructions by starting a coroutine on the <see cref="executionContext"/>.
        /// </summary>
        /// <returns>The FluentCoroutine on which this was called.</returns>
        public FluentCoroutine Execute()
        {
            if (instructions.Count == 0)
            {
                Debug.LogWarning("Attempted to execute a Fluent Coroutine with no instructions.");
            }
            coroutines.Push(executionContext.StartCoroutine(ExecuteRoutine()));
            return this;
        }

        /// <summary>
        /// Iterates through every instruction in <see cref="instructions"/>. This runs as a coroutine on <see cref="executionContext"/>.
        /// </summary>
        private IEnumerator ExecuteRoutine()
        {
            foreach (FCInstruction instruction in instructions)
            {
                switch (instruction.Type)
                {
                    case FCInstructionType.None:
                        Debug.LogWarning("Attempted to execute a Fluent Coroutine instruction with no instruction type.");
                        break;
                    case FCInstructionType.Yield:
                        yield return instruction.Yield;
                        break;
                    case FCInstructionType.CustomYield:
                        yield return instruction.EnumeratorFunc();
                        break;
                    case FCInstructionType.Action:
                        instruction.Action();
                        break;
                    case FCInstructionType.Coroutine:
                        Coroutine coroutine = executionContext.StartCoroutine(instruction.EnumeratorFunc());
                        coroutines.Push(coroutine);
                        yield return coroutine;
                        coroutines.Pop();
                        break;
                    default:
                        Debug.LogWarning("Attempted to execute a Fluent Coroutine instruction with an unrecognized instruction type.");
                        break;
                }
            }
            coroutines.Pop();

            yield break;
        }

        /// <summary>
        /// Stops the execution of the FluentCoroutine.
        /// </summary>
        /// <returns>The FluentCoroutine on which this was called.</returns>
        public FluentCoroutine Stop()
        {
            while (coroutines.Count > 0)
            {
                executionContext.StopCoroutine(coroutines.Pop());
            }
            return this;
        }
    }
}