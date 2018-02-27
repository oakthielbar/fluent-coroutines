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

        public FCInstructionType type { get; protected set; }
        public YieldInstruction yield { get; protected set; }
        public IEnumerator customYield { get; protected set; }
        public Action action { get; protected set; }
        public Func<IEnumerator> coroutine { get; protected set; }

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
            instruction.yield = _yield;
            instruction.type = FCInstructionType.Yield;
            return instruction;
        }

        public static FCInstruction Create(IEnumerator _customYield)
        {
            FCInstruction instruction = GetInstruction();
            instruction.customYield = _customYield;
            instruction.type = FCInstructionType.CustomYield;
            return instruction;
        }

        public static FCInstruction Create(Action _action)
        {
            FCInstruction instruction = GetInstruction();
            instruction.action = _action;
            instruction.type = FCInstructionType.Action;
            return instruction;
        }

        public static FCInstruction Create(Func<IEnumerator> _coroutine)
        {
            FCInstruction instruction = GetInstruction();
            instruction.coroutine = _coroutine;
            instruction.type = FCInstructionType.Coroutine;
            return instruction;
        }

        public void Release()
        {
            yield = null;
            customYield = null;
            action = null;
            coroutine = null;
            type = FCInstructionType.None;
            pool.Push(this);
        }
    }
}