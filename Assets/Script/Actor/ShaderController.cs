using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    //개별 오브젝트 material 수정을 위한 프로퍼티블록
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
    /// 피격효과 WhiteFlash -> shader의 Emission변수값 수정 : 김현진
    /// </summary>
    /// <param name="rendererCaches">캐싱할 쉐이더 정보 담을 리스트</param>
    /// <param name="emissionCaches">캐싱할 쉐이더 emission 프로퍼티 정보 담을 리스트</param>
    /// <param name="actor">호출한 Actor객체</param>
    public IEnumerator ShowFlash(List<Renderer> rendererCaches, List<Vector4> emissionCaches, Actor actor, Vector4 color)
    {
        //코루틴 실행 플래그 갱신
        actor.showWhiteFlash_coroutine_is_running = true;

        //예외처리
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

        //코루틴 종료 플래그 갱신
        actor.showWhiteFlash_coroutine_is_running = false;
    }

    /// <summary>
    /// 쉐이더 캐시데이터 초기화 : 김현진
    /// </summary>
    /// <param name="rendererArr">객체 전체 Renderer</param>
    /// <param name="rendererCaches">캐싱할 쉐이더 정보 담을 리스트</param>
    /// <param name="emissionCaches">캐싱할 쉐이더 emission 프로퍼티 정보 담을 리스트</param>
    public void InitializeShaderCaches(Renderer[] rendererArr, List<Renderer> rendererCaches , List<Vector4> emissionCaches)
    { 
        //예외처리
        if (rendererArr.Length <= 0 || (rendererCaches.Count != emissionCaches.Count))
            return;

        //수정용 쉐이더 정보 캐싱
        for (int i = 0; i < rendererArr.Length; i++)
        {
            if (!(rendererArr[i].sharedMaterial.shader.name != "Custom/CustomToon" &&
                rendererArr[i].sharedMaterial.shader.name != "Custom/Lambert_Blinnphong"))
            {
                //propertyNameID 초기화
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
    /// 쉐이더를 통해 피격 효과 보여주기 : 김현진
    /// </summary>
    /// <param name="rendererCaches">변경할 쉐이더 정보 담고있는 Renderer 리스트</param>
    void OnFlash(List<Renderer> rendererCaches, Vector4 color)
    {
        //예외처리
        if (rendererCaches.Count <= 0)
            return;

        mpb_emission.SetVector(emission_propertyNameID, color);
        for (int i = 0; i < rendererCaches.Count; i++)
        {
            rendererCaches[i].SetPropertyBlock(mpb_emission);
        }

    }

    /// <summary>
    /// 피격 효과 종료시 원래 값으로 쉐이더 정보 되돌리기 : 김현진
    /// </summary>
    /// <param name="rendererCaches">변경할 쉐이더 정보 담고있는 Renderer 리스트</param>
    /// <param name="emissionCaches">변경할 emission값 정보 담고있는 리스트</param>
    public void OffFlash(List<Renderer> rendererCaches, List<Vector4> emissionCaches)
    {
        //예외처리
        if (rendererCaches.Count <= 0 || (rendererCaches.Count != emissionCaches.Count))
            return;

        for (int i = 0; i < rendererCaches.Count; i++)
        {
            mpb_emission.SetVector(emission_propertyNameID, emissionCaches[i]);
            rendererCaches[i].SetPropertyBlock(mpb_emission);
        }

    }

    /// <summary>
    /// 외곽선 옵션 변경 : 김현진
    /// </summary>
    /// <param name="rendererCaches">변경할 쉐이더 정보 담고있는 Renderer 리스트</param>
    /// <param name="outLineOption">outLineOption - 0:없음 1:black 2:color</param>
    public void ChangeOutLineOption(List<Renderer> rendererCaches,int outLineOption)
    {
        //예외처리
        if (rendererCaches.Count <= 0)
            return;

        mpb_outLineOption.SetInt(outLineOption_propertyNameID, outLineOption);
        for (int i = 0; i < rendererCaches.Count; i++)
        {
            rendererCaches[i].SetPropertyBlock(mpb_outLineOption);
        }
    }
}
