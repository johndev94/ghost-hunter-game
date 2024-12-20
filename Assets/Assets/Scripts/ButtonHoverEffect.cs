using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour
{
    public TextMeshProUGUI buttonText; // Ensure this is TextMeshProUGUI

    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f);


    public void OnPointerEnter()
    {
        buttonText.transform.localScale = hoverScale;
    }

    public void OnPointerExit()
    {
        buttonText.transform.localScale = normalScale;
    }
}
