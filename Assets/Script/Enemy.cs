using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int maxHealth;
    public int curHealth;
    bool isHit = false; //���� �¾Ҵ��� Ȯ���ϴ� ����

    Material mat; //Material ������Ʈ

    Rigidbody rigid;
    BoxCollider boxColl;

    void Start()
    {
        rigid = GetComponent<Rigidbody>(); //Rigidbody ������Ʈ�� ������
        boxColl = GetComponent<BoxCollider>(); //BoxCollider ������Ʈ�� ������
        mat = GetComponent<MeshRenderer>().material; //Renderer ������Ʈ�� Material�� ������
    }

    // Update is called once per frame
    void Update()
    {
        
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
            StartCoroutine(OnDamage(reactVec)); //������ �ڷ�ƾ ����
            
        }
        else if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage; //���⿡�� �������� ������
            Vector3 reactVec = transform.position- other.transform.position; //�浹�� ������Ʈ�� ��ġ�� ������
            Destroy(other.gameObject); //�Ѿ� ����

            StartCoroutine(OnDamage(reactVec)); //������ �ڷ�ƾ ����
        }
    }

    void IsHitFalse()
    {
        isHit = false; //���� ���� �ʾ���
    }

    private void OnTriggerExit(Collider other)
    {
        
    }


    IEnumerator OnDamage(Vector3 reactVec)
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

            reactVec = reactVec.normalized; //����ȭ�� ���ͷ� ����
            reactVec += Vector3.up ; //�������� ���� ��
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);

            Destroy(gameObject, 4); //3�� �� �� ����
        }


    }
}
