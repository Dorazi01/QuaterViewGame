using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int damage; //�Ѿ��� ������


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor") //�浹�� ������Ʈ�� �ٴ��� ���
        {
            Destroy(gameObject,3); //�Ѿ��� ����
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall") //�浹�� ������Ʈ�� ���� ���
        {
            Destroy(gameObject); //�Ѿ��� ����
        }
    }


}
