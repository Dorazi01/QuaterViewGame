using UnityEngine;

public class GranadeOrbit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform target;    //������ �߽���
    public float orbitSpeed;    //���� �ӵ�
    Vector3 offSet;             //��ǥ���� �Ÿ�




    void Start()
    {
        offSet = transform.position - target.position;      //��ǥ���� ���� �Ÿ���
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offSet; //��ǥ���� �Ÿ����� ������
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime); //ȸ��
        offSet = transform.position - target.position;



    }
}
