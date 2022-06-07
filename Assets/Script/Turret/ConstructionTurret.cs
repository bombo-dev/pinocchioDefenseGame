using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTurret : MonoBehaviour
{
    [SerializeField]
    string filePath;

    //타이머
    public float timer;

    //소환할 터렛
    public int currentSelectedTurretIdx;

    //소환할 둥지 
    public GameObject nestGo;

    private void Update()
    {
        UpdateBuildTurret();
    }

    public void UpdateBuildTurret()
    {
        //공사시간 종료
        if (Time.time - timer > SystemManager.Instance.TurretManager.turretConstructionTime[currentSelectedTurretIdx])
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
                nest.haveTurret = true;
                nest.turret = turretGo;
            }

            //공사용 터렛 제거
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
        }


    }
}
