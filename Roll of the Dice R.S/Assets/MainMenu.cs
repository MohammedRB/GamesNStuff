using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Ran when the player presses the play button.
    public void PlayGame()
    {
        //Goes to the next scene (i.e. the actual game).
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Ran when the player presses the quit button.
    public void Quit()
    {
        /*It will output the message "You have quit the game."
        in the console and then shut down the application/ game.*/
        Debug.Log("You have quit the game.");
        Application.Quit();
    }
}