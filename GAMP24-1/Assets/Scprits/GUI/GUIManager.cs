// GUIManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance { get; private set; }

    public GUIStatusBar guiPlayerHPBar;
    public GUIItemIventory guiItemIventory;
    public GameObject objPopup;

    public List<GameObject> listGUIScene;

    private Iventory playerInventory;
    private GameObject playerObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeUI(Iventory inventory, GameObject playerObj)
    {
        playerInventory = inventory;
        playerObject = playerObj;

        if (objPopup != null)
        {
            objPopup.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GUIManager: objPopup is not assigned!");
        }
    }

    public void UpdatePlayerHPBar(Player player)
    {
        if (guiPlayerHPBar != null)
        {
            guiPlayerHPBar.UpdateStatus(player);
        }
        else
        {
            Debug.LogWarning("GUIManager: guiPlayerHPBar is not assigned!");
        }
    }

    public void ToggleInventoryPopup()
    {
        if (objPopup == null || guiItemIventory == null)
        {
            Debug.LogWarning("GUIManager: Inventory UI components are not assigned!");
            return;
        }

        bool isActive = !objPopup.activeSelf;
        objPopup.SetActive(isActive);

        if (isActive)
        {
            if (playerInventory != null && playerObject != null)
            {
                guiItemIventory.ClearButton();
                guiItemIventory.InitItemButton(playerInventory, playerObject);
            }
            else
            {
                Debug.LogError("GUIManager: Player inventory or object not initialized for UI!");
            }
        }
    }

    public void ShowSceneUI(GameManager.SceneStatus sceneStatus)
    {
        for (int idx = 0; idx < (int)GameManager.SceneStatus.MAX; idx++)
        {
            if (listGUIScene.Count > idx && listGUIScene[idx] != null)
            {
                if (idx == (int)sceneStatus)
                {
                    listGUIScene[idx].SetActive(true);
                    Debug.Log($"ShowSceneUI:{sceneStatus}");
                }
                else
                    listGUIScene[idx].SetActive(false);
            }
        }
    }

    public void SetGameScene(GameManager.SceneStatus sceneStatus)
    {
        Debug.Log($"GUIManager: SetGameScene({sceneStatus})");
        ShowSceneUI(sceneStatus);
    }
}