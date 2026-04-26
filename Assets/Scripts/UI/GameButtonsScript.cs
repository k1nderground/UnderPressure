using UnityEngine;

public class GameButtonsScript : MonoBehaviour
{
    [SerializeField] GameObject LeftButton;
    [SerializeField] GameObject RightButton;

    [SerializeField] GameObject JumpButton;
    [SerializeField] GameObject GoButton;

    void Awake(){
        if (SystemInfo.deviceType == DeviceType.Handheld){
            LeftButton.SetActive(true);
            RightButton.SetActive(true);
            JumpButton.SetActive(true);
            GoButton.SetActive(true);
        }
        else{
            LeftButton.SetActive(false);
            RightButton.SetActive(false);
            JumpButton.SetActive(false);
            GoButton.SetActive(false);
        }
    }
}
