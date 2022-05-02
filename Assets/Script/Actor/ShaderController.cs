using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    //개별 오브젝트 material 수정을 위한 프로퍼티블록
    MaterialPropertyBlock mpb;

    public int emission_propertyNameID = 279; // _Emission -> PropertyNameID = 279

    void Start()
    {
        mpb = new MaterialPropertyBlock();
    }

    /// <summary>
    /// 피격효과 WhiteFlash -> shader의 Emission변수값 수정 : 김현진
    /// </summary>
    /// <param name="rendererArr">변경할 쉐이더 정보들 가지고있는 renderer배열</param>
    /// <param name="actor">코루틴 호출한 Actor객체</param>
    /// <returns></returns>
    public IEnumerator ShowWhiteFlash(List<Renderer> rendererCaches, List<Vector4> emissionCaches, Actor actor)
    {
        //코루틴 실행 플래그 갱신
        actor.showWhiteFlash_coroutine_is_running = true;

        //예외처리
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

        //코루틴 종료 플래그 갱신
        actor.showWhiteFlash_coroutine_is_running = false;
    }


    public void InitializeShaderCaches(Renderer[] rendererArr, List<Renderer> rendererCaches , List<Vector4> emissionCaches)
    {
        //수정용 쉐이더 정보 캐싱
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
    /// 쉐이더를 통해 피격 효과 보여주기 : 김현진
    /// </summary>
    /// <param name="rendererList">피격효과를 보여줄 쉐이더 정보를 가지고 있는 Renderer 리스트</param>
    void OnWhiteFlash(List<Renderer> rendererCaches, int emission_propertyNameID)
    {
        mpb.SetVector(emission_propertyNameID, new Vector4(1, 1, 1, 1));
        for (int i = 0; i < rendererCaches.Count; i++)
        {
            rendererCaches[i].SetPropertyBlock(mpb);
        }

    }

    /// <summary>
    /// 피격 효과 종료시 원래 값으로 쉐이더 정보 되돌리기 : 김현진
    /// </summary>
    /// <param name="rendererList">프로퍼티 값을 원래대로 되돌려줄 쉐이더 정보를 가지고있는 Renderer 리스트</param>
    /// <param name="emissionList">원래대로 되돌려줄 프로퍼티 값을 가지고있는 리스트</param>
    public void OffWhiteFlash(List<Renderer> rendererCaches, List<Vector4> emissionCaches, int emission_propertyNameID)
    {
        for (int i = 0; i < rendererCaches.Count; i++)
        {
            mpb.SetVector(emission_propertyNameID, emissionCaches[i]);
            rendererCaches[i].SetPropertyBlock(mpb);
        }

    }
}
