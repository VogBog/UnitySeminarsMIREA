using System;
using UnityEngine;

namespace Extensions
{
    public static class ObjectExtensions
    {
        public static T FindFirstObjectByTypeOrException<T>() where T : UnityEngine.Object
        {
            var result = UnityEngine.Object.FindFirstObjectByType<T>();
            if(result == null)
                throw new NullReferenceException($"Cannot find object of type {typeof(T).Name}");
            return result;
        }

        public static T FindFirstObjectByTypeOrException<T>(this MonoBehaviour monoBeh) where T : UnityEngine.Object
            => FindFirstObjectByTypeOrException<T>();
    }
}