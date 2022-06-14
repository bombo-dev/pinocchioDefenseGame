using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyButtonPanel : UI_Controller
{
    //로비씬의 플레이어 스크립트
    LobbyPlayer lobbyPlayer;

    enum Buttons
    {
        GameStartButton
    }

    private void Start()
    {
        //로비씬에서 hotelPino 게임오브젝트 찾아서 객체의 LobbyPlayer스크립트 가져오기
        lobbyPlayer = GameObject.Find("hotelPino").GetComponent<LobbyPlayer>();
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));

        
    }

    /// <summary>
    /// 게임시작 버튼 클릭
    /// </summary>
    void OnClickGameStartButton()
    {
        lobbyPlayer.animator.SetBool("gameStart", true);
    }
}
