using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyButtonPanel : UI_Controller
{
    //�κ���� �÷��̾� ��ũ��Ʈ
    LobbyPlayer lobbyPlayer;

    enum Buttons
    {
        GameStartButton
    }

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));

        AddUIEvent(GetButton((int)Buttons.GameStartButton).gameObject, OnClickGameStartButton, Define.UIEvent.Click);

        //�κ������ hotelPino ���ӿ�����Ʈ ã�Ƽ� ��ü�� LobbyPlayer��ũ��Ʈ ��������
        lobbyPlayer = GameObject.FindObjectOfType<LobbyPlayer>();
    }

    /// <summary>
    /// ���ӽ��� ��ư Ŭ���� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickGameStartButton(PointerEventData data)
    {
        //�÷��̾� ���� �ִϸ��̼�
        lobbyPlayer.animator.SetBool("gameStart", true);

        //ī�޶� ���� �ִϸ��̼�
        lobbyPlayer.camAnimator.Play("Walk");
    }
}
