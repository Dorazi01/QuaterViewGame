using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{

    public float speed;     //캐릭터 속도 선언
    public GameObject[] waepons; //무기 배열 선언
    public bool[] hasWaepons; //무기 보유 여부 배열 선언


    float hAxis;
    float vAxis;
    bool wDown;     //쉬프트 키가 눌렸는지 확인하는 변수 선언]
    bool jDown;
    bool iDown;     //E키 입력
    bool sDown1;
    bool sDown2;
    bool sDown3;

    bool isJump;
    bool isDodge;
    bool isSwap;


    bool isSide; //벽 충돌 유무
    Vector3 sideVec; //벽 충돌 방향 저장

   

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObject; //근처에 있는 오브젝트를 저장할 변수 선언
    GameObject equipWaepon; //장착된 무기를 저장할 변수 선언

    int equipWaeponIndex = -1; //장착된 무기의 인덱스 저장할 변수 선언
    //첫 인덱스 값이 -1인 이유는 해머의 인덱스 값이 0이고, int의 초기값은0으로 지정되므로 해머가 없어도 장착될 문제가 발생하기 때문이다

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //리지드바디 컴포넌트 가져오기
        anim = GetComponentInChildren<Animator>(); //애니메이터 컴포넌트 가져오기
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    /// <summary>
    /// 업데이트 구간
    /// </summary>
    void Update()
    {
        
        GetInput(); //입력값을 받아오는 함수 호출

        Move();
        //이동하는 함수 호출

        PlayerTurn(); //캐릭터가 바라보는 방향을 바꿔주는 함수 호출
        Jump(); //점프하는 함수 호출
        Dodge();
        Interaction();
        Swap();


    }


    /// <summary>
    /// 플레이어의 키보드 입력을 받아오는 함수
    /// </summary>
    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        wDown = Input.GetButton("Walk"); //쉬프트키가 눌렸는지 확인하는 변수에 대입
        jDown = Input.GetButtonDown("Jump"); //점프키가 눌렸는지 확인하는 변수에 대입
        iDown = Input.GetButtonDown("Interaction"); //점프키가 눌렸는지 확인하는 변수에 대입
        sDown1 = Input.GetButtonDown("Swap1"); //무기1변경
        sDown2 = Input.GetButtonDown("Swap2"); //무기2변경
        sDown3 = Input.GetButtonDown("Swap3"); //무기3변경
    }


    /// <summary>
    /// 캐릭터를 이동시키는 함수 
    /// </summary>
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            dodgeVec = moveVec;

        if (isSwap)
            moveVec = Vector3.zero; //스왑 중에는 이동을 하지 않음

        //충돌하는 방향은 무시
        if (isSide && moveVec == sideVec)
            moveVec = Vector3.zero;

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }


    /// <summary>
    /// 캐릭터가 바라보는 방향을 이동 방향으로 바꿔주는 함수
    /// </summary>
    void PlayerTurn()
    {
        transform.LookAt(transform.position + moveVec); //캐릭터가 바라보는 방향을 바꿔줌
    }

    /// <summary>
    /// 멈춰있을 때 스페이스바를 누르면 점프하는 함수
    /// </summary>
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
    
    /// <summary>
    /// 움직이고 있는 상태에서, 스페이스바를 눌렀을 때 기본 이속의 2배로 구르기 하는 함수
    /// </summary>
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

    /// <summary>
    /// 무기를 교체하는 함수 , 중복 무기 교체방지와 미장착 무기 교체 방지가 들어가있는 함수
    /// </summary>
    void Swap()
    {

        if (sDown1 && (!hasWaepons[0] || equipWaeponIndex == 0)) return;
        if (sDown2 && (!hasWaepons[1] || equipWaeponIndex == 1)) return;
        if (sDown3 && (!hasWaepons[2] || equipWaeponIndex == 2)) return;
        //무기를 가지고있지 않거나 지금 장착된 무기에서 똑같은 무기로 스왑하려는 경우 실행하지 않음


        //그 밖의 경우 = 무기를 가지고 있으며, 지금 장착된 무기와 다른 무기로 스왑할 때
        int waeponIndex = -1;   //값을 초기화하고

        //입력 값에 따라서 무기데이터 인덱스 변경함
        if (sDown1) waeponIndex = 0;                //무기1변경
        if (sDown2) waeponIndex = 1;                //무기1변경
        if (sDown3) waeponIndex = 2;                //무기1변경

        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
        {
            if (equipWaepon != null)                //장착된 무기가 있을 경우에만
            {
                equipWaepon.SetActive(false);       //장착된 무기를 비활성화함
            }

            equipWaeponIndex = waeponIndex;        //장착된 무기의 인덱스를 저장함
            equipWaepon = waepons[waeponIndex];     //장착된 무기가 무엇인지 저장한 후에
            waepons[waeponIndex].SetActive(true);   //장착된 무기를 활성화함

            anim.SetTrigger("doSwap");              //무기 교체 애니메이션 실행

            isSwap = true; 

            Invoke("SwapOut", 0.4f); //0.4초 후에 SwapOut() 함수를 실행함

        }


    }

    void SwapOut()
    {
        isSwap = false;
    }


    /// <summary>
    /// E키를 눌렀을 때 주변 아이템이 존재할 경우 아이템을 hasWaepons 배열에 저장하는 함수
    /// </summary>
    void Interaction()
    {
        if (iDown && nearObject != null && !isJump && !isDodge) //E키를 눌렀을 때 주변 아이템이 있을 경우
        {
            if (nearObject.tag == "Waepon")
            {
                Item item = nearObject.GetComponent<Item>(); //아이템 컴포넌트를 가져옴
                int waeponIndex = item.value; //아이템의 값을 가져옴
                hasWaepons[waeponIndex] = true; //아이템을 보유하고 있다고 표시
                //결론 : 근접한 아이템의 컴포넌트 가져오기 -> 그 아이템의 값을 가져옴 ex)망치= 0 권총 = 1, 따발총 = 2.
                // -> 그 값을 hasWaepons 배열로 주소를 잡고, 그 아이템을 가지고 있다고 표시함.
                // hasWaepons[1] = true ---> 권총을 가지고 있다
                Destroy(nearObject); //아이템을 삭제
            }

        }

    }


    /// <summary>
    /// 바닥에 닿지 않았을때 점프하지 않기 위한 함수
    /// </summary>
    /// <param name="collision"></param>
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

    /// <summary>
    /// 무기 오브젝트에 닿았을 때 그 무기를 nearObject에 저장하는 함수
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Waepon")
        {
            nearObject = other.gameObject; //근처에 있는 오브젝트를 저장

            //Debug.Log(nearObject.name); //근처에 있는 오브젝트의 이름을 출력 (실험용)
        }



    }

    /// <summary>
    /// 무기 오브젝트에서 벗어났을 때 nearObject를 null로 초기화하는 함수
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Waepon")
        {
            nearObject = null; //근처에 있는 오브젝트를 저장
        }
    }





}
