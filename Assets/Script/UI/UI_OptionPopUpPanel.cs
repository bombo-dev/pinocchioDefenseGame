using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_OptionPopUpPanel : UI_Controller
{
    //사운드 이미지
    [SerializeField]
    Sprite soundOnSprite;
    [SerializeField]
    Sprite soundOffSprite;

    enum Buttons
    {
        OptionButton,   //옵션버튼 팝업
        CloseOptionOpUpPanel,   //옵션팝업 패널 닫기
        ExitOptionButton,   //게임종료
        BgSoundOptionPanel,  //배경음 버튼
        EfSoundOptionPanel,  //효과음 버튼
    }

    enum GameObjects
    {
        touchGuardPanel,    //다른 UI터치방지 패널
        OptionPopUpPanel    //옵션 팝업 패널
    }

    enum TextMeshProUGUIs
    {
        BgSoundOptionText,   //배경음 버튼 텍스트
        EfSoundOptionText,   //효과음 버튼 텍스트
    }

    enum Images
    {
        BgSoundOptionImage,  //배경음 이미지
        EfSoundOptionImage,  //효과음 이미지
    }


    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));

        //이벤트 추가
        AddUIEvent(GetButton((int)Buttons.OptionButton).gameObject, OnClickOptionPopUpButton, Define.UIEvent.Click);    //옵션패널 활성화
        AddUIEvent(GetButton((int)Buttons.CloseOptionOpUpPanel).gameObject, OnClickCloseOptionPopUpButton, Define.UIEvent.Click);    //옵션패널 비활성화
        AddUIEvent(GetButton((int)Buttons.BgSoundOptionPanel).gameObject, OnClickBgSoundButton, Define.UIEvent.Click);    //배경음 
        AddUIEvent(GetButton((int)Buttons.EfSoundOptionPanel).gameObject, OnClickEfSoundButton, Define.UIEvent.Click);    //효과음 
        AddUIEvent(GetButton((int)Buttons.ExitOptionButton).gameObject, OnClickExitButton, Define.UIEvent.Click);    //앱종료 


        //옵션패널 팝업 닫기
        GetGameobject((int)GameObjects.OptionPopUpPanel).SetActive(false);

        //터치가드 닫기
        GetGameobject((int)GameObjects.touchGuardPanel).SetActive(false);
    }

    #region 옵션

    /// <summary>
    /// 옵션 팝업 패널 활성화 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickOptionPopUpButton(PointerEventData data)
    {
        //터치가드 패널 활성화하여 뒤에있는 UI의 터치를 막는다
        GetGameobject((int)GameObjects.touchGuardPanel).SetActive(true);

        //옵션패널 팝업
        GetGameobject((int)GameObjects.OptionPopUpPanel).SetActive(true);
    }

    /// <summary>
    /// 옵션 팝업 패널 활성화 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickCloseOptionPopUpButton(PointerEventData data)
    {
        //옵션패널 팝업 닫기
        GetGameobject((int)GameObjects.OptionPopUpPanel).SetActive(false);

        //터치가드 닫기
        GetGameobject((int)GameObjects.touchGuardPanel).SetActive(false);
    }

    /// <summary>
    /// 앱 종료 버튼 동작 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickExitButton(PointerEventData data)
    {
        //에디터일 경우 플레이모드 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else   //앱종료
            UnityEngine.Application.Quit();
#endif

    }

    /// <summary>
    /// 배경음 켜기/끄기 버튼 동작 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickBgSoundButton(PointerEventData data)
    {
        if (GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text.Equals("켜기"))
        {
            //소리켜기
            GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text = "끄기";
            GetImage((int)Images.BgSoundOptionImage).sprite = soundOffSprite;
        }
        else
        {
            //소리끄기
            GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text = "켜기";
            GetImage((int)Images.BgSoundOptionImage).sprite = soundOnSprite;
        }
    }

    /// <summary>
    /// 효과음 켜기/끄기 버튼 동작 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param> 
    void OnClickEfSoundButton(PointerEventData data)
    {
        if (GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text.Equals("켜기"))
        {
            //소리켜기
            GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "끄기";
            GetImage((int)Images.EfSoundOptionImage).sprite = soundOffSprite;
        }
        else
        {
            //소리끄기
            GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "켜기";
            GetImage((int)Images.EfSoundOptionImage).sprite = soundOnSprite;
        }
    }
    #endregion
}
