using UnityEngine;

public class Item : MonoBehaviour
{

    public enum Type { Ammo, Coin, Granade, Heart, Waepon };
    //변수가 아닌 하나의 열거 타입

    public Type type; //열거 타입을 변수로 선언
    public int value; //아이템의 값




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.up * 30 * Time.deltaTime); //아이템이 회전하는 함수

    }
}
