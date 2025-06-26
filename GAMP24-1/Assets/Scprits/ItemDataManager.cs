// ItemDataManager.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum ItemType { NONE, FOOD, BULLET, BEHAVIOUR }
public enum Fuction { NONE, SCORE, POISON, BLESS, BULLET, LEASER, SUPERMODE, EVENT_TRIGGER }

[System.Serializable]
public class ItemData
{
    public string name;
    public ItemType type;
    public float time;
    public Fuction fuction;
    public int Score;
    public string Comment;

    public ItemData(string name, ItemType type, float time, Fuction fuction, int score, string comment)
    {
        this.name = name;
        this.type = type;
        this.time = time;
        this.fuction = fuction;
        Score = score;
        Comment = comment;
    }

    public void Use(GameObject obj)
    {
        if (obj == null) return;
        Dynamic dynamic = obj.GetComponent<Dynamic>();
        if (dynamic == null) return;
        switch (fuction)
        {
            case Fuction.SCORE:
                dynamic.score += Score;
                Debug.Log($"Score increased: {dynamic.score}");
                break;
            case Fuction.POISON:
                Debug.Log("Used PoisonCherry!");
                // 독 관련 로직 추가 (예: 체력 감소, 상태 이상)
                break;
            case Fuction.BLESS:
                Debug.Log("Used GoldCherry!");
                // 축복 관련 로직 추가 (예: 일시적 버프)
                break;
            case Fuction.BULLET:
                dynamic.gun.bulletType = Fuction.BULLET;
                Debug.Log("Changed bullet type to Bullet.");
                break;
            case Fuction.LEASER:
                dynamic.gun.bulletType = Fuction.LEASER;
                Debug.Log("Changed bullet type to Laser.");
                break;
            case Fuction.SUPERMODE:
                SuperMode superMode = obj.GetComponent<SuperMode>();
                if (superMode != null)
                {
                    superMode.Use();
                    Debug.Log("Activated SuperMode!");
                }
                break;
            case Fuction.EVENT_TRIGGER:
                Debug.Log("Event Triggered!");
                // 특정 게임 이벤트 발생 로직 추가 (예: 퀘스트 진행, 컷씬)
                break;
            case Fuction.NONE:
            default:
                Debug.LogWarning($"Item {name} has no defined use function.");
                break;
        }
    }
}

// List<ItemData>를 직렬화하기 위한 래퍼 클래스
[System.Serializable]
public class ItemDataListWrapper
{
    public List<ItemData> items;

    public ItemDataListWrapper()
    {
        items = new List<ItemData>();
    }
}

public class ItemDataManager : MonoBehaviour
{
    private List<ItemData> itemDatas = new List<ItemData>();
    // 파일 이름 관련 멤버 변수들은 제거되었습니다.

    public List<ItemData> ItemDatas { get { return itemDatas; } }

    public ItemData GetItem(int idx)
    {
        if (idx >= 0 && idx < itemDatas.Count)
        {
            return itemDatas[idx];
        }
        Debug.LogWarning($"GetItem: Index {idx} out of range.");
        return null;
    }

    public ItemData GetItem(string name)
    {
        return itemDatas.Find(x => x.name == name);
    }

    // 임시 테스트용: 하드코딩된 데이터로 초기화
    public void InitData()
    {
        Debug.Log("ItemDataManager: Initializing data with hardcoded values (temporary for testing).");
        itemDatas = new List<ItemData>();
        itemDatas.Add(new ItemData("Cherry", ItemType.FOOD, -1, Fuction.SCORE, 10, "사용하면 점수가 오른다."));
        itemDatas.Add(new ItemData("PoisonCherry", ItemType.FOOD, -1, Fuction.POISON, 10, "사용하면 점수가 오른다."));
        itemDatas.Add(new ItemData("GoldCherry", ItemType.FOOD, -1, Fuction.BLESS, 10, "사용하면 점수가 오른다."));
        itemDatas.Add(new ItemData("Bullet", ItemType.BULLET, -1, Fuction.BULLET, 10, "총알을 발사한다."));
        itemDatas.Add(new ItemData("Leaser", ItemType.BULLET, -1, Fuction.LEASER, 10, "레이저를 발사한다."));
        itemDatas.Add(new ItemData("SuperGem", ItemType.BEHAVIOUR, -1, Fuction.SUPERMODE, 10, "일정시간 무적 상태가 된다."));
        itemDatas.Add(new ItemData("EventTriggerItem", ItemType.BEHAVIOUR, -1, Fuction.EVENT_TRIGGER, 0, "특정 이벤트를 발생시킨다."));
        Debug.Log($"ItemDataManager: Hardcoded {itemDatas.Count} items loaded.");
    }

