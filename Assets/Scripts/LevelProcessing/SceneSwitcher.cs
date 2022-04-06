using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public static SceneSwitcher instance;

    public static int currentScene;
    void Awake() {
        if (instance == null)
            instance = this;
    }

    public void A_ExitButton() {
        Application.Quit();
    }

    public void A_LoadScene(int i) {
        currentScene = i;
        SceneManager.LoadScene(i);
    }

    public void A_LoadCurrentScene() {
        SceneManager.LoadScene(currentScene);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && (currentScene < 3 || currentScene > 7))
            A_ExitButton();
    }
}
