using UnityEngine;

public class Item : MonoBehaviour
{

    public enum Type { Ammo, Coin, Granade, Heart, Waepon };
    //������ �ƴ� �ϳ��� ���� Ÿ��

    public Type type; //���� Ÿ���� ������ ����
    public int value; //�������� ��




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.up * 30 * Time.deltaTime); //�������� ȸ���ϴ� �Լ�

    }
}
