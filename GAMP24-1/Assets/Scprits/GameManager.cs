using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
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

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        itemDataManager.InitData();

        Iventory Iventory = responnerPlayer.objPlayer.GetComponent<Iventory>();
        Iventory.ChatItem(itemDataManager,3);

        guiItemIventory.InitItemButton(Iventory, responnerPlayer.objPlayer);

        Debug.Log(this.gameObject.name + "GameManager.Start");
        SetScene(curSceneStatus);
        foreach (var item in itemList)
        {
            ItemData getitem = itemDataManager.GetItem(item.name);
            if (getitem != null)
                item.itemData = getitem;
            else
                Debug.LogWarning($"item:{item.name} is not Find!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraTracker.objTarget == null)
        {
            cameraTracker.objTarget = responnerPlayer.objPlayer;
            cameraTracker.transform.rotation = Quaternion.identity;
        }

        if(responnerEagle.objPlayer)
        {
             Eagle eagle = responnerEagle.objPlayer.GetComponent<Eagle>();

            if (eagle != null)
            {
                eagle.trResponPoint = responnerEagle.transform;
                eagle.trPatrolPoint = responnerOpussum.transform;
            }
        }

        UpdateScene();
    }

    public List<GameObject> listGUIScene;
    public enum SceneStatus { NONE = - 1, TITLE, THEEND, GAMEOVER, PLAY, MAX}
    public SceneStatus curSceneStatus = SceneStatus.NONE;

    public GUIStatusBar guiPlayerHPBar;
    public GUIItemIventory guiItemIventory;

    public void UpdatePlayerHPBar()
    {
        if(responnerPlayer.objPlayer != null)
        {
            Player player = responnerPlayer.objPlayer.GetComponent<Player>();
            guiPlayerHPBar.UpdateStatus(player);
        }
    }

    public void EventChangeSecne(int sceneStatueIdx)
    {
        SetScene((SceneStatus)sceneStatueIdx);
    }

    public void EventTheEnd()
    {
        if(curSceneStatus == SceneStatus.PLAY)
            SetScene(SceneStatus.THEEND);
    }

    public void EventExit()
    {
        Application.Quit();
    }
    private void Awake()
    {
        
    }

    void ShowScene(SceneStatus sceneStatus)
    {
        for (int idx = 0; idx < listGUIScene.Count; idx++)
        {
            if (idx == (int)sceneStatus)
            {
                listGUIScene[idx].SetActive(true);
                Debug.Log($"ShowScene({sceneStatus})");
            }
            else
                listGUIScene[idx].SetActive(false);
        }
    }

    public void SetScene(SceneStatus sceneStatus)
    {
        Debug.Log($"SetScene({sceneStatus})");
        Time.timeScale = 0; //정지
        switch(sceneStatus)
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
                Time.timeScale = 1; //정상속도
                break;
        }
        ShowScene(sceneStatus);
        curSceneStatus = sceneStatus;
    }

    public void UpdateScene()
    {
        switch (curSceneStatus)
        {
            case SceneStatus.NONE:
                break;
            case SceneStatus.TITLE:
                break;
            case SceneStatus.THEEND:
                break;
            case SceneStatus.GAMEOVER:
                break;
            case SceneStatus.PLAY:
                UpdatePlayerHPBar();

                if (responnerPlayer.objPlayer == null)
                {
                    SetScene(SceneStatus.GAMEOVER);
                }
                break;
        }
    }
}
