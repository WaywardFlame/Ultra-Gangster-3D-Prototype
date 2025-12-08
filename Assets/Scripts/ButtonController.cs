using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void GoAgainButton(){
        PlayerPrefs.SetInt("CurrentLevel", 1);
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
    }

    public void ReturnToMainMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void NewGameButton(){
        PlayerPrefs.SetInt("CurrentLevel", 1);
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
    }

    public void LoadGameButton(){
        Time.timeScale = 1;
        int CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        if(CurrentLevel == 1){
            SceneManager.LoadScene("Level1");
        } else if(CurrentLevel == 2){
            SceneManager.LoadScene("Level2");
        } else if(CurrentLevel == 3){
            SceneManager.LoadScene("Level3");
        } else if(CurrentLevel == 4){
            SceneManager.LoadScene("Level4");
        } else if(CurrentLevel == 5){
            SceneManager.LoadScene("Level5");
        }
    }

    public void SettingsButton(){
        SceneManager.LoadScene("SettingsScene");
    }

    public void QuitGameButton(){
        Application.Quit();
    }
}
