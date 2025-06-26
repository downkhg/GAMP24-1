// GameManager.cs
using System.Collections;
using System.Collections.Generic;
// using UnityEditor;
// using UnityEditor.Experimental.GraphView;
// using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public CameraTracker cameraTracker;
    public Responner responnerPlayer;
    public Responner responnerEagle;
    public Responner responnerOpussum;

    public ItemDataManager itemDataManager;
    public List<Item> itemList;

    public static GameManager instance;

    // GUIManager 참조로 변경
    public GUIManager guiManager;

    public enum SceneStatus { NONE = -1, TITLE, THEEND, GAMEOVER, PLAY, MAX }
    public SceneStatus curSceneStatus;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //itemDataManager.LoadCSVData("ItemData"); // Resources 폴더의 "ItemData.csv" 파일에서 로드
        itemDataManager.LoadJsonDataFromResources("ItemData");// Resources 폴더의 "ItemData.json" TextAsset에서 로드

        //itemDataManager.InitData(); // 하드코딩된 데이터로 초기화
        //itemDataManager.SaveItems("MySaveGame.json");   // persistentDataPath에 "MySaveGame.json" 파일로 저장
        //itemDataManager.LoadJsonData("MySaveGame.json");  // persistentDataPath의 "MySaveGame.json" 파일에서 로드

        Iventory playerInventory = responnerPlayer.objPlayer.GetComponent<Iventory>();
        if (playerInventory == null)
        {
            Debug.LogError("Player's Inventory component not found!");
            return;
        }

        playerInventory.ChatItem(itemDataManager, 1);

        // GUIManager 초기화
        if (guiManager == null)
        {
            guiManager = FindObjectOfType<GUIManager>();
            if (guiManager == null)
            {
                Debug.LogError("GUIManager not found in the scene! Please add one.");
                return;
            }
        }
        guiManager.InitializeUI(playerInventory, responnerPlayer.objPlayer);

        Debug.Log(this.gameObject.name + " GameManager.Start");
        SetScene(curSceneStatus);

        foreach (var item in itemList)
        {
            if (item != null)
            {
                ItemData getitem = itemDataManager.GetItem(item.name);
                if (getitem != null)
                    item.itemData = getitem;
                else
                    Debug.LogWarning($"item:{item.name} is not Find!");
            }
        }
    }

    void Update()
    {
        if (cameraTracker != null && responnerPlayer.objPlayer != null)
        {
            cameraTracker.objTarget = responnerPlayer.objPlayer;
        }

        UpdateScene();
    }

    public void SetScene(SceneStatus sceneStatus)
    {
        Debug.Log($"GameManager: SetScene({sceneStatus})");
        Time.timeScale = 0;

        switch (sceneStatus)
        {
            case SceneStatus.NONE:
                break;
            case SceneStatus.TITLE:
                break;
            case SceneStatus.THEEND:
                break;
            case SceneStatus.GAMEOVER:
                SceneManager.LoadScene(0);
                break;
            case SceneStatus.PLAY:
                Time.timeScale = 1;
                break;
        }

        // GUIManager를 통해 UI 활성화/비활성화 지시
        if (guiManager != null)
        {
            guiManager.ShowSceneUI(sceneStatus);
        }
        else
        {
            Debug.LogError("GUIManager is not assigned! Cannot show scene UI.");
        }

        curSceneStatus = sceneStatus;
    }

    public void UpdateScene()
    {
        switch (curSceneStatus)
        {
            case SceneStatus.NONE:
            case SceneStatus.TITLE:
            case SceneStatus.THEEND:
            case SceneStatus.GAMEOVER:
                break;
            case SceneStatus.PLAY:
                Player player = responnerPlayer.objPlayer.GetComponent<Player>();
                if (player != null && guiManager != null)
                {
                    guiManager.UpdatePlayerHPBar(player);
                }

                if (Input.GetKeyDown(KeyCode.I))
                {
                    if (guiManager != null)
                    {
                        guiManager.ToggleInventoryPopup();
                    }
                }

                if (responnerPlayer.objPlayer == null)
                {
                    SetScene(SceneStatus.GAMEOVER);
                }
                break;
        }
    }

    public void EventChangeSecne(int sceneStatueIdx)
    {
        SetScene((SceneStatus)sceneStatueIdx);
    }

    public void EventTheEnd()
    {
        SetScene(SceneStatus.THEEND);
    }

    public void EventExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}