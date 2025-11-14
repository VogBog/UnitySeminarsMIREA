using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public interface IEditablePool
    {
        Dictionary<Type, Stack<Component>> GetPoolObjects();
        Dictionary<Type, List<Component>> GetSpawnedObjects();
    }
}