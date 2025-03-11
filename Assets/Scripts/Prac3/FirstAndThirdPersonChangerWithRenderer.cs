using UnityEngine;

namespace Practice3
{
    public class FirstAndThirdPersonChangerWithRenderer : MonoBehaviour
    {
        public Transform camera;
        public SkinnedMeshRenderer renderer;

        bool isThirdPerson;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                isThirdPerson = !isThirdPerson;
                if (isThirdPerson)
                {
                    camera.transform.localPosition = new Vector3(0, 0, -10f);
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                else
                {
                    camera.transform.localPosition = Vector3.zero;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
        }
    }
}
