using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUiCtrl : MonoBehaviour
{
    public Image[] healthImages;
    public Image radiationImage;
    public Image biohazardImage;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI radiationText;
    public TextMeshProUGUI biohazardText;

    public void SetHealthInUi(float m_health)
    {
        healthText.text = ((int)m_health).ToString();
        foreach (Image item in healthImages)
        {
            item.fillAmount = m_health/100f;
        }
    }

    public void SetRadiationInUi(float m_radiation)
    {
        radiationText.text = ((int)m_radiation).ToString();
        radiationImage.fillAmount = m_radiation / 100f;
    }

    public void SetBioHazardInUi(float m_biohazard)
    {
        biohazardText.text = ((int)m_biohazard).ToString();
        biohazardImage.fillAmount = m_biohazard / 100f;
    }
}
