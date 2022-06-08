using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    //Ư���ͷ� �ε���
    int BASETURRET_INDEX = 23;
    public int CONSTRUCTIONTURRET_INDEX = 24;


    //Load�� Turret ������ ����
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // Ȱ��ȭ�� turret �޾ƿ� ����Ʈ
    public List<GameObject> turrets;

    [SerializeField]
    Transform turretParents;

    //filePath, cacheCount ����
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;

    //TurretNum���� �׼��� �� �� �ִ� �ͷ� �Ǽ��ð� �迭
    public float[] turretConstructionTime;

    // Start is called before the first frame update
    void Start()
    {
        PrepareData();

        //���̽� �ͷ� �Ǽ�
        EnableBase();
    }

    /// <summary>
    /// �� �ε� �� Enemy ĳ�� �����͸� �������� ������ �Լ� ȣ�� : ������
    /// </summary>
    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), turretParents);
        }
    }

    /// <summary>
    /// ���� ������ �ϴ� ���̽� �ͷ� �Ǽ� : ������
    /// </summary>
    void EnableBase()
    {
        GameObject go = EnableTurret(BASETURRET_INDEX, Vector3.zero);

        if (!go)
            return;

        //��ġ �ʱ�ȭ
        go.transform.localPosition = new Vector3(0f, 0f, -178f);

        Turret baseTurret = go.GetComponent<Turret>();
        // �ͷ� ���� ���� �г� ����
        GameObject statusMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<StatusMngPanel>(3, go);

        if (!SystemManager.Instance.PanelManager.statusMngPanel)
            return;

        StatusMngPanel statusMngPanel = statusMngPanelGo.GetComponent<StatusMngPanel>();
        baseTurret.statusMngPanel = statusMngPanel;

        statusMngPanel.panelPos = baseTurret.transform.position;
        statusMngPanel.hpBarOwner = go;
    }

    /// <summary>
    /// ������ ��θ� ���� ���ӿ�����Ʈ�� �����´� : ������
    /// </summary>
    /// <param name="filePath">�������� ������ִ� ���</param>
    /// <returns>��ο��� ������ ���� ������Ʈ</returns>
    GameObject Load(string filePath)
    {
        //�̹� ĳ�ÿ� ���ԵǾ� ���� ���
        if (prefabCaChes.ContainsKey(filePath))
            return prefabCaChes[filePath];
        else
        {
            GameObject go = Resources.Load<GameObject>(filePath);
            prefabCaChes.Add(filePath, go);

            return go;
        }
    }

    /// <summary>
    /// Turret ��ü�� ���� : ������
    /// </summary>
    /// <param name="enemyIndex">������ Turret�� ����� �ε���</param>
    /// <param name="turretPos">Turret�� ������ ��ġ</param>
    public GameObject EnableTurret(int turretIndex, Vector3 turretPos)
    {
        //����ó��
        if (turretIndex >= prefabCacheDatas.Length || prefabCacheDatas[turretIndex].filePath == null)
            return null;

        //������ ������ ���ӿ�����Ʈ ���� �޾ƿ���
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[turretIndex].filePath);

        if (go == null)
            return null;

        //�����ͷ��� ���
        if (turretIndex == CONSTRUCTIONTURRET_INDEX)
        {
            go.transform.position = turretPos;
            return go;
        }

        //������ �������� ��ȿ�� ���
        Turret turret = go.GetComponent<Turret>();
        turrets.Add(go);

        //�ͷ� ��ġ �ʱ�ȭ
        go.transform.position = turretPos;

        //������ ������ ��ü ���� �ʱ�ȭ
        turret.turretIndex = turrets.FindIndex(x => x == go); //enemise ����Ʈ�� �ε����� ��ġ�ϴ� ��ȣ ����

        //�ͷ��� �ʱ���·�
        turret.Reset();

        return go;
    }

    /// <summary>
    /// ����Ʈ���� ������ turret�� �����ϰ� ����Ʈ�� �籸��
    /// </summary>
    /// <param name="removeEnemyIndex">�籸���� ������ gameObject</param>
    public void ReorganizationEnemiesList(int removeTurretndex)
    {
        List<GameObject> tempTurrets = new List<GameObject>();
        int index = 0;

        for (int i = 0; i < turrets.Count; i++)
        {
            //������ gameObject�� ����
            if (i != removeTurretndex)
            {
                //enemies[i]�� null�̸� ����
                if (turrets[i])
                {
                    //����Ʈ �籸��
                    tempTurrets.Add(turrets[i]);
                    //enemyIndex��ȣ �ʱ�ȭ
                    turrets[i].GetComponent<Turret>().turretIndex = index;

                    index++;
                }
            }
        }//end of for

        turrets = tempTurrets;
    }
}
