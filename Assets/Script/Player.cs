using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;     //캐릭터 속도 선언
    

    float hAxis;
    float vAxis;
    bool wDown;     //쉬프트 키가 눌렸는지 확인하는 변수 선언]
    bool jDown;

    bool isJump;
    bool isDodge;



    bool isSide; //벽 충돌 유무
    Vector3 sideVec; //벽 충돌 방향 저장

   

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //리지드바디 컴포넌트 가져오기
        anim = GetComponentInChildren<Animator>(); //애니메이터 컴포넌트 가져오기
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        GetInput(); //입력값을 받아오는 함수 호출

        Move();
        //이동하는 함수 호출

        PlayerTurn(); //캐릭터가 바라보는 방향을 바꿔주는 함수 호출
        Jump(); //점프하는 함수 호출
        Dodge();




    }



    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        wDown = Input.GetButton("Walk"); //쉬프트키가 눌렸는지 확인하는 변수에 대입
        jDown = Input.GetButtonDown("Jump"); //점프키가 눌렸는지 확인하는 변수에 대입
    }


    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            dodgeVec = moveVec;

        //충돌하는 방향은 무시
        if (isSide && moveVec == sideVec)
            moveVec = Vector3.zero;

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void PlayerTurn()
    {
        transform.LookAt(transform.position + moveVec); //캐릭터가 바라보는 방향을 바꿔줌
    }

    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge)        //움직이지 않고 있을 때 스페이스 바를 누르면
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse ); //위쪽으로 힘을 가해줌 (점프)_ , ForceMode.Impulse는 순간적으로 힘을 가해주는 것
            anim.SetBool("isJump", true); 
            anim.SetTrigger("doJump");
            isJump = true; //점프를 했다고 표시
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
        if(collision.gameObject.tag == "Floor") //바닥에 닿았을때
        {
            anim.SetBool("isJump", false);
            isJump = false; //점프를 하지 않았다고 표시
        }
    }
    //벽 충돌 In 체크
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isSide = true;
            sideVec = moveVec;
        }
    }
    //벽 충돌 Out 체크
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isSide = false;
            sideVec = Vector3.zero;
        }
    }






}
