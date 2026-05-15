using TMPro;
using UnityEngine;

public class GuestNameFix : MonoBehaviour
{
    public TMP_Text nameText;

    private void LateUpdate()
    {
        if (string.IsNullOrEmpty(nameText.text))
        {
            nameText.text = "Гость";
        }
    }
}