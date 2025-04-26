using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{

    public float speed;     //ĳ���� �ӵ� ����
    public GameObject[] waepons; //���� �迭 ����
    public bool[] hasWaepons; //���� ���� ���� �迭 ����
    public GameObject[] granades; //����ź �迭 ����
    public GameObject granadeObj;
    public int hasGranades;
    public Camera followCam;
    


    public int ammo;
    public int coin;
    public int health;
    


    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGranades;


    float hAxis;
    float vAxis;

    bool wDown;     //����Ʈ Ű�� ���ȴ��� Ȯ���ϴ� ���� ����]
    bool jDown;
    bool fDown;     //FŰ �Է�
    bool gDown;
    bool rDown;     //RŰ �Է�
    bool iDown;     //EŰ �Է�
    bool sDown1;
    bool sDown2;
    bool sDown3;


    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isReload;
    bool isFireReady = true; //�߻� �غ� ����
    bool isBorder; //���� �ε������� Ȯ���ϴ� ���� ����
    bool isDamage;    //�ǰ� ���� üũ

    bool isSide; //�� �浹 ����
    Vector3 sideVec; //�� �浹 ���� ����

   

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;
    MeshRenderer[] meshes;

    GameObject nearObject; //��ó�� �ִ� ������Ʈ�� ������ ���� ����
    Weapon equipWaepon; //������ ���⸦ ������ ���� ����

    int equipWaeponIndex = -1; //������ ������ �ε��� ������ ���� ����
    //ù �ε��� ���� -1�� ������ �ظ��� �ε��� ���� 0�̰�, int�� �ʱⰪ��0���� �����ǹǷ� �ظӰ� ��� ������ ������ �߻��ϱ� �����̴�
    float fireDelay;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //������ٵ� ������Ʈ ��������
        anim = GetComponentInChildren<Animator>(); //�ִϸ����� ������Ʈ ��������
        meshes = GetComponentsInChildren<MeshRenderer>();
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    /// <summary>
    /// ������Ʈ ����
    /// </summary>
    void Update()
    {
        
        GetInput(); //�Է°��� �޾ƿ��� �Լ� ȣ��

        Move();
        //�̵��ϴ� �Լ� ȣ��

        PlayerTurn(); //ĳ���Ͱ� �ٶ󺸴� ������ �ٲ��ִ� �Լ� ȣ��
        Jump(); //�����ϴ� �Լ� ȣ��
        Attack(); //�����ϴ� �Լ� ȣ��
        Reload();
        Dodge();
        Interaction();
        Swap();
        Granade();

    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero; //ȸ�� �ӵ��� 0���� ������
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green); //�ʷϻ� ����ĳ��Ʈ�� ��Ƽ� ���� �ε������� Ȯ����
        isBorder = Physics.Raycast(transform.position, transform.forward, 5,LayerMask.GetMask("Wall")); //����ĳ��Ʈ�� ��Ƽ� ���� �ε������� Ȯ����

    }

    private void FixedUpdate()
    {
        FreezeRotation();
        StopToWall(); //���� �ε����� �� ���ߴ� �Լ� ȣ��
    }


    /// <summary>
    /// �÷��̾��� Ű���� �Է��� �޾ƿ��� �Լ�
    /// </summary>
    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        wDown = Input.GetButton("Walk"); //����ƮŰ�� ���ȴ��� Ȯ���ϴ� ������ ����
        jDown = Input.GetButtonDown("Jump"); //����Ű�� ���ȴ��� Ȯ���ϴ� ������ ����
        fDown = Input.GetButton("Fire1"); //�߻�Ű�� ���ȴ��� Ȯ���ϴ� ������ ����
        gDown = Input.GetButtonDown("Fire2"); //����ź
        rDown = Input.GetButtonDown("Reload"); //������Ű�� ���ȴ��� Ȯ���ϴ� ������ ����
        iDown = Input.GetButtonDown("Interaction"); //����Ű�� ���ȴ��� Ȯ���ϴ� ������ ����
        sDown1 = Input.GetButtonDown("Swap1"); //����1����
        sDown2 = Input.GetButtonDown("Swap2"); //����2����
        sDown3 = Input.GetButtonDown("Swap3"); //����3����
        
    }


    /// <summary>
    /// ĳ���͸� �̵���Ű�� �Լ� 
    /// </summary>
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            dodgeVec = moveVec;

        if (isSwap || !isFireReady || isReload) //���� ��ü + ���� �߻� �غ� �ȵ� ������ �� = ���� ��� �� ���� �����̱���
            moveVec = Vector3.zero; //���� �߿��� �̵��� ���� ����

        //�浹�ϴ� ������ ����
        if (isSide && moveVec == sideVec)
            moveVec = Vector3.zero;

        if (!isBorder)
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }




    /// <summary>
    /// ĳ���Ͱ� �ٶ󺸴� ������ �̵� �������� �ٲ��ִ� �Լ�
    /// </summary>
    void PlayerTurn()
    {
        //Ű���忡 ���� ȸ��
        transform.LookAt(transform.position + moveVec); //ĳ���Ͱ� �ٶ󺸴� ������ �ٲ���


        //���콺�� ���� ȸ��
        if (fDown)
        {
            Ray ray = followCam.ScreenPointToRay(Input.mousePosition); //���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100)) //����ĳ��Ʈ�� ��Ƽ� �浹�� ��ü�� ���� ���
                                                       //out = ��ȯ���� �־��� ������ �����ϴ� Ű���� :return�� �����
            {
                Vector3 nextVec = rayHit.point - transform.position; //�浹�� ��ü�� ��ġ - ĳ������ ��ġ
                nextVec.y = 0; //y���� 0���� ����
                transform.LookAt(transform.position + nextVec); //ĳ���Ͱ� �ٶ󺸴� ������ �ٲ���
            }
        }
        
    }

    /// <summary>
    /// �������� �� �����̽��ٸ� ������ �����ϴ� �Լ�
    /// </summary>
    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge && !isReload )        //�������� �ʰ� ���� �� �����̽� �ٸ� ������
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse ); //�������� ���� ������ (����)_ , ForceMode.Impulse�� ���������� ���� �����ִ� ��
            anim.SetBool("isJump", true); 
            anim.SetTrigger("doJump");
            isJump = true; //������ �ߴٰ� ǥ��
        }

    }



    void Granade()
    {
        if (hasGranades == 0)
        {
            return;
        }

        if (gDown && !isReload && !isSwap)
        {
            Ray ray = followCam.ScreenPointToRay(Input.mousePosition); //���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100)) //����ĳ��Ʈ�� ��Ƽ� �浹�� ��ü�� ���� ���
                                                       //out = ��ȯ���� �־��� ������ �����ϴ� Ű���� :return�� �����
            {
                Vector3 nextVec = rayHit.point - transform.position; //�浹�� ��ü�� ��ġ - ĳ������ ��ġ
                nextVec.y = 10; //y���� 0���� ����

                GameObject instantGranade = Instantiate(granadeObj, transform.position, transform.rotation);
                Rigidbody rigidGranade = instantGranade.GetComponent<Rigidbody>();
                rigidGranade.AddForce(nextVec, ForceMode.Impulse);
                rigidGranade.AddTorque(Vector3.back * 4, ForceMode.Impulse);

                hasGranades--;
                granades[hasGranades].SetActive(false);

            }
        }



        
    }




    void Attack()
    {
        if (equipWaepon == null)
        {
            return;
        }

        fireDelay += Time.deltaTime; //�߻� �����̸� ������Ŵ
        isFireReady = equipWaepon.rate < fireDelay; //�߻� �غ� ���¸� üũ��

        if (fDown && isFireReady &&!isDodge && !isSwap && !isReload&& !isJump)
        {
            equipWaepon.Use(); //���⸦ �����
            anim.SetTrigger(equipWaepon.type == Weapon.Type.Melee ?"doSwing" : "doShot"); //���� �ִϸ��̼� ����
            fireDelay = 0; //�߻� �����̸� �ʱ�ȭ��
        }

    }

    void Reload()
    {
        
        if (equipWaepon == null) //�տ� ���Ⱑ ������
        {
            return;     //����
        }
        else if (equipWaepon.type == Weapon.Type.Melee) //���������̸�
        {
            return;
        }
        else if (ammo == 0)//ź���� ������
        {
            return; 
        }
        else if (isReload == true)
        {
            return;
        }

        if (rDown && !isJump && !isDodge && !isSwap && isFireReady) //RŰ�� ������ ��
        {
            if (equipWaepon.curAmmo < equipWaepon.maxAmmo) //���� ������ ������ ��ź���� �ִ� ��ź������ ������
            {
                anim.SetTrigger("doReload"); //������ �ִϸ��̼� ����
                isReload = true; //������ ���·� ����


                Invoke("ReloadOut", 2f); //2�� �Ŀ� ReloadOut() �Լ��� ������

                
            }
        }



    }

    //���� ������ �ƴ� ź ����� ���缭 �ִ� ź�� ����, �÷��̾��� ź�� ���� ���ҽ��Ѿ� ��
    void ReloadOut()
    {
        int reAmmo;
        //reAmmo = �������� ź���� ��
        
        
        if (ammo >= equipWaepon.maxAmmo)
        {
            reAmmo = equipWaepon.maxAmmo - equipWaepon.curAmmo; //���� źȯ�� ����
            equipWaepon.curAmmo += reAmmo; //��ź���� �ִ� ��ź���� ������
        } 
        else
        {
            reAmmo = ammo < equipWaepon.maxAmmo ? ammo : equipWaepon.maxAmmo; //��ź���� �ִ� ��ź���� ������
            equipWaepon.curAmmo = reAmmo; //��ź���� �ִ� ��ź���� ������
        }

        

        ammo -= reAmmo; //���� ź���� ���ҽ�Ŵ

        isReload = false; //�������� ���� ���·� ����

    }





    
    /// <summary>
    /// �����̰� �ִ� ���¿���, �����̽��ٸ� ������ �� �⺻ �̼��� 2��� ������ �ϴ� �Լ�
    /// </summary>
    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap &&!isReload)       
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
    /// ���⸦ ��ü�ϴ� �Լ� , �ߺ� ���� ��ü������ ������ ���� ��ü ������ ���ִ� �Լ�
    /// </summary>
    void Swap()
    {

        if (sDown1 && (!hasWaepons[0] || equipWaeponIndex == 0)) return;
        if (sDown2 && (!hasWaepons[1] || equipWaeponIndex == 1)) return;
        if (sDown3 && (!hasWaepons[2] || equipWaeponIndex == 2)) return;
        //���⸦ ���������� �ʰų� ���� ������ ���⿡�� �Ȱ��� ����� �����Ϸ��� ��� �������� ����


        //�� ���� ��� = ���⸦ ������ ������, ���� ������ ����� �ٸ� ����� ������ ��
        int waeponIndex = -1;   //���� �ʱ�ȭ�ϰ�

        //�Է� ���� ���� ���ⵥ���� �ε��� ������
        if (sDown1) waeponIndex = 0;                //����1����
        if (sDown2) waeponIndex = 1;                //����1����
        if (sDown3) waeponIndex = 2;                //����1����

        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge &&!isReload)
        {
            if (equipWaepon != null)                //������ ���Ⱑ ���� ��쿡��
            {
                equipWaepon.gameObject.SetActive(false);       //������ ���⸦ ��Ȱ��ȭ��
            }

            equipWaeponIndex = waeponIndex;        //������ ������ �ε����� ������
            equipWaepon = waepons[waeponIndex].GetComponent<Weapon>();     //������ ���Ⱑ �������� ������ �Ŀ�
            equipWaepon.gameObject.SetActive(true);   //������ ���⸦ Ȱ��ȭ��

            anim.SetTrigger("doSwap");              //���� ��ü �ִϸ��̼� ����

            isSwap = true; 

            Invoke("SwapOut", 0.4f); //0.4�� �Ŀ� SwapOut() �Լ��� ������

        }


    }

    void SwapOut()
    {
        isSwap = false;
    }


    /// <summary>
    /// EŰ�� ������ �� �ֺ� �������� ������ ��� �������� hasWaepons �迭�� �����ϴ� �Լ�
    /// </summary>
    void Interaction()
    {
        if (iDown && nearObject != null && !isJump && !isDodge && !isReload) //EŰ�� ������ �� �ֺ� �������� ���� ���
        {
            if (nearObject.tag == "Waepon")
            {
                Item item = nearObject.GetComponent<Item>(); //������ ������Ʈ�� ������
                int waeponIndex = item.value; //�������� ���� ������
                hasWaepons[waeponIndex] = true; //�������� �����ϰ� �ִٰ� ǥ��
                //��� : ������ �������� ������Ʈ �������� -> �� �������� ���� ������ ex)��ġ= 0 ���� = 1, ������ = 2.
                // -> �� ���� hasWaepons �迭�� �ּҸ� ���, �� �������� ������ �ִٰ� ǥ����.
                // hasWaepons[1] = true ---> ������ ������ �ִ�
                Destroy(nearObject); //�������� ����
            }

        }

    }


    /// <summary>
    /// �ٴڿ� ���� �ʾ����� �������� �ʱ� ���� �Լ�
    /// </summary>
    /// <param name="collision"></param>
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

    /// <summary>
    /// ���� ������Ʈ�� ����� �� �� ���⸦ nearObject�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Waepon")
        {
            nearObject = other.gameObject; //��ó�� �ִ� ������Ʈ�� ����

            //Debug.Log(nearObject.name); //��ó�� �ִ� ������Ʈ�� �̸��� ��� (�����)
        }



    }

    /// <summary>
    /// ���� ������Ʈ���� ����� �� nearObject�� null�� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Waepon")
        {
            nearObject = null; //��ó�� �ִ� ������Ʈ�� ����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>(); //������ ������Ʈ�� ������

            //������ Ÿ���� ���� ���� ����ġ�� ĳ�̽��� 
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo) ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin) coin = maxCoin;
                    break;
                case Item.Type.Granade:
                    granades[hasGranades].SetActive(true); //����ź�� Ȱ��ȭ��
                    hasGranades += item.value;
                    if (hasGranades > maxHasGranades) hasGranades = maxHasGranades;
                    

                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth) health = maxHealth;
                    break;
            }
            Destroy(other.gameObject); //������ �������� ������

        }
        else if (other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet bullet = other.GetComponent<Bullet>();
                health -= bullet.damage; //���⿡�� �������� ������
                if (other.GetComponent<Rigidbody>() != null){       //������ �ƴ� �̻����� ���
                    Destroy(other.gameObject);      //�ǰ� �� �̻��� ����
                }
                StartCoroutine(OnDamage()); //������ �ڷ�ƾ ����
            }
            

        }

    }

    IEnumerator OnDamage()
    {
        isDamage = true; //�ǰ� ���·� ����
        foreach(MeshRenderer mesh in meshes) //��� �޽� �������� ����
        {
            mesh.material.color = Color.yellow; //��������� ����
        }
        yield return new WaitForSeconds(1f);


        isDamage = false; //�ǰ�X ���·� ����
        foreach (MeshRenderer mesh in meshes) //��� �޽� �������� ����
        {
            mesh.material.color = Color.white; //������� ����
        }
    }





}




