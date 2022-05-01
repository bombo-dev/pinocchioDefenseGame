using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    //���� ������Ʈ material ������ ���� ������Ƽ���
    MaterialPropertyBlock mpb;

    public bool showWhiteFlash_coroutine_is_running = false;//�ڷ�ƾ ������ ���� �÷���

    void Start()
    {
        mpb = new MaterialPropertyBlock();
    }

    /// <summary>
    /// �ǰ�ȿ�� WhiteFlash -> shader�� Emission������ ���� : ������
    /// </summary>
    public IEnumerator ShowWhiteFlash(Renderer[] rendererArr)
    {
        //�ڷ�ƾ ���� �÷��� ����
        showWhiteFlash_coroutine_is_running = true;

        // _Emission -> PropertyNameID = 279
        int emission_propertyNameID = 279;

        List<Renderer> rendererList = new List<Renderer>();
        List<Vector4> emissionList = new List<Vector4>();

        //���̴��� _Emission������ null�� �ƴ� rendererList����
        for (int i = 0; i < rendererArr.Length; i++)
        {
            if (!(rendererArr[i].sharedMaterial.shader.name != "Custom/CustomToon" && rendererArr[i].sharedMaterial.shader.name != "Custom/Lambert_BlinnphongEmission" &&
                rendererArr[i].sharedMaterial.shader.name != "Custom/Lambert_Blinnphong"))
            {
                rendererList.Add(rendererArr[i]);
                emissionList.Add(rendererArr[i].sharedMaterial.GetVector(emission_propertyNameID)); // _Emission -> PropertyNameID = 279
            }
        }

        //����ó��
        if (rendererList.Count <= 0)
        {
            yield break;
        }

        OnWhiteFlash(rendererArr, rendererList, emission_propertyNameID);

        yield return new WaitForSeconds(0.1f);

        OffWhiteFlash(rendererArr, rendererList, emissionList, emission_propertyNameID);

        yield return new WaitForSeconds(0.1f);

        OnWhiteFlash(rendererArr, rendererList, emission_propertyNameID);

        yield return new WaitForSeconds(0.1f);

        OffWhiteFlash(rendererArr, rendererList, emissionList, emission_propertyNameID);

        //�ڷ�ƾ ���� �÷��� ����
        showWhiteFlash_coroutine_is_running = false;
    }

    /// <summary>
    /// ���̴��� ���� �ǰ� ȿ�� �����ֱ� : ������
    /// </summary>
    /// <param name="rendererList">�ǰ�ȿ���� ������ ���̴� ������ ������ �ִ� Renderer ����Ʈ</param>
    void OnWhiteFlash(Renderer[] rendererArr, List<Renderer> rendererList, int emission_propertyNameID)
    {
        mpb.SetVector(emission_propertyNameID, new Vector4(1, 1, 1, 1));
        for (int i = 0; i < rendererList.Count; i++)
        {
            rendererArr[i].SetPropertyBlock(mpb);
        }

    }

    /// <summary>
    /// �ǰ� ȿ�� ����� ���� ������ ���̴� ���� �ǵ����� : ������
    /// </summary>
    /// <param name="rendererList">������Ƽ ���� ������� �ǵ����� ���̴� ������ �������ִ� Renderer ����Ʈ</param>
    /// <param name="emissionList">������� �ǵ����� ������Ƽ ���� �������ִ� ����Ʈ</param>
    void OffWhiteFlash(Renderer[] rendererArr, List<Renderer> rendererList, List<Vector4> emissionList, int emission_propertyNameID)
    {
        for (int i = 0; i < rendererList.Count; i++)
        {
            mpb.SetVector(emission_propertyNameID, emissionList[i]);
            rendererArr[i].SetPropertyBlock(mpb);
        }

    }
}
