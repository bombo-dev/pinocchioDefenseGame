using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    //개별 오브젝트 material 수정을 위한 프로퍼티블록
    MaterialPropertyBlock mpb;

    public bool showWhiteFlash_coroutine_is_running = false;//코루틴 실행중 여부 플래그

    void Start()
    {
        mpb = new MaterialPropertyBlock();
    }

    /// <summary>
    /// 피격효과 WhiteFlash -> shader의 Emission변수값 수정 : 김현진
    /// </summary>
    public IEnumerator ShowWhiteFlash(Renderer[] rendererArr)
    {
        //코루틴 실행 플래그 갱신
        showWhiteFlash_coroutine_is_running = true;

        // _Emission -> PropertyNameID = 279
        int emission_propertyNameID = 279;

        List<Renderer> rendererList = new List<Renderer>();
        List<Vector4> emissionList = new List<Vector4>();

        //쉐이더의 _Emission변수가 null이 아닌 rendererList구성
        for (int i = 0; i < rendererArr.Length; i++)
        {
            if (!(rendererArr[i].sharedMaterial.shader.name != "Custom/CustomToon" && rendererArr[i].sharedMaterial.shader.name != "Custom/Lambert_BlinnphongEmission" &&
                rendererArr[i].sharedMaterial.shader.name != "Custom/Lambert_Blinnphong"))
            {
                rendererList.Add(rendererArr[i]);
                emissionList.Add(rendererArr[i].sharedMaterial.GetVector(emission_propertyNameID)); // _Emission -> PropertyNameID = 279
            }
        }

        //예외처리
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

        //코루틴 종료 플래그 갱신
        showWhiteFlash_coroutine_is_running = false;
    }

    /// <summary>
    /// 쉐이더를 통해 피격 효과 보여주기 : 김현진
    /// </summary>
    /// <param name="rendererList">피격효과를 보여줄 쉐이더 정보를 가지고 있는 Renderer 리스트</param>
    void OnWhiteFlash(Renderer[] rendererArr, List<Renderer> rendererList, int emission_propertyNameID)
    {
        mpb.SetVector(emission_propertyNameID, new Vector4(1, 1, 1, 1));
        for (int i = 0; i < rendererList.Count; i++)
        {
            rendererArr[i].SetPropertyBlock(mpb);
        }

    }

    /// <summary>
    /// 피격 효과 종료시 원래 값으로 쉐이더 정보 되돌리기 : 김현진
    /// </summary>
    /// <param name="rendererList">프로퍼티 값을 원래대로 되돌려줄 쉐이더 정보를 가지고있는 Renderer 리스트</param>
    /// <param name="emissionList">원래대로 되돌려줄 프로퍼티 값을 가지고있는 리스트</param>
    void OffWhiteFlash(Renderer[] rendererArr, List<Renderer> rendererList, List<Vector4> emissionList, int emission_propertyNameID)
    {
        for (int i = 0; i < rendererList.Count; i++)
        {
            mpb.SetVector(emission_propertyNameID, emissionList[i]);
            rendererArr[i].SetPropertyBlock(mpb);
        }

    }
}
