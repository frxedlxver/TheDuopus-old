using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StaminaSliderController : MonoBehaviour
{

    public Sucker Sucker;
    public Slider Slider;
    private Image sliderFill;
    private Image[] sliderImages;

    [Range(0f, 1f)]
    public float LowStaminaPoint = 0.3f;

    public Color NormalFillColor = Color.green;
    public Color LowStaminaFillColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        sliderFill = Slider.fillRect.GetComponent<Image>();
        sliderImages = Slider.GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Sucker == null)
        {
            Debug.LogWarning("Sucker not Set");
            return;
        }


        Slider.transform.position = Sucker.transform.position;

        Slider.value = Sucker.CurStamina / Sucker.MaxStamina;

        if (Slider.value < LowStaminaPoint)
        {
            sliderFill.color = LowStaminaFillColor;
        }
        else
        {
            sliderFill.color = NormalFillColor;
        }

        SetSliderVisibility(Slider.value != 1);
    }

    private void SetSliderVisibility(bool enabled)
    {
        foreach(Image image in sliderImages)
        {
            image.enabled = enabled;
        }
    }
}
