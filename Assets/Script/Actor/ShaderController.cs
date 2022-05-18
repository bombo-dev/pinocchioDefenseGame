using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    //���� ������Ʈ material ������ ���� ������Ƽ���
    MaterialPropertyBlock mpb_emission;
    MaterialPropertyBlock mpb_outLineOption;

    [SerializeField]
    int emission_propertyNameID = -1; // _Emission -> PropertyNameID = 279 , rendererArr[i].sharedMaterial.shader.GetPropertyNameId(rendererArr[i].sharedMaterial.shader.FindPropertyIndex("_Emission"));
    [SerializeField]
    int outLineOption_propertyNameID = -1; // _OutLineOption -> PropertyNameID = 423, rendererArr[i].sharedMaterial.shader.GetPropertyNameId(rendererArr[i].sharedMaterial.shader.FindPropertyIndex("_OutLineOption"));

    //Color
    public static Vector4 WHITE { get { return new Vector4(0.7f, 0.7f, 0.7f, 1); } }
    public static Vector4 RED { get { return new Vector4(1, 0, 0, 1); } }
    public static Vector4 BLUE { get { return new Vector4(0, 0, 1, 1); } }
    public static Vector4 YELOOW { get { return new Vector4(1, 1, 0, 1); } }
    public static Vector4 PURPLE { get { return new Vector4(1, 0, 1, 1); } }


    void Start()
    {
        mpb_emission = new MaterialPropertyBlock();
        mpb_outLineOption = new MaterialPropertyBlock();
    }

    /// <summary>
    /// �ǰ�ȿ�� WhiteFlash -> shader�� Emission������ ���� : ������
    /// </summary>
    /// <param name="rendererCaches">ĳ���� ���̴� ���� ���� ����Ʈ</param>
    /// <param name="emissionCaches">ĳ���� ���̴� emission ������Ƽ ���� ���� ����Ʈ</param>
    /// <param name="actor">ȣ���� Actor��ü</param>
    public IEnumerator ShowFlash(List<Renderer> rendererCaches, List<Vector4> emissionCaches, Actor actor, Vector4 color)
    {
        //�ڷ�ƾ ���� �÷��� ����
        actor.showWhiteFlash_coroutine_is_running = true;

        //����ó��
        if (rendererCaches.Count <= 0 || (rendererCaches.Count != emissionCaches.Count))
        {
            actor.showWhiteFlash_coroutine_is_running = false;
            yield break;
        }

        OnFlash(rendererCaches, color);

        yield return new WaitForSeconds(0.1f);

        OffFlash(rendererCaches, emissionCaches);

        yield return new WaitForSeconds(0.1f);

        OnFlash(rendererCaches, color);

        yield return new WaitForSeconds(0.1f);

        OffFlash(rendererCaches, emissionCaches);

        //�ڷ�ƾ ���� �÷��� ����
        actor.showWhiteFlash_coroutine_is_running = false;
    }

    /// <summary>
    /// ���̴� ĳ�õ����� �ʱ�ȭ : ������
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
            if (!(rendererArr[i].sharedMaterial.shader.name != "Custom/CustomToon" &&
                rendererArr[i].sharedMaterial.shader.name != "Custom/Lambert_Blinnphong"))
            {
                //propertyNameID �ʱ�ȭ
                if(emission_propertyNameID == -1)
                    emission_propertyNameID = rendererArr[i].sharedMaterial.shader.GetPropertyNameId(rendererArr[i].sharedMaterial.shader.FindPropertyIndex("_Emission"));
                if(outLineOption_propertyNameID == -1)
                    outLineOption_propertyNameID = rendererArr[i].sharedMaterial.shader.GetPropertyNameId(rendererArr[i].sharedMaterial.shader.FindPropertyIndex("_OutLineOption"));

                rendererCaches.Add(rendererArr[i]);
                emissionCaches.Add(rendererArr[i].sharedMaterial.GetVector(emission_propertyNameID));
            }
        }

    }

    /// <summary>
    /// ���̴��� ���� �ǰ� ȿ�� �����ֱ� : ������
    /// </summary>
    /// <param name="rendererCaches">������ ���̴� ���� ����ִ� Renderer ����Ʈ</param>
    void OnFlash(List<Renderer> rendererCaches, Vector4 color)
    {
        //����ó��
        if (rendererCaches.Count <= 0)
            return;

        mpb_emission.SetVector(emission_propertyNameID, color);
        for (int i = 0; i < rendererCaches.Count; i++)
        {
            rendererCaches[i].SetPropertyBlock(mpb_emission);
        }

    }

    /// <summary>
    /// �ǰ� ȿ�� ����� ���� ������ ���̴� ���� �ǵ����� : ������
    /// </summary>
    /// <param name="rendererCaches">������ ���̴� ���� ����ִ� Renderer ����Ʈ</param>
    /// <param name="emissionCaches">������ emission�� ���� ����ִ� ����Ʈ</param>
    public void OffFlash(List<Renderer> rendererCaches, List<Vector4> emissionCaches)
    {
        //����ó��
        if (rendererCaches.Count <= 0 || (rendererCaches.Count != emissionCaches.Count))
            return;

        for (int i = 0; i < rendererCaches.Count; i++)
        {
            mpb_emission.SetVector(emission_propertyNameID, emissionCaches[i]);
            rendererCaches[i].SetPropertyBlock(mpb_emission);
        }

    }

    /// <summary>
    /// �ܰ��� �ɼ� ���� : ������
    /// </summary>
    /// <param name="rendererCaches">������ ���̴� ���� ����ִ� Renderer ����Ʈ</param>
    /// <param name="outLineOption">outLineOption - 0:���� 1:black 2:color</param>
    public void ChangeOutLineOption(List<Renderer> rendererCaches,int outLineOption)
    {
        //����ó��
        if (rendererCaches.Count <= 0)
            return;

        mpb_outLineOption.SetInt(outLineOption_propertyNameID, outLineOption);
        for (int i = 0; i < rendererCaches.Count; i++)
        {
            rendererCaches[i].SetPropertyBlock(mpb_outLineOption);
        }
    }
}
