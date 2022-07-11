using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeController : MonoBehaviour
{
    [SerializeField]
    int mScore;

    [SerializeField]
    TMPro.TextMeshProUGUI mScoreTextShadow, mScoreText;

    void Start()
    {
        CharacterMovementController.OnPointScored += OnPointScored;
    }

    void OnPointScored()
    {
        mScore++;
        mScoreTextShadow.text = mScore + "/10";
        mScoreText.text = mScore + "/10";

        if(mScore == 10)
        SceneManager.LoadScene(0);

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
