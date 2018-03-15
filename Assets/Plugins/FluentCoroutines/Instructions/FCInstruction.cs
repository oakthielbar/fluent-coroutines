using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluentCoroutines.Instructions
{
    public class FCInstruction
    {
        private static Stack<FCInstruction> m_pool;
        private static Stack<FCInstruction> pool
        {
            get
            {
                if (m_pool == null)
                {
                    m_pool = new Stack<FCInstruction>();
                }
                return m_pool;
            }
        }

        public FCInstructionType Type { get; protected set; }
        public YieldInstruction Yield { get; protected set; }
        public Action Action { get; protected set; }
        public Func<IEnumerator> EnumeratorFunc { get; protected set; }

        private FCInstruction() { }

        private static FCInstruction GetInstruction()
        {
            if (FCInstruction.pool.Count == 0)
            {
                return new FCInstruction();
            }
            else
            {
                return pool.Pop();
            }
        }

        public static FCInstruction Create(YieldInstruction _yield)
        {
            FCInstruction instruction = GetInstruction();
            instruction.Yield = _yield;
            instruction.Type = FCInstructionType.Yield;
            return instruction;
        }

        public static FCInstruction Create(Action _action)
        {
            FCInstruction instruction = GetInstruction();
            instruction.Action = _action;
            instruction.Type = FCInstructionType.Action;
            return instruction;
        }

        public static FCInstruction CreateCoroutine(Func<IEnumerator> _coroutine)
        {
            FCInstruction instruction = GetInstruction();
            instruction.EnumeratorFunc = _coroutine;
            instruction.Type = FCInstructionType.Coroutine;
            return instruction;
        }

        public static FCInstruction CreateYield(Func<IEnumerator> _customYield)
        {
            FCInstruction instruction = GetInstruction();
            instruction.EnumeratorFunc = _customYield;
            instruction.Type = FCInstructionType.CustomYield;
            return instruction;
        }

        public void Release()
        {
            Yield = null;
            Action = null;
            EnumeratorFunc = null;
            Type = FCInstructionType.None;
            pool.Push(this);
        }
    }
}