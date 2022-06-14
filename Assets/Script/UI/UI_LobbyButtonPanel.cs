using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyButtonPanel : UI_Controller
{
    //로비씬의 플레이어 스크립트
    LobbyPlayer lobbyPlayer;

    [SerializeField]
    Animator lobbyPanelAnimator;

    enum Buttons
    {
        GameStartButton
    }

    enum GameObjects
    {
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

        AddUIEvent(GetButton((int)Buttons.GameStartButton).gameObject, OnClickGameStartButton, Define.UIEvent.Click);

        //로비씬에서 hotelPino 게임오브젝트 찾아서 객체의 LobbyPlayer스크립트 가져오기
        lobbyPlayer = GameObject.FindObjectOfType<LobbyPlayer>();

        //GameSetting UI 끄기
        GetGameobject((int)GameObjects.TurretSelectScrollView).SetActive(false);
    }

    /// <summary>
    /// 게임시작 버튼 클릭후 연출 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickGameStartButton(PointerEventData data)
    {
        //Lobby UI닫기
        lobbyPanelAnimator.Play("OffLobbyPanelAnim");

        //플레이어 연출 애니메이션
        lobbyPlayer.animator.SetBool("gameStart", true);

        //카메라 연출 애니메이션
        lobbyPlayer.camAnimator.Play("Walk");
    }

    /// <summary>
    /// UI 업데이트 : 김현진
    /// </summary>
    void UpdateUI()
    {
        //카메라 연출이 종료 되었을경우 Game Setting UI생성
        if (lobbyPlayer.camAnimator.GetBool("finCameraWalk"))
        {
            //GameSetting UI 켜기
            GetGameobject((int)GameObjects.TurretSelectScrollView).SetActive(true);

            lobbyPanelAnimator.Play("OnGameSettingPanelAnim");
        }
    }
}
