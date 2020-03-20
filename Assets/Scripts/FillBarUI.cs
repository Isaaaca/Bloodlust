using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBarUI : MonoBehaviour
{

    [SerializeField] Image bar;
    [SerializeField] Meter meter;

    private void Update()
    {
        bar.fillAmount = meter.GetNormalised();
    }
}
