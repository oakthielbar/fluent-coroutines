using UnityEngine;

namespace FluentCoroutines
{
    public static class MonoBehaviourExtensions
    {
        public static IFCInitializer FluentCoroutine(this MonoBehaviour monoBehaviour)
        {
            return FCBuilder.Initialize(monoBehaviour);
        }
    }
}