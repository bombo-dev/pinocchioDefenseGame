using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyButtonPanel : UI_Controller
{
    //�κ���� �÷��̾� ��ũ��Ʈ
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
        //UI ������Ʈ
        UpdateUI();
    }
    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        AddUIEvent(GetButton((int)Buttons.GameStartButton).gameObject, OnClickGameStartButton, Define.UIEvent.Click);

        //�κ������ hotelPino ���ӿ�����Ʈ ã�Ƽ� ��ü�� LobbyPlayer��ũ��Ʈ ��������
        lobbyPlayer = GameObject.FindObjectOfType<LobbyPlayer>();

        //GameSetting UI ����
        GetGameobject((int)GameObjects.TurretSelectScrollView).SetActive(false);
    }

    /// <summary>
    /// ���ӽ��� ��ư Ŭ���� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickGameStartButton(PointerEventData data)
    {
        //Lobby UI�ݱ�
        lobbyPanelAnimator.Play("OffLobbyPanelAnim");

        //�÷��̾� ���� �ִϸ��̼�
        lobbyPlayer.animator.SetBool("gameStart", true);

        //ī�޶� ���� �ִϸ��̼�
        lobbyPlayer.camAnimator.Play("Walk");
    }

    /// <summary>
    /// UI ������Ʈ : ������
    /// </summary>
    void UpdateUI()
    {
        //ī�޶� ������ ���� �Ǿ������ Game Setting UI����
        if (lobbyPlayer.camAnimator.GetBool("finCameraWalk"))
        {
            //GameSetting UI �ѱ�
            GetGameobject((int)GameObjects.TurretSelectScrollView).SetActive(true);

            lobbyPanelAnimator.Play("OnGameSettingPanelAnim");
        }
    }
}
