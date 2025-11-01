using Global;
using MainMenu;
using UnityEngine;

namespace Network.General
{
    public abstract class AbstractNetwork : MonoBehaviour
    {
        protected virtual bool DestroyAfterAwake => true;
        
        private void Awake()
        {
            if(StaticParameters.GameType is not GameTypes.LocalMultiplayer)
                IsNotLocalMultiplayer();
            
            if(DestroyAfterAwake)
                Destroy(this);
        }

        protected virtual void BeforeAwake()
        {
        }

        protected abstract void IsNotLocalMultiplayer();
    }
}