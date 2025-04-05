using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;     //ĳ���� �ӵ� ����

    float hAxis;
    float vAxis;
    bool wDown;     //����Ʈ Ű�� ���ȴ��� Ȯ���ϴ� ���� ����

    Vector3 moveVec;

    Animator anim;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>(); //�ִϸ����� ������Ʈ ��������
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        wDown = Input.GetButton("Walk"); //����ƮŰ�� ���ȴ��� Ȯ���ϴ� ������ ����






        moveVec = new Vector3(hAxis, 0, vAxis).normalized;  //�̵��ӵ��� ���� ���� �ӵ��� 1�� �������Բ� normalized�� �����

        transform.position += moveVec * speed * (wDown ? 0.3f: 1f) * Time.deltaTime; //�̵��ӵ� * �����ӽð��� ���������ν� �����ӿ� ������� ������ �ӵ��� �̵��ϰԲ� ����
        //���� ������  = ����ƮŰ�� ������ �� 0.3���, ������ �ʾ��� �� 1��



        anim.SetBool("isRun", moveVec != Vector3.zero); //�̵��ӵ��� 0�� �ƴҶ� �ִϸ��̼��� ���������
        anim.SetBool("isWalk", wDown);  //����Ʈ�� �������� �ִϸ��̼� ����


        transform.LookAt(transform.position + moveVec); //ĳ���Ͱ� �ٶ󺸴� ������ �ٲ���




    }
}
