using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; // 스피드 함수 선언
    float hAxis; // 이동을 위한 함수 선언
    float vAxis; // 이동을 위한 함수 선언
    bool wDown;
    bool jDown;
    //bool iDown;

    bool isjump;
    bool isDodge;

    Vector3 moveVec; // 이동을 위한 함수 선언
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //이동 함수의 초기화
        vAxis = Input.GetAxisRaw("Vertical"); //이동 함수의 초기화
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        //iDown = Input.GetButtonDown("Interation");
    }

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
        if(jDown && moveVec == Vector3.zero && !isjump && !isDodge)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isjump = true;
        }
    }
    void Dodge()
    {
        if(jDown && moveVec != Vector3.zero && !isjump && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f); 
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isjump = false;
        }
    }

    //    void OnTriggerStay(Collider other)
    //    {
    //        if(other.tag == "weapon")
    //            nearObject = other.gameObject;
    //        Debug.Log(nearObject.name);
    //    }


    //    void OnTriggerExit(Collider other)
    //    {
    //        if (other.tag == "weapon")
    //            nearObject = null;
    //    }


}
