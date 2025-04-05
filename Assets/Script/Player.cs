using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;     //ĳ���� �ӵ� ����
    

    float hAxis;
    float vAxis;
    bool wDown;     //����Ʈ Ű�� ���ȴ��� Ȯ���ϴ� ���� ����]
    bool jDown;

    bool isJump;
    bool isDodge;



    bool isSide; //�� �浹 ����
    Vector3 sideVec; //�� �浹 ���� ����

   

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //������ٵ� ������Ʈ ��������
        anim = GetComponentInChildren<Animator>(); //�ִϸ����� ������Ʈ ��������
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        GetInput(); //�Է°��� �޾ƿ��� �Լ� ȣ��

        Move();
        //�̵��ϴ� �Լ� ȣ��

        PlayerTurn(); //ĳ���Ͱ� �ٶ󺸴� ������ �ٲ��ִ� �Լ� ȣ��
        Jump(); //�����ϴ� �Լ� ȣ��
        Dodge();




    }



    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        wDown = Input.GetButton("Walk"); //����ƮŰ�� ���ȴ��� Ȯ���ϴ� ������ ����
        jDown = Input.GetButtonDown("Jump"); //����Ű�� ���ȴ��� Ȯ���ϴ� ������ ����
    }


    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            dodgeVec = moveVec;

        //�浹�ϴ� ������ ����
        if (isSide && moveVec == sideVec)
            moveVec = Vector3.zero;

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void PlayerTurn()
    {
        transform.LookAt(transform.position + moveVec); //ĳ���Ͱ� �ٶ󺸴� ������ �ٲ���
    }

    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge)        //�������� �ʰ� ���� �� �����̽� �ٸ� ������
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse ); //�������� ���� ������ (����)_ , ForceMode.Impulse�� ���������� ���� �����ִ� ��
            anim.SetBool("isJump", true); 
            anim.SetTrigger("doJump");
            isJump = true; //������ �ߴٰ� ǥ��
        }

    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge)       
        {
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true; 

            Invoke("DodgeOut", 0.4f); 

        }

    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor") //�ٴڿ� �������
        {
            anim.SetBool("isJump", false);
            isJump = false; //������ ���� �ʾҴٰ� ǥ��
        }
    }
    //�� �浹 In üũ
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isSide = true;
            sideVec = moveVec;
        }
    }
    //�� �浹 Out üũ
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isSide = false;
            sideVec = Vector3.zero;
        }
    }






}
