using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;     //캐릭터 속도 선언

    float hAxis;
    float vAxis;
    bool wDown;     //쉬프트 키가 눌렸는지 확인하는 변수 선언

    Vector3 moveVec;

    Animator anim;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>(); //애니메이터 컴포넌트 가져오기
        
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
        wDown = Input.GetButton("Walk"); //쉬프트키가 눌렸는지 확인하는 변수에 대입






        moveVec = new Vector3(hAxis, 0, vAxis).normalized;  //이동속도가 어디로 가던 속도가 1로 정해지게끔 normalized를 사용함

        transform.position += moveVec * speed * (wDown ? 0.3f: 1f) * Time.deltaTime; //이동속도 * 프레임시간을 곱해줌으로써 프레임에 상관없이 일정한 속도로 이동하게끔 해줌
        //삼항 연산자  = 쉬프트키가 눌렸을 땐 0.3배속, 누르지 않았을 땐 1로



        anim.SetBool("isRun", moveVec != Vector3.zero); //이동속도가 0이 아닐때 애니메이션을 실행시켜줌
        anim.SetBool("isWalk", wDown);  //쉬프트가 눌렸을때 애니메이션 실행


        transform.LookAt(transform.position + moveVec); //캐릭터가 바라보는 방향을 바꿔줌




    }
}
