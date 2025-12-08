using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalVariableStorage : MonoBehaviour
{
    public static int EnemiesToKill;
    public static int EnemiesDead;

    public int ToKillValue;
    public int DeadValue;
    public int Level;

    void Awake()
    {
        GlobalVariableStorage.EnemiesToKill = ToKillValue;
        GlobalVariableStorage.EnemiesDead = DeadValue;
    }

    void Update()
    {
        if(GlobalVariableStorage.EnemiesDead == GlobalVariableStorage.EnemiesToKill){
            if(Level == 1){
                PlayerPrefs.SetInt("CurrentLevel", 2);
                SceneManager.LoadScene("Level2");
            } else if(Level == 2){
                PlayerPrefs.SetInt("CurrentLevel", 3);
                SceneManager.LoadScene("Level3");
            } else if(Level == 3){
                PlayerPrefs.SetInt("CurrentLevel", 4);
                SceneManager.LoadScene("Level4");
            } else if(Level == 4){
                PlayerPrefs.SetInt("CurrentLevel", 5);
                SceneManager.LoadScene("Level5");
            } else if(Level == 5){
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("VictoryScene");
            }
        }
    } 
}
