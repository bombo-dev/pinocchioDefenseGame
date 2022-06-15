using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyPanel : UI_Controller
{
    //�κ���� �÷��̾� ��ũ��Ʈ
    LobbyPlayer lobbyPlayer;

    enum Buttons
    {
        GameStartButton,    //���ӽ��۹�ư
        TurretSelectLeftArrowButton,    //�ͷ� ���� ��ũ�� <��ư
        TurretSelectRightArrowButton,    //�ͷ� ���� ��ũ�� >��ư
        BackLobbyMenuButton //�κ� �޴��� ���ư��� ��ư
    }

    enum GameObjects
    {
        LobbyMenuPanel,
        GameSettingPanel,
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

        //�κ� �޴��г� UI �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.GameStartButton).gameObject, OnClickGameStartButton, Define.UIEvent.Click);

        //�ͷ� �����г� UI �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.BackLobbyMenuButton).gameObject, OnClickBackLobbyMenu, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectRightArrowButton).gameObject, OnClickRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectLeftArrowButton).gameObject, OnClickLeftArrow, Define.UIEvent.Click);

        //�κ������ hotelPino ���ӿ�����Ʈ ã�Ƽ� ��ü�� LobbyPlayer��ũ��Ʈ ��������
        lobbyPlayer = GameObject.FindObjectOfType<LobbyPlayer>();

        //GameSetting UI ����
        GetGameobject((int)GameObjects.GameSettingPanel).SetActive(false);
    }

    /// <summary>
    /// ���ӽ��� ��ư Ŭ���� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickGameStartButton(PointerEventData data)
    {
        //Lobby UI�ݱ�
        GetGameobject((int)GameObjects.LobbyMenuPanel).SetActive(false);

        //�÷��̾� ���� �ִϸ��̼�
        lobbyPlayer.animator.SetBool("gameStart", true);

        //ī�޶� ���� �ִϸ��̼�
        lobbyPlayer.camAnimator.Play("CamAnim");
    }

    /// <summary>
    /// ���ӽ��� ��ư Ŭ���� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickBackLobbyMenu(PointerEventData data)
    {
        //Lobby UI�ݱ�
        GetGameobject((int)GameObjects.GameSettingPanel).SetActive(false);

        //�÷��̾� ���� �ִϸ��̼�
        lobbyPlayer.animator.SetBool("gameStart", false);
        lobbyPlayer.animator.Play("Idle");

        //ī�޶� ���� �ִϸ��̼�
        lobbyPlayer.camAnimator.Play("CamBackAnim");
    }

    /// <summary>
    /// UI ������Ʈ : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void UpdateUI()
    {
        //ī�޶� ������ ���� �Ǿ��� ��� GameSetting UI���� - LoobyMenu -> GameSetting
        if (lobbyPlayer.camAnimator.GetBool("finCameraWalk"))
        {
            //GameSetting UI �ѱ�
            GetGameobject((int)GameObjects.GameSettingPanel).SetActive(true);

            //�ѹ��� ����ǰ�
            lobbyPlayer.camAnimator.SetBool("finCameraWalk",false);
        }

        //ī�޶� ������ ���� �Ǿ��� ��� LoobyMenu UI���� - GameSetting -> LoobyMenu
        if (lobbyPlayer.camAnimator.GetBool("finCameraBack"))
        {
            //GameSetting UI �ѱ�
            GetGameobject((int)GameObjects.LobbyMenuPanel).SetActive(true);

            //�ѹ��� ����ǰ�
            lobbyPlayer.camAnimator.SetBool("finCameraBack", false);
        }
    }

    /// <summary>
    /// �ͷ� ���� ��ũ�� ������ ������ �̵� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickRightArrow(PointerEventData data)
    {
        GetGameobject((int)GameObjects.TurretSelectScrollView).GetComponent<ScrollRect>().normalizedPosition = new Vector2(1, 0);
    }

    /// <summary>
    /// �ͷ� ���� ��ũ�� ���� ������ �̵� : ������
    /// </summary>
    void OnClickLeftArrow(PointerEventData data)
    {
        GetGameobject((int)GameObjects.TurretSelectScrollView).GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }
}
