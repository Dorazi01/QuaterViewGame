using System.Collections;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int maxHealth;
    public int curHealth;
    public Transform target;
    public bool isChase;
    bool isHit = false; //���� �¾Ҵ��� Ȯ���ϴ� ����
    

    Material mat; //Material ������Ʈ
    Rigidbody rigid;
    BoxCollider boxColl;
    NavMeshAgent nav;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //Rigidbody ������Ʈ�� ������
        boxColl = GetComponent<BoxCollider>(); //BoxCollider ������Ʈ�� ������
        mat = GetComponentInChildren<MeshRenderer>().material; //Renderer ������Ʈ�� Material�� ������
        nav = GetComponent<NavMeshAgent>(); //NavMeshAgent ������Ʈ�� ������
        anim = GetComponentInChildren<Animator>(); //Animator ������Ʈ�� ������

        Invoke("ChaseStart", 2); 
    }



    // Update is called once per frame
    void Update()
    {
        if (isChase)
        {
            nav.SetDestination(target.position); //Ÿ���� ��ġ�� �̵�
        }
        
    }


    void ChaseStart()
    {
        isChase = true; //���� ����
        anim.SetBool("isWalk", true); //�ִϸ��̼� ����
    }





    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.linearVelocity = Vector3.zero; //ȸ�� �ӵ��� 0���� ������
            rigid.angularVelocity = Vector3.zero; //ȸ�� �ӵ��� 0���� ������
        }
        
    }

    private void FixedUpdate()
    {
        FreezeVelocity(); //�ӵ� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee"))
        {
            if (isHit) //���� ���� �ʾ��� ��
            {
                return;
            }
            isHit = true; //���� �¾���
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage; //���⿡�� �������� ������
            Vector3 reactVec = transform.position - other.transform.position; //�浹�� ������Ʈ�� ��ġ�� ������
            Invoke("IsHitFalse", 0.4f);
            StartCoroutine(OnDamage(reactVec, false)); //������ �ڷ�ƾ ����
            
        }
        else if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage; //���⿡�� �������� ������
            Vector3 reactVec = transform.position- other.transform.position; //�浹�� ������Ʈ�� ��ġ�� ������
            Destroy(other.gameObject); //�Ѿ� ����

            StartCoroutine(OnDamage(reactVec,false)); //������ �ڷ�ƾ ����
        }
    }

    void IsHitFalse()
    {
        isHit = false; //���� ���� �ʾ���
    }

    public void HitByNade(Vector3 explosionPos)
    {
        curHealth -= 10; //����ź ������
        Vector3 reactVec = transform.position - explosionPos; //�浹�� ������Ʈ�� ��ġ�� ������
        StartCoroutine(OnDamage(reactVec,true)); //������ �ڷ�ƾ ����
    }


    IEnumerator OnDamage(Vector3 reactVec, bool isNade)
    {
        mat.color = Color.red; //���� ����

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            mat.color = Color.white; //���� ����
        }
        else
        {
            mat.color = Color.gray; //���� ����
            gameObject.layer = 11; //�� ���� ���̾�� ����
            isChase = false; //���� ����
            nav.enabled = false; //NavMeshAgent ��Ȱ��ȭ
            anim.SetTrigger("doDie"); //���� �ִϸ��̼� ����


            if (isNade) //����ź�� �¾��� ��
            {
                reactVec = reactVec.normalized; //����ȭ�� ���ͷ� ����
                reactVec += Vector3.up * 3; //�������� ���� ��


                rigid.freezeRotation = false; //ȸ�� ���� ����
                rigid.AddForce(reactVec * 3, ForceMode.Impulse);
                rigid.AddTorque(reactVec* 3, ForceMode.Impulse); //ȸ�� �߰�

                Destroy(gameObject, 4); //3�� �� �� ����
            }
            else //�Ѿ˿� �¾��� ��
            {

                reactVec = reactVec.normalized; //����ȭ�� ���ͷ� ����
                reactVec += Vector3.up; //�������� ���� ��
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);

                Destroy(gameObject, 4); //3�� �� �� ����
            }
        }


    }
}
