using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Eagle : MonoBehaviour
{
    public float speed;
    public bool isMove;
    public float site;

    public GameObject objTarget;

    public Transform trResponPoint;
    public bool isRetrun;

    void Return()
    {
        if (!isRetrun) return;

        if (objTarget == null)
        {
            if (trResponPoint)
                objTarget = trResponPoint.gameObject;
        }

        if (!isMove)
            isPatrol = true;
    }

    public Transform trPatrolPoint;
    public bool isPatrol;

    void Patrol()
    {
        if (!isPatrol) return;

        if (trPatrolPoint)
        {
            if (!isMove)
            {
                if(objTarget.name == trResponPoint.gameObject.name)
                    objTarget = trPatrolPoint.gameObject;
                else if (objTarget.name == trPatrolPoint.gameObject.name)
                    objTarget = trResponPoint.gameObject;
            }
        }
    }

    public bool isFind = true;

    void Find()
    {
        if (!isFind) return;

        Collider2D[] collisions = Physics2D.OverlapCircleAll(this.transform.position, site);

        foreach (Collider2D collision in collisions)
        {
            Debug.Log($"OverlapCircle({collision.gameObject.name})");

            if (collision.tag == "Player")
            {
                objTarget = collision.gameObject;
                isPatrol = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isMove = Move(objTarget);
        Patrol();
        Return(); 
    }

    private void FixedUpdate()
    {
        Find();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, site);
    }

    bool Move(GameObject target)
    {
        if (target == null) return false;
        Vector3 vTargetPos = target.transform.position;
        Vector3 vPos = this.transform.position;

        Vector3 vDist = vTargetPos - vPos;
        Vector3 vDir = vDist.normalized;

        float fDist = vDist.magnitude;
        float fMove = speed * Time.deltaTime;

        if (fDist > fMove)
        {
            transform.position += vDir * speed * Time.deltaTime;
            Debug.DrawLine(vTargetPos, vPos, Color.green);
            return true;
        }
        else
        {
            Debug.DrawLine(vTargetPos, vPos, Color.red);
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            objTarget = collision.gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{gameObject.name}.OnCollisionEnter2D({collision.gameObject}/{collision.gameObject.tag})");
        if (collision.gameObject.tag == "Player")
            Destroy(collision.gameObject);
    }
}
