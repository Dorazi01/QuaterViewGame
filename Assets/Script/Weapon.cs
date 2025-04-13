using System.Collections;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range }
    public Type type;  //무기 타입
    public int damage; //무기 데미지
    public float rate;   //무기 발사 속도
    public BoxCollider meleeArea; //근접 공격 범위
    public TrailRenderer trailEffect; //무기 궤적


    /// <summary>
    /// 플레이어가 무기를 사용할 때
    /// </summary>
    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing"); //코루틴 정지
            StartCoroutine("Swing"); //코루틴 사용
        }

        
    }

    //함수 출력 도중 잠깐 멈춰야 할때 = Invoke를 사용하지만, 코루틴도 사용함


    //코루틴 사용시 메인루틴과 같이 실행됨 코루틴의 코가 Co-op임
    

    /// <summary>
    /// 코루틴 이해가 잘 안됨
    /// </summary>
    /// <returns></returns>
    IEnumerator Swing()
    {

        yield return new WaitForSeconds(0.1f); //0.5초 대기같은 형식도 사용 가능
        meleeArea.enabled = true; //근접 공격 범위 활성화
        trailEffect.enabled = true; //무기 궤적 활성화

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false; //근접 공격 범위 비활성화


        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false; //무기 궤적 비활성화
        /*
        yield return null;
        //결과 전달 키워드 yield
        //yield return null;  //다음 프레임까지 대기

        yield break; //코루틴 종료
        */
    }




}
