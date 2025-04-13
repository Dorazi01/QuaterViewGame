using UnityEngine;

public class Item : MonoBehaviour
{

    public enum Type { Ammo, Coin, Granade, Heart, Waepon };
    //������ �ƴ� �ϳ��� ���� Ÿ��

    public Type type; //���� Ÿ���� ������ ����
    public int value; //�������� ��

    Rigidbody rigid;
    SphereCollider sphereCollider;



    private void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //Rigidbody ������Ʈ�� ������
        sphereCollider = GetComponent<SphereCollider>(); //SphereCollider ������Ʈ�� ������
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.up * 30 * Time.deltaTime); //�������� ȸ���ϴ� �Լ�

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) //�÷��̾�� �浹���� ��
        {
            rigid.isKinematic = true; //���������� ������ ���� ����
            sphereCollider.enabled = false; //�浹ü�� Trigger�� �۵���
        }
    }

}