    // CSV 파일에서 데이터를 로드하는 함수 (Resources 폴더)
    // csvFileName: Resources 폴더 내의 CSV 파일 이름 (확장자 제외)
    public void LoadCSVData(string csvFileName)
    {
        Debug.Log($"ItemDataManager: Loading data from CSV file '{csvFileName}.csv' in Resources.");
        itemDatas.Clear();

        TextAsset csvFile = Resources.Load<TextAsset>(csvFileName);

        if (csvFile == null)
        {
            Debug.LogError($"ItemDataManager: CSV file '{csvFileName}.csv' not found in Resources folder. Please ensure it exists and is named correctly.");
            return;
        }

        string[] lines = csvFile.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length <= 1)
        {
            Debug.LogWarning("ItemDataManager: CSV file is empty or only contains header.");
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] tokens = line.Split(',');

            if (tokens.Length != 6)
            {
                Debug.LogWarning($"ItemDataManager: Skipping malformed line in CSV: {line}. Expected 6 columns, got {tokens.Length}.");
                continue;
            }

            try
            {
                string name = tokens[0].Trim();
                ItemType type = (ItemType)Enum.Parse(typeof(ItemType), tokens[1].Trim());
                float time = float.Parse(tokens[2].Trim());
                Fuction fuction = (Fuction)Enum.Parse(typeof(Fuction), tokens[3].Trim());
                int score = int.Parse(tokens[4].Trim());
                string comment = tokens[5].Trim();

                itemDatas.Add(new ItemData(name, type, time, fuction, score, comment));
            }
            catch (Exception e)
            {
                Debug.LogError($"ItemDataManager: Error parsing line '{line}': {e.Message}");
            }
        }
        Debug.Log($"ItemDataManager: Loaded {itemDatas.Count} items from CSV.");
    }

    // 저장 파일 경로를 반환하는 헬퍼 함수 (persistentDataPath용)
    // fileName: 저장할 파일 이름 (예: "ItemDataSave.json")
    private string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    // 현재 아이템 데이터를 JSON 파일로 저장하는 함수 (persistentDataPath)
    // jsonFileName: 저장할 JSON 파일 이름 (예: "ItemDataSave.json")
    public void SaveItems(string jsonFileName)
    {
        ItemDataListWrapper wrapper = new ItemDataListWrapper();
        wrapper.items = itemDatas;

        string json = JsonUtility.ToJson(wrapper, true);

        string path = GetSavePath(jsonFileName);
        try
        {
            File.WriteAllText(path, json);
            Debug.Log($"ItemDataManager: Items saved to {path}");
        }
        catch (Exception e)
        {
            Debug.LogError($"ItemDataManager: Failed to save items to {path}: {e.Message}");
        }
    }

    // JSON 파일에서 아이템 데이터를 불러오는 함수 (persistentDataPath)
    // jsonFileName: 불러올 JSON 파일 이름 (예: "ItemDataSave.json")
    public void LoadJsonData(string jsonFileName)
    {
        Debug.Log($"ItemDataManager: Attempting to load data from JSON file '{jsonFileName}' in persistentDataPath.");
        string path = GetSavePath(jsonFileName);

        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                ItemDataListWrapper wrapper = JsonUtility.FromJson<ItemDataListWrapper>(json);
                if (wrapper != null && wrapper.items != null)
                {
                    itemDatas = wrapper.items;
                    Debug.Log($"ItemDataManager: Loaded {itemDatas.Count} items from {path}");
                }
                else
                {
                    Debug.LogWarning($"ItemDataManager: JSON file at {path} was empty or malformed. No items loaded.");
                    itemDatas.Clear();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"ItemDataManager: Failed to load items from {path}: {e.Message}");
                itemDatas.Clear();
            }
        }
        else
        {
            Debug.LogWarning($"ItemDataManager: JSON save file not found at {path}.");
            itemDatas.Clear();
        }
    }

    // Resources 폴더에서 JSON 파일을 TextAsset으로 읽어 아이템 데이터를 불러오는 함수
    // jsonFileName: Resources 폴더 내의 JSON 파일 이름 (확장자 제외)
    public void LoadJsonDataFromResources(string jsonFileName)
    {
        Debug.Log($"ItemDataManager: Attempting to load data from JSON TextAsset '{jsonFileName}' in Resources.");
        itemDatas.Clear();

        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);

        if (jsonFile == null)
        {
            Debug.LogError($"ItemDataManager: JSON file '{jsonFileName}.json' (or .txt) not found in Resources folder. Please ensure it exists and is named correctly.");
            return;
        }

        try
        {
            string json = jsonFile.text;
            ItemDataListWrapper wrapper = JsonUtility.FromJson<ItemDataListWrapper>(json);
            if (wrapper != null && wrapper.items != null)
            {
                itemDatas = wrapper.items;
                Debug.Log($"ItemDataManager: Loaded {itemDatas.Count} items from Resources/{jsonFileName}.json");
            }
            else
            {
                Debug.LogWarning($"ItemDataManager: JSON TextAsset '{jsonFileName}' was empty or malformed. No items loaded.");
                itemDatas.Clear();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"ItemDataManager: Failed to parse JSON from TextAsset '{jsonFileName}': {e.Message}");
            itemDatas.Clear();
        }
    }
}