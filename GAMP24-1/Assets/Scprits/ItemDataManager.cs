using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {NONE, FOOD, BULLET, BEHAVIOUR}
public enum Fuction { NONE, SCORE, POISON, BLESS, BULLET, LEASER, SUPERMODE, EVENT_TRIGGER}

[System.Serializable]//직렬화: 데이터를 저장하기 쉬운구조로 만든다.
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
                break;
            case Fuction.POISON:

                break;
            case Fuction.BLESS:

                break;
            case Fuction.BULLET:
                dynamic.gun.bulletType = Fuction.BULLET;
                break;
            case Fuction.LEASER:
                dynamic.gun.bulletType = Fuction.LEASER;
                break;
        }
    }
}

public class ItemDataManager : MonoBehaviour
{
    [SerializeField]
    List<ItemData> itemDatas;
    public List<ItemData> ItemDatas { get { return itemDatas; } }

    public ItemData GetItem(int idx)
    {
        return itemDatas[idx];
    }

    public ItemData GetItem(string name)
    {
        return itemDatas.Find(x => x.name == name);
    }

    public void InitData()
    {
        Debug.Log("InitData");
        itemDatas = new List<ItemData>();
        itemDatas.Clear();
        itemDatas.Add(new ItemData("Cherry",ItemType.FOOD, -1, Fuction.SCORE, 10,"사용하면 점수가 오른다."));
        itemDatas.Add(new ItemData("PoisonCherry", ItemType.FOOD, -1, Fuction.POISON, 10, "사용하면 점수가 오른다."));
        itemDatas.Add(new ItemData("GoldCherry", ItemType.FOOD, -1, Fuction.BLESS, 10, "사용하면 점수가 오른다."));
        itemDatas.Add(new ItemData("Bullet", ItemType.BULLET, -1, Fuction.BULLET, 10, "사용하면 점수가 오른다."));
        itemDatas.Add(new ItemData("Leaser", ItemType.BULLET, -1, Fuction.LEASER, 10, "사용하면 점수가 오른다."));
        itemDatas.Add(new ItemData("SuperGem", ItemType.BEHAVIOUR, -1, Fuction.SUPERMODE, 10, "사용하면 점수가 오른다."));
        itemDatas.Add(new ItemData("Trigger", ItemType.BEHAVIOUR, -1, Fuction.EVENT_TRIGGER, 10, "사용하면 점수가 오른다."));
    }
    // Start is called before the first frame update
    //다음과 같이 게임관리자에서 사용하는 콜백함수의 호출보다, 게임관리자의 스타트가 먼저 호출될 가능성 있는 경우 문제가 발생함.
    //void Start()
    //{
    //    Debug.Log(gameObject.name + "ItemDataManager.Start()");
    //    InitData();
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
