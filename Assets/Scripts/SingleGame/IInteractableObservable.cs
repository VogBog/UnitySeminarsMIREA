using System;
using Game;
using UnityEngine;

namespace SingleGame
{
    public interface IInteractableObservable : IInteractable
    {
        event Action<IInteractableObservable, GameObject> Interacted;
    }
}