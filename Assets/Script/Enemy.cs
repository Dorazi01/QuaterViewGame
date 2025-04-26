using System.Collections;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int maxHealth;
    public int curHealth;
    public Transform target;
    public bool isChase;
    bool isHit = false; //적이 맞았는지 확인하는 변수
    

    Material mat; //Material 컴포넌트
    Rigidbody rigid;
    BoxCollider boxColl;
    NavMeshAgent nav;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //Rigidbody 컴포넌트를 가져옴
        boxColl = GetComponent<BoxCollider>(); //BoxCollider 컴포넌트를 가져옴
        mat = GetComponentInChildren<MeshRenderer>().material; //Renderer 컴포넌트의 Material을 가져옴
        nav = GetComponent<NavMeshAgent>(); //NavMeshAgent 컴포넌트를 가져옴
        anim = GetComponentInChildren<Animator>(); //Animator 컴포넌트를 가져옴

        Invoke("ChaseStart", 2); 
    }



    // Update is called once per frame
    void Update()
    {
        if (isChase)
        {
            nav.SetDestination(target.position); //타겟의 위치로 이동
        }
        
    }


    void ChaseStart()
    {
        isChase = true; //추적 시작
        anim.SetBool("isWalk", true); //애니메이션 시작
    }





    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.linearVelocity = Vector3.zero; //회전 속도를 0으로 고정함
            rigid.angularVelocity = Vector3.zero; //회전 속도를 0으로 고정함
        }
        
    }

    private void FixedUpdate()
    {
        FreezeVelocity(); //속도 고정
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee"))
        {
            if (isHit) //적이 맞지 않았을 때
            {
                return;
            }
            isHit = true; //적이 맞았음
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage; //무기에서 데미지를 가져옴
            Vector3 reactVec = transform.position - other.transform.position; //충돌한 오브젝트의 위치를 가져옴
            Invoke("IsHitFalse", 0.4f);
            StartCoroutine(OnDamage(reactVec, false)); //데미지 코루틴 실행
            
        }
        else if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage; //무기에서 데미지를 가져옴
            Vector3 reactVec = transform.position- other.transform.position; //충돌한 오브젝트의 위치를 가져옴
            Destroy(other.gameObject); //총알 삭제

            StartCoroutine(OnDamage(reactVec,false)); //데미지 코루틴 실행
        }
    }

    void IsHitFalse()
    {
        isHit = false; //적이 맞지 않았음
    }

    public void HitByNade(Vector3 explosionPos)
    {
        curHealth -= 10; //수류탄 데미지
        Vector3 reactVec = transform.position - explosionPos; //충돌한 오브젝트의 위치를 가져옴
        StartCoroutine(OnDamage(reactVec,true)); //데미지 코루틴 실행
    }


    IEnumerator OnDamage(Vector3 reactVec, bool isNade)
    {
        mat.color = Color.red; //색상 변경

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            mat.color = Color.white; //색상 변경
        }
        else
        {
            mat.color = Color.gray; //색상 변경
            gameObject.layer = 11; //적 삭제 레이어로 변경
            isChase = false; //추적 중지
            nav.enabled = false; //NavMeshAgent 비활성화
            anim.SetTrigger("doDie"); //죽음 애니메이션 실행


            if (isNade) //수류탄에 맞았을 때
            {
                reactVec = reactVec.normalized; //정규화된 벡터로 변경
                reactVec += Vector3.up * 3; //위쪽으로 힘을 줌


                rigid.freezeRotation = false; //회전 고정 해제
                rigid.AddForce(reactVec * 3, ForceMode.Impulse);
                rigid.AddTorque(reactVec* 3, ForceMode.Impulse); //회전 추가

                Destroy(gameObject, 4); //3초 후 적 삭제
            }
            else //총알에 맞았을 때
            {

                reactVec = reactVec.normalized; //정규화된 벡터로 변경
                reactVec += Vector3.up; //위쪽으로 힘을 줌
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);

                Destroy(gameObject, 4); //3초 후 적 삭제
            }
        }


    }
}
