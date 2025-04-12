using UnityEngine;

public class GranadeOrbit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform target;    //공전할 중심점
    public float orbitSpeed;    //공전 속도
    Vector3 offSet;             //목표와의 거리




    void Start()
    {
        offSet = transform.position - target.position;      //목표와의 사이 거릿값
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offSet; //목표와의 거리값을 더해줌
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime); //회전
        offSet = transform.position - target.position;



    }
}
