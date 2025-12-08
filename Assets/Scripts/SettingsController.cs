using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public TextMeshProUGUI vsync;
    public TextMeshProUGUI fullscreen;
    public TMP_InputField fps;
    public TMP_InputField sensitivity;
    public TMP_InputField volume;
    

    void Start()
    {
        if(PlayerPrefs.GetInt("VSYNC") == 1){
            vsync.text = "VSYNC: ON";
        }
        if(PlayerPrefs.GetInt("Fullscreen") == 1){
            fullscreen.text = "Fullscreen: ON";
        }
        fps.text = "" + PlayerPrefs.GetInt("CustomFPS");
        sensitivity.text = "" + PlayerPrefs.GetInt("Sensitivity");
        volume.text = "" + PlayerPrefs.GetInt("Volume");
    }

    public void VSYNCToggle(){
        if(PlayerPrefs.GetInt("VSYNC", 1) == 1){
            PlayerPrefs.SetInt("VSYNC", 0);
            vsync.text = "VSYNC: OFF";
            QualitySettings.vSyncCount = 0;
        } else {
            PlayerPrefs.SetInt("VSYNC", 1);
            vsync.text = "VSYNC: ON";
            QualitySettings.vSyncCount = 1;
        }
    }

    public void FullscreenToggle(){
        // int resWidth = PlayerPrefs.GetInt("ResWidth");
        // int resHeight = PlayerPrefs.GetInt("ResHeight");
        bool fullscreenBool = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;
        //Screen.SetResolution(resWidth, resHeight, !fullscreenBool);
        Screen.fullScreen = !Screen.fullScreen;
        if(fullscreenBool){
            fullscreen.text = "Fullscreen: OFF";
            PlayerPrefs.SetInt("Fullscreen", 0);
        } else {
            fullscreen.text = "Fullscreen: ON";
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
    }

    public void Res480(){
        bool fullscreenBool = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;
        Screen.SetResolution(854, 480, fullscreenBool);
    }

    public void Res720(){
        bool fullscreenBool = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;
        Screen.SetResolution(1280, 720, fullscreenBool);
    }

    public void Res1080(){
        bool fullscreenBool = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;
        Screen.SetResolution(1920, 1080, fullscreenBool);
    }

    public void Res1440(){
        bool fullscreenBool = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;
        Screen.SetResolution(2560, 1440, fullscreenBool);
    }

    public void Res2160(){
        bool fullscreenBool = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;
        Screen.SetResolution(3840, 2160, fullscreenBool);
    }

    public void ApplyFPS(){
        int num;
        bool success = int.TryParse(fps.text, out num);
        if(success && num > 0){
            PlayerPrefs.SetInt("CustomFPS", num);
            Application.targetFrameRate = num;
        } else {
            fps.text = "invalid";
        }
    }

    public void ApplySensitivity(){
        int num;
        bool success = int.TryParse(sensitivity.text, out num);
        if(success && num > 0){
            PlayerPrefs.SetInt("Sensitivity", num);
        } else {
            fps.text= "invalid";
        }
    }

    public void ApplyVolume(){
        int num;
        bool success = int.TryParse(volume.text, out num);
        if(success && num >= 0 && num <= 100){
            PlayerPrefs.SetInt("Volume", num);
        } else {
            volume.text = "invalid";
        }
    }
}
