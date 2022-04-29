using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MastermindsMaster : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;


    public static int permadeathVersion = 1;
    public static bool permadeath;
    private static MastermindsMaster instance;
    static bool buttonsSwapped = false;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //Ser till att det inte skapas en till kopia av MastermindsMaster när man går till main menu igen
        if (instance == null)
        {
            instance = this;

            // Med 50% sannolikhet byts platsen på knapparna för de två versionerna
            if (Random.value < 0.5f)
            {
                buttonsSwapped = true;
                SwapButtons();
            }
        }

        else
        {
            if (buttonsSwapped) SwapButtons();
            Destroy(gameObject);
        }
    }

    public void SwapButtons()
    {
        Debug.Log("Byter plats på knapparna");
        Vector3 storePosition = button2.GetComponent<RectTransform>().anchoredPosition;
        //Debug.Log(storePosition);
        button2.GetComponent<RectTransform>().anchoredPosition = button1.GetComponent<RectTransform>().anchoredPosition;
        button1.GetComponent<RectTransform>().anchoredPosition = storePosition;
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
