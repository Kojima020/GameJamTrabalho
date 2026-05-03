using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    [SerializeField] private bool isThirdPerson = true;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform thirdPerson;
    [SerializeField] private Transform firstPerson;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) ChangeCamera();
    }

    private void ChangeCamera()
    {
        isThirdPerson = !isThirdPerson;
        playerCamera.position = isThirdPerson ? thirdPerson.position : firstPerson.position;
        playerCamera.rotation = isThirdPerson ? thirdPerson.rotation : firstPerson.rotation;
    }
}
