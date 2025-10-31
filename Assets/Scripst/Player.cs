using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; // 스피드 변수 선언
    public GameObject[] weapons;
    public bool[] hasweapons;

    float hAxis; // 이동을 위한 변수 선언
    float vAxis; // 이동을 위한 변수 선언
    bool wDown; // 걷기를 위한 변수 선언
    bool jDown; // 점프를 위한 변수 선언
    bool iDown;

    bool isjump; // 당신은 지금 점프를 하고 있습니까? 변수 선언
    bool isDodge; // 닷지를 위한 변수 선언

    Vector3 moveVec; // 이동을 위한 변수 선언
    Vector3 dodgeVec; // 닷지를 위한 변수 선언

    Rigidbody rigid; // 물리를 위한 변수 선언
    Animator anim; // 애니메이션을 위한 변수 선언

    GameObject nearObject; //오브젝트 먹기를 위한 변수 선언

    void Awake()
    {
        rigid = GetComponent<Rigidbody>(); // 물리효과를 위해 Rigidbody 함수 선언 후, 변수 초기화
        anim = GetComponentInChildren<Animator>(); // 
    }

    

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
        Interation();
    }

    //Input 관련 함수를 묶어주었다.
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //이동 함수의 초기화
        vAxis = Input.GetAxisRaw("Vertical"); //이동 함수의 초기화
        wDown = Input.GetButton("Walk"); //걷기 함수의 초기화
        jDown = Input.GetButtonDown("Jump"); //점프 함수의 초기화
        iDown = Input.GetButtonDown("Interation");
    }


    //move 관련 함수들도 합침
    void Move()
    {
        //vector3를 쓰는건 x, y, z를 쓰기 위해서다.
        // x축과 y축, z 축에 대해서 사용할 것을 확인한다. 노멀라이즈 방향값이 1로 보정된 벡터를 사용해야 대각선도 동일한 값을 가진다.
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; 

        if (isDodge)
            moveVec = dodgeVec;

        //transform은 어떤 오브젝트든지 기본적으로 들어있다.
        //transform을 사용하면 델타 타임을 꼭 넣어야 한다.
        //델타 타임은 초당 실행횟수인데 간단하게 말하면, 똥컴이든 좋은컴이든 동일한 속도로 1초당 이동거리가 동일하게 유지된다.

        if (wDown)
            transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        else
            transform.position += moveVec * speed * Time.deltaTime;



        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if(jDown && moveVec == Vector3.zero && !isjump && !isDodge) // 조건을 통해 닷지든 점프든 한개만 
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse); // 점프 파워를 지정
            anim.SetBool("isJump", true); // 연속 점프를 막기 위한것
            anim.SetTrigger("doJump");
            isjump = true;
        }
    }
    void Dodge()
    {
        //점프를 하고 있지 않을때 
        if(jDown && moveVec != Vector3.zero && !isjump && !isDodge) // 조건을 통해 닷지든 점프든 한개만 
        {
            dodgeVec = moveVec; //닷지백터의 이용
            speed *= 2; //속도를 두배로 늘렸다.
            anim.SetTrigger("doDodge"); //애니메이션
            isDodge = true; 

            Invoke("DodgeOut", 0.5f); // 인보크 함수로 시간차 함수를 호출하였다. 첫번째 파라메터와 두번째 파라메터를 적용하였다.  
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false; // 닷지가 끝나면 
    }

    void Interation()
    {
        if(iDown && nearObject != null && !isjump && !isDodge)
        {
            if(nearObject.tag == "Weapon")
            {
                item item = nearObject.GetComponent<item>();
                int weaponIndex = item.value;
                hasweapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }

    //점프 후 바닥에 닿았을때를 위한 함수
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isjump = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "weapon") // 웨폰 태그면 아래의 오브젝트에 저장한다.
            nearObject = other.gameObject;
        
        Debug.Log(nearObject.name);
    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "weapon") // 웨폰 태그의 영향에서 벗어나면 널값을 적용한다.
            nearObject = null;
    }


}
