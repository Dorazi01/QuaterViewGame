using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public Transform target;  // ī�޶� ���� Ÿ�� ���� ����
    public Vector3 offset;  // ī�޶�� Ÿ�� ������ ������ ���� ����


    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;  // ī�޶��� ��ġ�� Ÿ���� ��ġ�� �������� ���� ������ ����

    }
}
