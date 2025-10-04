using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Transform editorPanel, popUp, splashScreen;
    [SerializeField] TMP_Text winingTxt, turnsLeft, matchedCrds;
    [SerializeField] Button replay, playBtn;

    private bool isEditorPanelEnabled = false;
    private void Awake()
    {
        if (editorPanel == null) { return; }
        editorPanel?.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        replay.onClick.AddListener(ReplayBtnClick);
        playBtn.onClick.AddListener(ReplayBtnClick);
    }
    private void OnDisable()
    {
        replay.onClick.RemoveListener(ReplayBtnClick);
        playBtn.onClick.RemoveListener(ReplayBtnClick);
    }
    public void ToggleEditorPanel()
    {
        if (editorPanel == null) { return; }
        isEditorPanelEnabled = !isEditorPanelEnabled;
        editorPanel.gameObject.SetActive(isEditorPanelEnabled);
    }

    public void GameEndStatus(bool gameStatus)
    {
        popUp.gameObject.SetActive(true);
        winingTxt.text = gameStatus ? "You Won the Game!" : "You Lost the Game!";
    }

    public void UpdateTurnLeftTxt(int turnLeft)
    {
        turnsLeft.text = "Turns Left : " + turnLeft.ToString();
    }

    public void MatchedItems(int matchedItems)
    {
        matchedCrds.text = "Matched Items : " + matchedItems.ToString();
    }
    void ReplayBtnClick()
    {
        Debug.Log("Replay BTN CLICKED");
        GameManager.Instance?.Replay();
        splashScreen.gameObject.SetActive(false);
    }
}
