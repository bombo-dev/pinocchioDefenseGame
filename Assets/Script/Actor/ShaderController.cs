using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    //���� ������Ʈ material ������ ���� ������Ƽ���
    MaterialPropertyBlock mpb;

    public int emission_propertyNameID = 279; // _Emission -> PropertyNameID = 279

    void Start()
    {
        mpb = new MaterialPropertyBlock();
    }

    /// <summary>
    /// �ǰ�ȿ�� WhiteFlash -> shader�� Emission������ ���� : ������
    /// </summary>
    /// <param name="rendererCaches">ĳ���� ���̴� ���� ���� ����Ʈ</param>
    /// <param name="emissionCaches">ĳ���� ���̴� emission ������Ƽ ���� ���� ����Ʈ</param>
    /// <param name="actor">ȣ���� Actor��ü</param>
    public IEnumerator ShowWhiteFlash(List<Renderer> rendererCaches, List<Vector4> emissionCaches, Actor actor)
    {
        //�ڷ�ƾ ���� �÷��� ����
        actor.showWhiteFlash_coroutine_is_running = true;

        //����ó��
        if (rendererCaches.Count <= 0 || (rendererCaches.Count != emissionCaches.Count))
        {
            actor.showWhiteFlash_coroutine_is_running = false;
            yield break;
        }

        OnWhiteFlash(rendererCaches, emission_propertyNameID);

        yield return new WaitForSeconds(0.1f);

        OffWhiteFlash(rendererCaches, emissionCaches, emission_propertyNameID);

        yield return new WaitForSeconds(0.1f);

        OnWhiteFlash(rendererCaches, emission_propertyNameID);

        yield return new WaitForSeconds(0.1f);

        OffWhiteFlash(rendererCaches, emissionCaches, emission_propertyNameID);

        //�ڷ�ƾ ���� �÷��� ����
        actor.showWhiteFlash_coroutine_is_running = false;
    }

    /// <summary>
    /// ���̴� ĳ�õ����� �ʱ�ȭ
    /// </summary>
    /// <param name="rendererArr">��ü ��ü Renderer</param>
    /// <param name="rendererCaches">ĳ���� ���̴� ���� ���� ����Ʈ</param>
    /// <param name="emissionCaches">ĳ���� ���̴� emission ������Ƽ ���� ���� ����Ʈ</param>
    public void InitializeShaderCaches(Renderer[] rendererArr, List<Renderer> rendererCaches , List<Vector4> emissionCaches)
    {
        //����ó��
        if (rendererArr.Length <= 0 || (rendererCaches.Count != emissionCaches.Count))
            return;

        //������ ���̴� ���� ĳ��
        for (int i = 0; i < rendererArr.Length; i++)
        {
            if (!(rendererArr[i].sharedMaterial.shader.name != "Custom/CustomToon" && rendererArr[i].sharedMaterial.shader.name != "Custom/Lambert_BlinnphongEmission" &&
                rendererArr[i].sharedMaterial.shader.name != "Custom/Lambert_Blinnphong"))
            {
                rendererCaches.Add(rendererArr[i]);
                emissionCaches.Add(rendererArr[i].sharedMaterial.GetVector(emission_propertyNameID));
            }
        }
    }

    /// <summary>
    /// ���̴��� ���� �ǰ� ȿ�� �����ֱ� : ������
    /// </summary>
    /// <param name="rendererCaches">������ ���̴� ���� ����ִ� Renderer ����Ʈ</param>
    /// <param name="emission_propertyNameID">ĳ���� ���̴� emission ������Ƽ ���� ���� ����Ʈ</param>
    void OnWhiteFlash(List<Renderer> rendererCaches, int emission_propertyNameID)
    {
        //����ó��
        if (rendererCaches.Count <= 0)
            return;

        mpb.SetVector(emission_propertyNameID, new Vector4(1, 1, 1, 1));
        for (int i = 0; i < rendererCaches.Count; i++)
        {
            rendererCaches[i].SetPropertyBlock(mpb);
        }

    }

    /// <summary>
    /// �ǰ� ȿ�� ����� ���� ������ ���̴� ���� �ǵ����� : ������
    /// </summary>
    /// <param name="rendererCaches">������ ���̴� ���� ����ִ� Renderer ����Ʈ</param>
    /// <param name="emissionCaches">������ emission�� ���� ����ִ� ����Ʈ</param>
    /// <param name="emission_propertyNameID">ĳ���� ���̴� emission ������Ƽ ���� ���� ����Ʈ</param>
    public void OffWhiteFlash(List<Renderer> rendererCaches, List<Vector4> emissionCaches, int emission_propertyNameID)
    {
        //����ó��
        if (rendererCaches.Count <= 0 || (rendererCaches.Count != emissionCaches.Count))
            return;

        for (int i = 0; i < rendererCaches.Count; i++)
        {
            mpb.SetVector(emission_propertyNameID, emissionCaches[i]);
            rendererCaches[i].SetPropertyBlock(mpb);
        }

    }
}
