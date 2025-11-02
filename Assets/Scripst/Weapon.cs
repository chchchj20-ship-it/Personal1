using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }

    // 코루틴은 메인루틴 + 서브루틴이 동시에 실행되는것
    IEnumerator Swing()
    {
        //yield 결과를 전달하는 키워드
        //1
        yield return new WaitForSeconds(0.1f); // 0.1초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        //2
        yield return new WaitForSeconds(0.3f); // 1프레임 대기
        meleeArea.enabled = false;
        //3
        yield return new WaitForSeconds(0.3f); // 1프레임 대기
        trailEffect.enabled = false;
    }


}
