using UnityEngine;

public class FirstThirdPersonChanger : MonoBehaviour
{
    public Transform camera;

    bool isThirdPerson;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isThirdPerson = !isThirdPerson;
            if (isThirdPerson)
            {
                camera.transform.localPosition = new Vector3(0, 0, -6f);
            }
            else
            {
                camera.transform.localPosition = Vector3.zero;
            }
        }
    }
}
