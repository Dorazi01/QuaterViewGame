using System.Collections;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range }
    public Type type;  //���� Ÿ��
    public int damage; //���� ������
    public float rate;   //���� �߻� �ӵ�
    public BoxCollider meleeArea; //���� ���� ����
    public TrailRenderer trailEffect; //���� ����


    /// <summary>
    /// �÷��̾ ���⸦ ����� ��
    /// </summary>
    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing"); //�ڷ�ƾ ����
            StartCoroutine("Swing"); //�ڷ�ƾ ���
        }

        
    }

    //�Լ� ��� ���� ��� ����� �Ҷ� = Invoke�� ���������, �ڷ�ƾ�� �����


    //�ڷ�ƾ ���� ���η�ƾ�� ���� ����� �ڷ�ƾ�� �ڰ� Co-op��
    

    /// <summary>
    /// �ڷ�ƾ ���ذ� �� �ȵ�
    /// </summary>
    /// <returns></returns>
    IEnumerator Swing()
    {

        yield return new WaitForSeconds(0.1f); //0.5�� ��ⰰ�� ���ĵ� ��� ����
        meleeArea.enabled = true; //���� ���� ���� Ȱ��ȭ
        trailEffect.enabled = true; //���� ���� Ȱ��ȭ

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false; //���� ���� ���� ��Ȱ��ȭ


        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false; //���� ���� ��Ȱ��ȭ
        /*
        yield return null;
        //��� ���� Ű���� yield
        //yield return null;  //���� �����ӱ��� ���

        yield break; //�ڷ�ƾ ����
        */
    }




}
