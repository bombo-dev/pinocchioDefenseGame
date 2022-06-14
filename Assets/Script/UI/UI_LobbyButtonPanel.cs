using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyButtonPanel : UI_Controller
{
    //�κ���� �÷��̾� ��ũ��Ʈ
    LobbyPlayer lobbyPlayer;

    enum Buttons
    {
        GameStartButton
    }

    private void Start()
    {
        //�κ������ hotelPino ���ӿ�����Ʈ ã�Ƽ� ��ü�� LobbyPlayer��ũ��Ʈ ��������
        lobbyPlayer = GameObject.Find("hotelPino").GetComponent<LobbyPlayer>();
    }

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));

        
    }

    /// <summary>
    /// ���ӽ��� ��ư Ŭ��
    /// </summary>
    void OnClickGameStartButton()
    {
        lobbyPlayer.animator.SetBool("gameStart", true);
    }
}
