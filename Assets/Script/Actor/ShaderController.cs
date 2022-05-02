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
    /// <param name="rendererCaches">캐싱할 쉐이더 정보 담을 리스트</param>
    /// <param name="emissionCaches">캐싱할 쉐이더 emission 프로퍼티 정보 담을 리스트</param>
    /// <param name="actor">호출한 Actor객체</param>
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

    /// <summary>
    /// 쉐이더 캐시데이터 초기화
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
    /// <param name="rendererCaches">변경할 쉐이더 정보 담고있는 Renderer 리스트</param>
    /// <param name="emission_propertyNameID">캐싱할 쉐이더 emission 프로퍼티 정보 담을 리스트</param>
    void OnWhiteFlash(List<Renderer> rendererCaches, int emission_propertyNameID)
    {
        //예외처리
        if (rendererCaches.Count <= 0)
            return;

        mpb.SetVector(emission_propertyNameID, new Vector4(1, 1, 1, 1));
        for (int i = 0; i < rendererCaches.Count; i++)
        {
            rendererCaches[i].SetPropertyBlock(mpb);
        }

    }

    /// <summary>
    /// 피격 효과 종료시 원래 값으로 쉐이더 정보 되돌리기 : 김현진
    /// </summary>
    /// <param name="rendererCaches">변경할 쉐이더 정보 담고있는 Renderer 리스트</param>
    /// <param name="emissionCaches">변경할 emission값 정보 담고있는 리스트</param>
    /// <param name="emission_propertyNameID">캐싱할 쉐이더 emission 프로퍼티 정보 담을 리스트</param>
    public void OffWhiteFlash(List<Renderer> rendererCaches, List<Vector4> emissionCaches, int emission_propertyNameID)
    {
        //예외처리
        if (rendererCaches.Count <= 0 || (rendererCaches.Count != emissionCaches.Count))
            return;

        for (int i = 0; i < rendererCaches.Count; i++)
        {
            mpb.SetVector(emission_propertyNameID, emissionCaches[i]);
            rendererCaches[i].SetPropertyBlock(mpb);
        }

    }
}
