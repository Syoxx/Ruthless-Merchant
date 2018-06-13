using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.SceneManagement; 
 
public class MainMenu : MonoBehaviour 
{ 
 
    public void PlayButton() 
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    } 
 
    public void QuitButton() 
    { 
        Debug.Log("Quit!"); 
        Application.Quit(); 
    } 
 
    public void LoadGame() 
    { 
        throw new System.NotImplementedException(); 
    } 
 
 
} 