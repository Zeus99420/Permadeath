using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MastermindsMaster : MonoBehaviour
{
    public static int permadeathVersion = 1;
    public static bool permadeath;
    private static MastermindsMaster instance;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //Ser till att det inte skapas en till kopia av MastermindsMaster när man går till main menu igen
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public static void PlayVersion (int version)
    {
        if (version == permadeathVersion) permadeath = true;
        else permadeath = false;

        SceneManager.LoadScene("Gameplay");
    }

    public static void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }




}
