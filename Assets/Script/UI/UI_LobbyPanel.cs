using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyPanel : UI_Controller
{
    //로비씬의 플레이어 스크립트
    LobbyPlayer lobbyPlayer;

    enum Buttons
    {
        GameStartButton,    //게임시작버튼
        TurretSelectLeftArrowButton,    //터렛 선택 스크롤 <버튼
        TurretSelectRightArrowButton,    //터렛 선택 스크롤 >버튼
        BackLobbyMenuButton //로비 메뉴로 돌아가기 버튼
    }

    enum GameObjects
    {
        LobbyMenuPanel,
        GameSettingPanel,
        TurretSelectScrollView
    }

    private void Update()
    {
        //UI 업데이트
        UpdateUI();
    }
    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        //로비 메뉴패널 UI 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.GameStartButton).gameObject, OnClickGameStartButton, Define.UIEvent.Click);

        //터렛 선택패널 UI 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.BackLobbyMenuButton).gameObject, OnClickBackLobbyMenu, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectRightArrowButton).gameObject, OnClickRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectLeftArrowButton).gameObject, OnClickLeftArrow, Define.UIEvent.Click);

        //로비씬에서 hotelPino 게임오브젝트 찾아서 객체의 LobbyPlayer스크립트 가져오기
        lobbyPlayer = GameObject.FindObjectOfType<LobbyPlayer>();

        //GameSetting UI 끄기
        GetGameobject((int)GameObjects.GameSettingPanel).SetActive(false);
    }

    /// <summary>
    /// 게임시작 버튼 클릭후 연출 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickGameStartButton(PointerEventData data)
    {
        //Lobby UI닫기
        GetGameobject((int)GameObjects.LobbyMenuPanel).SetActive(false);

        //플레이어 연출 애니메이션
        lobbyPlayer.animator.SetBool("gameStart", true);

        //카메라 연출 애니메이션
        lobbyPlayer.camAnimator.Play("CamAnim");
    }

    /// <summary>
    /// 게임시작 버튼 클릭후 연출 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickBackLobbyMenu(PointerEventData data)
    {
        //Lobby UI닫기
        GetGameobject((int)GameObjects.GameSettingPanel).SetActive(false);

        //플레이어 연출 애니메이션
        lobbyPlayer.animator.SetBool("gameStart", false);
        lobbyPlayer.animator.Play("Idle");

        //카메라 연출 애니메이션
        lobbyPlayer.camAnimator.Play("CamBackAnim");
    }

    /// <summary>
    /// UI 업데이트 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void UpdateUI()
    {
        //카메라 연출이 종료 되었을 경우 GameSetting UI생성 - LoobyMenu -> GameSetting
        if (lobbyPlayer.camAnimator.GetBool("finCameraWalk"))
        {
            //GameSetting UI 켜기
            GetGameobject((int)GameObjects.GameSettingPanel).SetActive(true);

            //한번만 실행되게
            lobbyPlayer.camAnimator.SetBool("finCameraWalk",false);
        }

        //카메라 연출이 종료 되었을 경우 LoobyMenu UI생성 - GameSetting -> LoobyMenu
        if (lobbyPlayer.camAnimator.GetBool("finCameraBack"))
        {
            //GameSetting UI 켜기
            GetGameobject((int)GameObjects.LobbyMenuPanel).SetActive(true);

            //한번만 실행되게
            lobbyPlayer.camAnimator.SetBool("finCameraBack", false);
        }
    }

    /// <summary>
    /// 터렛 선택 스크롤 오른쪽 끝으로 이동 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickRightArrow(PointerEventData data)
    {
        GetGameobject((int)GameObjects.TurretSelectScrollView).GetComponent<ScrollRect>().normalizedPosition = new Vector2(1, 0);
    }

    /// <summary>
    /// 터렛 선택 스크롤 왼쪽 끝으로 이동 : 김현진
    /// </summary>
    void OnClickLeftArrow(PointerEventData data)
    {
        GetGameobject((int)GameObjects.TurretSelectScrollView).GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }
}
