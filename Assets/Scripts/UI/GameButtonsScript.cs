using UnityEngine;
using YG;

public class GameButtonsScript : MonoBehaviour
{
    [SerializeField] GameObject[] ToShow;
    [SerializeField] GameObject[] ToHide;

    void Awake(){
        if (SystemInfo.deviceType == DeviceType.Handheld || YG2.envir.isMobile || YG2.envir.isTablet){
            foreach(GameObject i in ToShow)
            {
                i.SetActive(true);
            }
            foreach(GameObject i in ToHide)
            {
                i.SetActive(false);
            }
        }
        else{
            foreach(GameObject i in ToShow)
            {
                i.SetActive(false);
            }
            foreach(GameObject i in ToHide)
            {
                i.SetActive(true);
            }
        }
    }
}
