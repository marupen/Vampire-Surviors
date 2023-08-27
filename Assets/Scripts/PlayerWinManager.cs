using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinManager : MonoBehaviour
{
    [SerializeField] GameObject winMessagePanel;
    PauseManager pauseManager;
    [SerializeField] DataConteiner dataConteiner;

    private void Awake()
    {
        pauseManager = GetComponent<PauseManager>();
    }

    public void Win()
    {
        winMessagePanel.SetActive(true);
        pauseManager.PauseGame();
        dataConteiner.StageComplete(0);
    }
}
