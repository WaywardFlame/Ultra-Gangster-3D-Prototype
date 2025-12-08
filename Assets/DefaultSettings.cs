using UnityEngine;

public class DefaultSettings : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSYNC", 1);
        Application.targetFrameRate = PlayerPrefs.GetInt("CustomFPS", 60);
        PlayerPrefs.SetInt("Sensitivity", PlayerPrefs.GetInt("Sensitivity", 100));

        int resWidth = PlayerPrefs.GetInt("ResWidth", 1920);
        int resHeight = PlayerPrefs.GetInt("ResHeight", 1080);
        bool fullscreen = (PlayerPrefs.GetInt("Fullscreen", 1) == 1) ? true : false;
        Screen.SetResolution(resWidth, resHeight, fullscreen);

        PlayerPrefs.SetInt("Volume", PlayerPrefs.GetInt("Volume", 100)); 
    }
}
