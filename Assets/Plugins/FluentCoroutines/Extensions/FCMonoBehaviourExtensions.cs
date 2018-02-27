using UnityEngine;

namespace FluentCoroutines
{
    public static class FCMonoBehaviourExtensions
    {
        public static IFCInitializer FluentCoroutine(this MonoBehaviour monoBehaviour)
        {
            return FCBuilder.Initialize(monoBehaviour);
        }
    }
}