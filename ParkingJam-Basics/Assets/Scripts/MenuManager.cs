using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject PanelSetting;
    private Animator settingPanelAnimator;
    [SerializeField] TMP_Text levelText;

    public static bool InPutEnableBool = true;
    void Start()
    {
        settingPanelAnimator = PanelSetting.GetComponent<Animator>();
        PanelSetting.SetActive(false);

        levelText.text = $"Level {SceneManager.GetActiveScene().buildIndex + 1}";
    }

    public void OpenSettingPanel()
    {
        InPutEnableBool = false;
        PanelSetting.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        settingPanelAnimator.SetTrigger("CloseTrig");
        Invoke("CloseSettingPanelFinal", 0.5f);
    }
    private void CloseSettingPanelFinal()
    {
        PanelSetting.SetActive(false);
        InPutEnableBool = true;
    }
}
