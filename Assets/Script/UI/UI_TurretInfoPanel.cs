using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class UI_TurretInfoPanel : UI_Controller
{
    public string filePath;

    const int TURRETSMOKEEFFECT = 3;

    const int MAXCOLORWOOD = 6;   //최대 나무 수

    const float BUFFDURATIONTIME = 10f;   //버프 지속시간

    bool isBind = false;

    [SerializeField]
    Sprite[] turretSprite;  //터렛 이미지 모음

    [SerializeField]
    Sprite emptySprite;   //빈 이미지

    enum Buttons
    {
        ColorWoodButton0,//0~
        ColorWoodButton1,
        ColorWoodButton2,
        ColorWoodButton3, 
        ColorWoodButton4, 
        ColorWoodButton5, //~5
        CacelTurretButton,
        DestroyTurretButton
    }

    enum Images
    {
        TurretInfoImage
    }

    enum TextMeshProUGUIs
    {
        ColorWoodText0,//0~
        ColorWoodText1,
        ColorWoodText2,
        ColorWoodText3,
        ColorWoodText4,
        ColorWoodText5, //~5
        ColorWoodNumText,    //강화시 소비되는 ColorWood표시
        HpPointText, 
        PowerPointText,
        AttackSpeedPointText,
        RegenerationPointText,
        DefensePointText,
        RangePointText,
        TargetPointText 
    }

    enum Gameobjects
    {
        ColorWoodEmpty,
        TurretStatePanel,
        IsConstructionPanel
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<GameObject>(typeof(Gameobjects));

        isBind = true;//바인드 완료

        Reset();

        //터렛 강화 버튼 이벤트 추가
        for (int i = 0; i < MAXCOLORWOOD; i++)
        {
            AddUIEvent(GetButton(i).gameObject, i, AddBuffTurret, Define.UIEvent.Click);
        }

        //터렛 파괴 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.DestroyTurretButton).gameObject, OnClickDestroyTurretButton, Define.UIEvent.Click);
        //공사중 파괴 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.CacelTurretButton).gameObject, OnClickCancelButton, Define.UIEvent.Click);
    }

    /// <summary>
    /// 터렛정보 UI 최신정보로 업데이트 : 김현진
    /// </summary>
    /// <param name="updateAllState">false면 피격,강화 state만 갱신, true면 모든정보 갱신</param>
    public void Reset(bool updateAllState = true, bool updateBuffTextColor = true)
    {
        //바인드가 아직 안된 상태
        if (!isBind)
            return;

        GameObject nestGo = SystemManager.Instance.InputManager.currenstSelectNest;

        //공사중일때, 공사완료상태일때 UI구분
        if (nestGo)
        {
            //둥지에 아무것도 없을 경우
            if (!nestGo.GetComponent<Nest>().construction && !nestGo.GetComponent<Nest>().haveTurret)
            {
                TurretInfoPanelClear();

                return;
            }
            //공사중일 경우
            else if (nestGo.GetComponent<Nest>().construction)
            {
                ConstructionTurret constructionTurret = nestGo.GetComponent<Nest>().turret.GetComponent<ConstructionTurret>();

                //이미지 정보 갱신
                GetImage((int)Images.TurretInfoImage).sprite = turretSprite[constructionTurret.currentSelectedTurretIdx];

                if (GetGameobject((int)Gameobjects.ColorWoodEmpty).activeSelf)
                    GetGameobject((int)Gameobjects.ColorWoodEmpty).SetActive(false);
                if (GetGameobject((int)Gameobjects.TurretStatePanel).activeSelf)
                    GetGameobject((int)Gameobjects.TurretStatePanel).SetActive(false);
                if (!GetGameobject((int)Gameobjects.IsConstructionPanel).activeSelf)
                    GetGameobject((int)Gameobjects.IsConstructionPanel).SetActive(true);
                if (!GetButton((int)Buttons.CacelTurretButton).gameObject.activeSelf)
                    GetButton((int)Buttons.CacelTurretButton).gameObject.SetActive(true);
                if (GetButton((int)Buttons.DestroyTurretButton).gameObject.activeSelf)
                    GetButton((int)Buttons.DestroyTurretButton).gameObject.SetActive(false);

                return;
            }
            //공사 완료된 상태일 경우
            else
            {
                if (!GetGameobject((int)Gameobjects.ColorWoodEmpty).activeSelf)
                    GetGameobject((int)Gameobjects.ColorWoodEmpty).SetActive(true);
                if (!GetGameobject((int)Gameobjects.TurretStatePanel).activeSelf)
                    GetGameobject((int)Gameobjects.TurretStatePanel).SetActive(true);
                if (GetGameobject((int)Gameobjects.IsConstructionPanel).activeSelf)
                    GetGameobject((int)Gameobjects.IsConstructionPanel).SetActive(false);
                if (GetButton((int)Buttons.CacelTurretButton).gameObject.activeSelf)
                    GetButton((int)Buttons.CacelTurretButton).gameObject.SetActive(false);
                if (!GetButton((int)Buttons.DestroyTurretButton).gameObject.activeSelf)
                    GetButton((int)Buttons.DestroyTurretButton).gameObject.SetActive(true);
            }
        }
        else
        {
            TurretInfoPanelClear();
        }

        Turret turret = getTurret();

        //예외처리
        if (!turret)
            return;

        if (updateAllState)
        {
            //이미지 정보 갱신
            if (turret.turretNum < turretSprite.Length)
                GetImage((int)Images.TurretInfoImage).sprite = turretSprite[turret.turretNum];

        }

        if (updateBuffTextColor)
        {
            //텍스트 컬러정보 버프에맞춰 갱신
            for (int i = 0; i < MAXCOLORWOOD; i++)
            {
                //인덱스를 buff로 형변환
                Turret.buff _buffIndex = (Turret.buff)(i + 1);

                if (turret.buffs.ContainsKey(_buffIndex))
                {
                    if (i == MAXCOLORWOOD - 1)
                    {
                        //올스텟 증가 버프일 경우
                        GetTextMeshProUGUI((int)TextMeshProUGUIs.PowerPointText).color = Color.red;
                        GetTextMeshProUGUI((int)TextMeshProUGUIs.DefensePointText).color = Color.red;
                        GetTextMeshProUGUI((int)TextMeshProUGUIs.RegenerationPointText).color = Color.red;
                    }
                    else
                        GetTextMeshProUGUI((int)TextMeshProUGUIs.PowerPointText + i).color = Color.red;
                }
                else
                    GetTextMeshProUGUI((int)TextMeshProUGUIs.PowerPointText + i).color = Color.white;
            }
        }

        //HP 텍스트 정보 갱신
        GetTextMeshProUGUI((int)TextMeshProUGUIs.HpPointText).text= turret.currentHP + "/" + turret.maxHP;
        //공격력 텍스트 정보 갱신
        GetTextMeshProUGUI((int)TextMeshProUGUIs.PowerPointText).text = turret.currentPower.ToString();
        //공격속도 텍스트 정보 갱신
        GetTextMeshProUGUI((int)TextMeshProUGUIs.AttackSpeedPointText).text = (1 / turret.currentAttackSpeed).ToString();
        //방어력 텍스트 정보 갱신
        GetTextMeshProUGUI((int)TextMeshProUGUIs.DefensePointText).text = turret.currentDefense.ToString();
        //사거리 텍스트 정보 갱신
        GetTextMeshProUGUI((int)TextMeshProUGUIs.RangePointText).text = turret.currentRange.ToString();
        //회복력 텍스트 정보 갱신
        GetTextMeshProUGUI((int)TextMeshProUGUIs.RegenerationPointText).text = turret.currentRegeneration.ToString();
        //최대타겟 텍스트 정보 갱신
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TargetPointText).text = turret.attackTargetNum.ToString();

        //Color Wood정보 갱신
        for (int i = 0; i < MAXCOLORWOOD; i++)
        {
            GetTextMeshProUGUI((int)i).text = (turret.turretNum + 1).ToString() + "/" + SystemManager.Instance.ResourceManager.colorWoodResource[i].ToString();

            if (turret.turretNum + 1 > SystemManager.Instance.ResourceManager.colorWoodResource[i])
                GetTextMeshProUGUI((int)i).color = Color.red;
            else
                GetTextMeshProUGUI((int)i).color = Color.white;
        }

        //강화시 소비되는 ColorWood 개수 정보
        GetTextMeshProUGUI((int)TextMeshProUGUIs.ColorWoodNumText).text = "강화시 " + (turret.turretNum + 1).ToString() + "개 소비";

    }

    /// <summary>
    /// 패널 메뉴들을 초기상태로 돌린다 : 김현진
    /// </summary>
    void TurretInfoPanelClear()
    {
        //이미지 정보 갱신
        GetImage((int)Images.TurretInfoImage).sprite = emptySprite;

        if (GetGameobject((int)Gameobjects.ColorWoodEmpty).activeSelf)
            GetGameobject((int)Gameobjects.ColorWoodEmpty).SetActive(false);
        if (GetGameobject((int)Gameobjects.TurretStatePanel).activeSelf)
            GetGameobject((int)Gameobjects.TurretStatePanel).SetActive(false);
        if (GetGameobject((int)Gameobjects.IsConstructionPanel).activeSelf)
            GetGameobject((int)Gameobjects.IsConstructionPanel).SetActive(false);
        if (GetButton((int)Buttons.CacelTurretButton).gameObject.activeSelf)
            GetButton((int)Buttons.CacelTurretButton).gameObject.SetActive(false);
        if (GetButton((int)Buttons.DestroyTurretButton).gameObject.activeSelf)
            GetButton((int)Buttons.DestroyTurretButton).gameObject.SetActive(false);
    }

    /// <summary>
    /// 터렛에 해당 idx에 해당하는 버프를 추가한다 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    /// <param name="idx">추가할 버프의 종류 인덱스</param>
    void AddBuffTurret(PointerEventData data, int idx)
    {
        Turret turret = getTurret();

        //예외처리
        if (!turret)
            return;

        //타워가 Dead상태면 취소
        if (turret.currentHP <= 0)
            return;

        //강화할 자원이 존재하는지 판단
        if (SystemManager.Instance.ResourceManager.colorWoodResource[idx] < (turret.turretNum + 1))
            return;

        //강화 자원 소비
        SystemManager.Instance.ResourceManager.colorWoodResource[idx] -= turret.turretNum + 1;

        turret.AddBebuff(idx + 1, BUFFDURATIONTIME);

        Reset();

        SaveLoad save = new SaveLoad();
        save.SaveUserInfo();
    }

    /// <summary>
    /// 클릭된 둥지로 부터 둥지 위에 소환 되어있는 터렛의 정보를 받아온다 : 김현진
    /// </summary>
    /// <returns></returns>
    Turret getTurret()
    {
        Nest nest = null;
        if (SystemManager.Instance.InputManager.currenstSelectNest)
            nest = SystemManager.Instance.InputManager.currenstSelectNest.GetComponent<Nest>();

        //예외처리
        if (!nest)
            return null;

        if (!nest.turret)
            return null;

         return nest.turret.GetComponent<Turret>();
    }

    /// <summary>
    /// 클릭된 둥지로 부터 둥지 위에 소환 되어있는 터렛의 정보를 받아온다 : 김현진
    /// </summary>
    /// <returns></returns>
    ConstructionTurret getConstructionTurret()
    {
        Nest nest = null;
        if (SystemManager.Instance.InputManager.currenstSelectNest)
            nest = SystemManager.Instance.InputManager.currenstSelectNest.GetComponent<Nest>();

        //예외처리
        if (!nest)
            return null;

        if (!nest.turret)
            return null;

        return nest.turret.GetComponent<ConstructionTurret>();
    }

    /// <summary>
    /// 이미 공사가 완료된 터렛을 파괴 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickDestroyTurretButton(PointerEventData data)
    {
        //터렛정보
        Turret turret = getTurret();

        //둥지 정보
        Nest nest = null;
        if (SystemManager.Instance.InputManager.currenstSelectNest)
            nest = SystemManager.Instance.InputManager.currenstSelectNest.GetComponent<Nest>();

        //예외처리
        if (!turret || !nest)
            return;

        //타워가 Dead상태면 취소
        if (turret.currentHP <= 0)
            return;

        //파괴 이펙트 출력
        SystemManager.Instance.EffectManager.EnableEffect(TURRETSMOKEEFFECT, turret.hitPos.transform.position);

        //터렛 파괴
        turret.DecreaseHP(99999);
    }

    /// <summary>
    /// 공사중인 터렛을 파괴 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickCancelButton(PointerEventData data)
    {
        ConstructionTurret turret = getConstructionTurret();

        //파괴 이펙트 출력
        SystemManager.Instance.EffectManager.EnableEffect(TURRETSMOKEEFFECT, turret.transform.position);

        //터렛 파괴
        turret.CancelConstruction();
    }
}
