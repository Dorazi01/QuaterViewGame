using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int damage; //총알의 데미지


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor") //충돌한 오브젝트가 바닥일 경우
        {
            Destroy(gameObject,3); //총알을 삭제
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall") //충돌한 오브젝트가 벽일 경우
        {
            Destroy(gameObject); //총알을 삭제
        }
    }


}
