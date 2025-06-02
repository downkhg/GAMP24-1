using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
            HitEffect hitEffect = collision.GetComponent<HitEffect>();
            if (hitEffect != null) hitEffect.Hit();
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
