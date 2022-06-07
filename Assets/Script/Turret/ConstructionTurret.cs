using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTurret : MonoBehaviour
{
    [SerializeField]
    string filePath;

    //Ÿ�̸�
    public float timer;

    //��ȯ�� �ͷ�
    public int currentSelectedTurretIdx;

    //��ȯ�� ���� 
    public GameObject nestGo;

    private void Update()
    {
        UpdateBuildTurret();
    }

    public void UpdateBuildTurret()
    {
        //����ð� ����
        if (Time.time - timer > SystemManager.Instance.TurretManager.turretConstructionTime[currentSelectedTurretIdx])
        {
            //�ͷ�����
            GameObject turretGo = SystemManager.Instance.TurretManager.EnableTurret(currentSelectedTurretIdx, nestGo.transform.position);

            if (!turretGo)
                return;

            Turret turret = turretGo.GetComponent<Turret>();

            // �ͷ� ���� ���� �г� ����
            SystemManager.Instance.PanelManager.EnablePanel<StatusMngPanel>(3, turret.hpPos.transform.position, turret.turretIndex, turret.GetType());
            //Debug.Log("turret.type=" + turret.GetType().Name);
            if (!SystemManager.Instance.PanelManager.statusMngPanel)
                return;



            //�������� ����
            Nest nest = nestGo.GetComponent<Nest>();
            if (nest)
            {
                turretGo.GetComponent<Turret>().nest = nestGo;
                nest.haveTurret = true;
                nest.turret = turretGo;
            }

            //����� �ͷ� ����
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
        }


    }
}
