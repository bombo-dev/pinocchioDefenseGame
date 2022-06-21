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
        ReStartOptionButton,  //다시시작 버튼
        LobbyOptionButton   //로비로 돌아가기 버튼
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

    enum Scrollbars
    {
        BgSoundScrollbar,   //배경사운드 음량 조절
        EfSoundScrollbar,   //효과사운드 음량 조절
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
        Bind<Scrollbar>(typeof(Scrollbars));

        //이벤트 추가
        AddUIEvent(GetButton((int)Buttons.OptionButton).gameObject, OnClickOptionPopUpButton, Define.UIEvent.Click);    //옵션패널 활성화
        AddUIEvent(GetButton((int)Buttons.CloseOptionOpUpPanel).gameObject, OnClickCloseOptionPopUpButton, Define.UIEvent.Click);    //옵션패널 비활성화
        AddUIEvent(GetButton((int)Buttons.BgSoundOptionPanel).gameObject, OnClickBgSoundButton, Define.UIEvent.Click);    //배경음 
        AddUIEvent(GetButton((int)Buttons.EfSoundOptionPanel).gameObject, OnClickEfSoundButton, Define.UIEvent.Click);    //효과음 
        AddUIEvent(GetButton((int)Buttons.ExitOptionButton).gameObject, OnClickExitButton, Define.UIEvent.Click);    //앱종료 

        //씬 별로 버튼 활성화
        OnButton();

        //사운드정보 초기화
        InitializeSoundInfo();

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

    #region 사운드
    /// <summary>
    /// 배경음 켜기/끄기 버튼 동작 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickBgSoundButton(PointerEventData data)
    {
        AudioSource audioSource = SoundManager.Instance.audioSource;

        if (GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text.Equals("켜기"))
        {
            //소리켜기
            audioSource.mute = false;

            //유저정보 업데이트
            SystemManager.Instance.UserInfo.isBgSound = true;

            //UI업데이트
            GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text = "끄기";
            GetImage((int)Images.BgSoundOptionImage).sprite = soundOffSprite;
        }
        else
        {
            //소리끄기
            audioSource.mute = true;

            //유저정보 업데이트
            SystemManager.Instance.UserInfo.isBgSound = false;

            //UI업데이트
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
        List<AudioSource> audioSource = SoundEffectManager.Instance.effectAudioSource;

        if (GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text.Equals("켜기"))
        {
            //소리켜기
            for (int i = 0; i < audioSource.Count; i++)
            {
                audioSource[i].mute = false;     
            }

            //유저정보 업데이트
            SystemManager.Instance.UserInfo.isEfSound = true;

            //UI업데이트
            GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "끄기";
            GetImage((int)Images.EfSoundOptionImage).sprite = soundOffSprite;
        }
        else
        {
            //소리끄기
            for (int i = 0; i < audioSource.Count; i++)
            {
                audioSource[i].mute = true;
            }

            //유저정보 업데이트
            SystemManager.Instance.UserInfo.isEfSound = false;

            //UI업데이트
            GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "켜기";
            GetImage((int)Images.EfSoundOptionImage).sprite = soundOnSprite;
        }
    }

    /// <summary>
    /// 배경사운드 음량 조절 : 김현진
    /// </summary>
    public void OnValueChagneBgSoundSlider()
    {
        float value = GetScrollBar((int)Scrollbars.BgSoundScrollbar).value;
        //음량정보 적용
        SoundManager.Instance.audioSource.volume = value;

        //음량정보 갱신
        SystemManager.Instance.UserInfo.bgSoundVolume = value;
    }

    /// <summary>
    /// 효과사운드 음량 조절 : 김현진
    /// </summary>
    public void OnValueChagneEfSoundSlider()
    {
        float value = GetScrollBar((int)Scrollbars.EfSoundScrollbar).value;
        List<AudioSource> audioSource = SoundEffectManager.Instance.effectAudioSource;

        //음량정보 적용
        for (int i = 0; i < audioSource.Count; i++)
        {
            SoundEffectManager.Instance.effectAudioSource[i].volume = value;    
        }

        //음량정보 갱신
        SystemManager.Instance.UserInfo.efSoundVolume = value;
    }

    /// <summary>
    /// 사운드 관련 정보 초기화 : 김현진
    /// </summary>
    void InitializeSoundInfo()
    {
        AudioSource bgAudioSource = SoundManager.Instance.audioSource;
        List<AudioSource> efAudioSource = SoundEffectManager.Instance.effectAudioSource;
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        //배경음
        bgAudioSource.volume = userInfo.bgSoundVolume;
        GetScrollBar((int)Scrollbars.BgSoundScrollbar).value = userInfo.bgSoundVolume;

        if (userInfo.isBgSound)
        {
            bgAudioSource.mute = false;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text = "끄기";
            GetImage((int)Images.BgSoundOptionImage).sprite = soundOffSprite;
        }
        else
        {
            bgAudioSource.mute = true;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text = "켜기";
            GetImage((int)Images.BgSoundOptionImage).sprite = soundOnSprite;
        }
        //효과음
        GetScrollBar((int)Scrollbars.EfSoundScrollbar).value = userInfo.efSoundVolume;

        for (int i = 0; i < efAudioSource.Count; i++)
        {
            efAudioSource[i].volume = userInfo.efSoundVolume;

            if (userInfo.isEfSound)
            {
                efAudioSource[i].mute = false;

                if (i == 0)
                {
                    GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "끄기";
                    GetImage((int)Images.EfSoundOptionImage).sprite = soundOffSprite;
                }
            }
            else
            {
                efAudioSource[i].mute = true;

                if (i == 0)
                {
                    GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "켜기";
                    GetImage((int)Images.EfSoundOptionImage).sprite = soundOnSprite;
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// 게임씬을 다시 로드 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickReStartButton(PointerEventData data)
    {
        SceneController.Instance.LoadScene(SceneController.Instance.gameSceneName);
    }

    /// <summary>
    /// 로비로 돌아가기 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickLobbyOptionButton(PointerEventData data)
    {
        SceneController.Instance.LoadScene(SceneController.Instance.lobbySceneName);
    }
    #endregion

    /// <summary>
    /// 게임씬일 경우 다시시작 버튼 활성화
    /// </summary>
    public void OnButton()
    {
        //게임씬일 경우 다시하기 옵션 활성화
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            //이벤트추가 - 다시시작
            AddUIEvent(GetButton((int)Buttons.ReStartOptionButton).gameObject, OnClickReStartButton, Define.UIEvent.Click);

            //이벤트추가 - 로비로
            AddUIEvent(GetButton((int)Buttons.LobbyOptionButton).gameObject, OnClickLobbyOptionButton, Define.UIEvent.Click);
        }
    }
}
