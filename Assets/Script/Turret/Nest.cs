using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    //터렛을 가지고 있는지 여부
    public bool haveTurret = false;

    //공사 여부
    public bool construction = false;

    //가지고 있는 터렛
    public GameObject turret;

    //버프 이펙트 포지션
    public GameObject[] buffEffectPos;
}
