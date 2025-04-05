using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public Transform target;  // 카메라가 따라갈 타겟 변수 선언
    public Vector3 offset;  // 카메라와 타겟 사이의 오프셋 변수 선언


    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;  // 카메라의 위치를 타겟의 위치에 오프셋을 더한 값으로 설정

    }
}
