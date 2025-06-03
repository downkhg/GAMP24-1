using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Player playerMaster;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }

    //�Ѿ��� �����Ǹ鼭 ������Ʈ �Ҹ�� �۵���������
    //IEnumerator HitTimmer(SpriteRenderer spriteRenderer, float time = 0.1f)
    //{
    //    spriteRenderer.color = Color.red;
    //    yield return new WaitForSeconds(time);
    //    spriteRenderer.color = Color.white;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            Player playerTarget = collision.gameObject.GetComponent<Player>();

            if (playerTarget && playerMaster)
            {
                HitEffect hitEffet = collision.gameObject.GetComponent<HitEffect>();
                if (hitEffet != null)
                {
                    if (!hitEffet.isHit)
                    {
                        playerMaster.Attack(playerTarget);
                        hitEffet.Hit();
                        Debug.Log("Attack!");
                        if (playerTarget.Death())
                        {
                            Destroy(collision.gameObject);
                            playerMaster.StillExp(playerTarget);
                        }
                    }
                }
                Destroy(this.gameObject);
            }
        }
    }
}
