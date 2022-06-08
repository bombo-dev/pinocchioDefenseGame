using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTurret : MonoBehaviour
{
    const int CONSTRUCTIONCOMPLETEDEffectINDEX = 54;

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

    //��� ������ ���É����� �ͷ� ���� ����
    public bool startConstruction = false;

    private void Update()
    {
        if(startConstruction)
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
                nest.construction = false;  //��������
                nest.haveTurret = true; //�ͷ� ����
                nest.turret = turretGo;
            }

            //���� ������ �г� ����
            SystemManager.Instance.PanelManager.DisablePanel<UI_ConstructionGauge>(constructionGaugePanel);

            //����Ϸ� ����Ʈ ���
            SystemManager.Instance.EffectManager.EnableEffect(CONSTRUCTIONCOMPLETEDEffectINDEX, transform.position);

            //���� �ʱ�ȭ
            Reset();

            //����� �ͷ� ����
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
        }
        else
        {
            constructionValue = (Time.time - timer) / constructionTime;
        }


    }


    /// <summary>
    /// ����� �ͷ� ���� �ʱ�ȭ : ������
    /// </summary>
    private void Reset()
    {
        //��뺯�� �ʱ�ȭ

        timer = Time.time;

        currentSelectedTurretIdx = 0;

        nestGo = null;

        constructionTime = 0;

        constructionValue = 0;

        constructionGaugePanel = null;

        startConstruction = false;
    }
}
