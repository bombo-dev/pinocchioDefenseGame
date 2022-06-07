using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTurret : MonoBehaviour
{
    const int CONSTRUCTIONCOMPLETEDEffectINDEX = 54;

    [SerializeField]
    string filePath;

    //게이지 패널이 생성될 위치
    public Transform gauegePos;

    //타이머
    public float timer;

    //소환할 터렛
    public int currentSelectedTurretIdx;

    //소환할 둥지 
    public GameObject nestGo;

    //건설에 걸리는 시간
    public float constructionTime;

    //건설시간 값 0~1
    public float constructionValue ;

    //건설 게이지 패널 정보
    public GameObject constructionGaugePanel;

    //모든 변수가 셋팅됬을때 터렛 공사 시작
    public bool startConstruction = false;

    private void Update()
    {
        if(startConstruction)
            UpdateBuildTurret();
    }

    /// <summary>
    /// 터렛 건설전 공사 동작 처리 : 김현진
    /// </summary>
    public void UpdateBuildTurret()
    {
        //공사시간 종료
        if (Time.time - timer > constructionTime)
        {
            //터렛생성
            GameObject turretGo = SystemManager.Instance.TurretManager.EnableTurret(currentSelectedTurretIdx, nestGo.transform.position);

            if (!turretGo)
                return;

            Turret turret = turretGo.GetComponent<Turret>();

            // 터렛 상태 관리 패널 생성
            SystemManager.Instance.PanelManager.EnablePanel<StatusMngPanel>(3, turret.hpPos.transform.position, turret.turretIndex, turret.GetType());
            //Debug.Log("turret.type=" + turret.GetType().Name);
            if (!SystemManager.Instance.PanelManager.statusMngPanel)
                return;



            //둥지정보 갱신
            Nest nest = nestGo.GetComponent<Nest>();
            if (nest)
            {
                turretGo.GetComponent<Turret>().nest = nestGo;
                nest.construction = false;  //공사종료
                nest.haveTurret = true; //터렛 생성
                nest.turret = turretGo;
            }

            //공사 게이지 패널 제거
            SystemManager.Instance.PanelManager.DisablePanel<UI_ConstructionGauge>(constructionGaugePanel);

            //공사완료 이펙트 출력
            SystemManager.Instance.EffectManager.EnableEffect(CONSTRUCTIONCOMPLETEDEffectINDEX, transform.position);

            //변수 초기화
            Reset();

            //공사용 터렛 제거
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
        }
        else
        {
            constructionValue = (Time.time - timer) / constructionTime;
        }


    }


    /// <summary>
    /// 공사용 터렛 변수 초기화 : 김현진
    /// </summary>
    private void Reset()
    {
        //사용변수 초기화

        timer = Time.time;

        currentSelectedTurretIdx = 0;

        nestGo = null;

        constructionTime = 0;

        constructionValue = 0;

        constructionGaugePanel = null;

        startConstruction = false;
    }
}
