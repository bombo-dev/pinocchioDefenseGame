using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTurret : MonoBehaviour
{
    [SerializeField]
    string filePath;

    //������ �г��� ������ ��ġ
    public Transform gauegePos;

    //Ÿ�̸�
    public float timer;

    //��ȯ�� �ͷ�
    public int currentSelectedTurretIdx;

    //��ȯ�� ���� 
    public GameObject nestGo;

    //�Ǽ��� �ɸ��� �ð�
    public float constructionTime;

    //�Ǽ��ð� �� 0~1
    public float constructionValue ;

    //�Ǽ� ������ �г� ����
    public GameObject constructionGaugePanel;

    private void Update()
    {
        UpdateBuildTurret();
    }

    /// <summary>
    /// �ͷ� �Ǽ��� ���� ���� ó�� : ������
    /// </summary>
    public void UpdateBuildTurret()
    {
        //����ð� ����
        if (Time.time - timer > constructionTime)
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

            //���� ������ �г� ����
            SystemManager.Instance.PanelManager.DisablePanel<UI_ConstructionGauge>(constructionGaugePanel);

            //����� �ͷ� ����
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
        }
        else
        {
            constructionValue = (Time.time - timer) / constructionTime;
        }


    }
}
