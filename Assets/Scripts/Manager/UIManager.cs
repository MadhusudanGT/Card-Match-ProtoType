using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Transform editorPanel;
    private bool isEditorPanelEnabled = false;
    private void Awake()
    {
        if (editorPanel == null) { return; }
        editorPanel?.gameObject.SetActive(false);
    }
    public void ToggleEditorPanel()
    {
        if (editorPanel == null) { return; }
        isEditorPanelEnabled = !isEditorPanelEnabled;
        editorPanel.gameObject.SetActive(isEditorPanelEnabled);
    }
}
