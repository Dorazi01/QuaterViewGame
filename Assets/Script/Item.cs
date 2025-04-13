using UnityEngine;

public class Item : MonoBehaviour
{

    public enum Type { Ammo, Coin, Granade, Heart, Waepon };
    //변수가 아닌 하나의 열거 타입

    public Type type; //열거 타입을 변수로 선언
    public int value; //아이템의 값

    Rigidbody rigid;
    SphereCollider sphereCollider;



    private void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //Rigidbody 컴포넌트를 가져옴
        sphereCollider = GetComponent<SphereCollider>(); //SphereCollider 컴포넌트를 가져옴
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.up * 30 * Time.deltaTime); //아이템이 회전하는 함수

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) //플레이어와 충돌했을 때
        {
            rigid.isKinematic = true; //물리엔진의 영향을 받지 않음
            sphereCollider.enabled = false; //충돌체가 Trigger로 작동함
        }
    }

}
