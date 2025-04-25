using System.Collections;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {

        yield return new WaitForSeconds(2.7f);
        rigid.angularVelocity = Vector3.zero;
        rigid.linearVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hitobj in rayHits)
        {
            hitobj.transform.GetComponent<Enemy>().HitByNade(transform.position);
        }


        Destroy(gameObject, 5); //5초 후 삭제

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
