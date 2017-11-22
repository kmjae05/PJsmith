using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatData : MonoBehaviour {
    //능력치 계산

    public static StatData instance = null;
    
    private static Stat playerRepreStat = new Stat();       //대장장이 대표 스탯
    private static List<Stat> playerStat;         //대장장이 스탯
    private List<Stat> mercenaryRepreStat; //용병 대표 스탯
    private List<Stat> mercenary1Stat;       //용병1 스탯
    private List<Stat> mercenary2Stat;       //용병2 스탯
    private List<Stat> mercenary3Stat;       //용병3 스탯
    private Stat repreSetStat = new Stat();  //대표 세트 스탯
    //private Stat set1Stat = new Stat();      //세트1 스탯
    //private Stat set2Stat = new Stat();      //세트2 스탯

    MercenaryData mercenaryData;


    private void Start()
    {
        mercenaryData = GameObject.Find("MercenaryData").GetComponent<MercenaryData>();
        playerStat = new List<Stat>();
        for (int i = 0; i < 2; i++) playerStat.Add(new Stat());
        mercenaryRepreStat = new List<Stat>();
        for (int i = 0; i < 3; i++) mercenaryRepreStat.Add(new Stat());
        mercenary1Stat = new List<Stat>(); mercenary2Stat = new List<Stat>(); mercenary3Stat = new List<Stat>();
        for (int i = 0; i < 2; i++) { mercenary1Stat.Add(new Stat()); mercenary2Stat.Add(new Stat()); mercenary3Stat.Add(new Stat()); }

        playerStatCal();
        mercenaryStatCal();
        repreSetStatCal();

    }

    //대장장이 스탯 대표 세트 계산
    public void playerRepreStatCal()
    {
        Stat equipStat = new Stat();
        equipStat = SumEquip(Player.instance.getUser().equipHelmet[SetSlotData.instance.getRepreSet() - 1], Player.instance.getUser().equipArmor[SetSlotData.instance.getRepreSet() - 1],
            Player.instance.getUser().equipWeapon[SetSlotData.instance.getRepreSet() - 1], Player.instance.getUser().equipBoots[SetSlotData.instance.getRepreSet() - 1]
            , Player.instance.getUser().equipGloves[SetSlotData.instance.getRepreSet() - 1], Player.instance.getUser().equipPants[SetSlotData.instance.getRepreSet() - 1]);
        playerRepreStat = SumChrEquip(Player.instance.getUser(), equipStat);
    }
    //대표 세트 계산
    public void repreSetStatCal()
    {
        playerRepreStatCal();
        repreSetStat.statClear();
        Stat[] merEquipStat = new Stat[3]; //용병 장비 스탯       
        for (int i = 0; i < merEquipStat.Length; i++)
        {
            Mercenary mer = mercenaryData.getMercenary()[i];
            merEquipStat[i] = SumEquip(mer.equipHelmet[SetSlotData.instance.getRepreSet() - 1], mer.equipArmor[SetSlotData.instance.getRepreSet() - 1],
                mer.equipWeapon[SetSlotData.instance.getRepreSet() - 1], mer.equipBoots[SetSlotData.instance.getRepreSet() - 1]
                , mer.equipGloves[SetSlotData.instance.getRepreSet() - 1], mer.equipPants[SetSlotData.instance.getRepreSet() - 1] );
            //용병 스탯
            mercenaryRepreStat[i] = SumChrEquip(mer, merEquipStat[i]);
        }
        for (int i = 0; i < merEquipStat.Length; i++)
        {
            repreSetStat = SumStat(repreSetStat, mercenaryRepreStat[i]);
        }
        repreSetStat = SumStat(repreSetStat, playerRepreStat);
    }
    //대장장이 스탯 계산
    public void playerStatCal()
    {
        for (int i = 0; i < playerStat.Count; i++)
        {
            Stat equipStat = new Stat();
            equipStat = SumEquip(Player.instance.getUser().equipHelmet[i], Player.instance.getUser().equipArmor[i],
                Player.instance.getUser().equipWeapon[i], Player.instance.getUser().equipBoots[i]
                , Player.instance.getUser().equipGloves[i], Player.instance.getUser().equipPants[i]);
            playerStat[i] = SumChrEquip(Player.instance.getUser(), equipStat);
        }
    }
    //용병 스탯 계산
    public void mercenaryStatCal()
    {
        for (int i = 0; i < mercenary1Stat.Count; i++)
        {
            Mercenary mer = mercenaryData.getMercenary()[0];
            Stat merEquipStat = new Stat(); //용병 장비 스탯       
            merEquipStat = SumEquip(mer.equipHelmet[i], mer.equipArmor[i], mer.equipWeapon[i], mer.equipBoots[i], mer.equipGloves[i], mer.equipPants[i]);
            mercenary1Stat[i] = SumChrEquip(mer, merEquipStat);
        }
        for (int i = 0; i < mercenary2Stat.Count; i++)
        {
            Mercenary mer = mercenaryData.getMercenary()[1];
            Stat merEquipStat = new Stat(); //용병 장비 스탯       
            merEquipStat = SumEquip(mer.equipHelmet[i], mer.equipArmor[i], mer.equipWeapon[i], mer.equipBoots[i], mer.equipGloves[i], mer.equipPants[i]);
            mercenary2Stat[i] = SumChrEquip(mer, merEquipStat);
        }
        for (int i = 0; i < mercenary3Stat.Count; i++)
        {
            Mercenary mer = mercenaryData.getMercenary()[2];
            Stat merEquipStat = new Stat(); //용병 장비 스탯       
            merEquipStat = SumEquip(mer.equipHelmet[i], mer.equipArmor[i], mer.equipWeapon[i], mer.equipBoots[i], mer.equipGloves[i], mer.equipPants[i]);
            mercenary3Stat[i] = SumChrEquip(mer, merEquipStat);
        }
    }




    //장비 6개 합산
    public Stat SumEquip(InventoryThings helmet, InventoryThings armor, InventoryThings weapon, InventoryThings boots, InventoryThings gloves, InventoryThings pants)
    {
        Stat stat = new Stat();
        stat.strPower = helmet.stat.strPower + armor.stat.strPower + weapon.stat.strPower + boots.stat.strPower + gloves.stat.strPower + pants.stat.strPower;
        stat.attackSpeed = helmet.stat.attackSpeed + armor.stat.attackSpeed + weapon.stat.attackSpeed + boots.stat.attackSpeed + gloves.stat.attackSpeed + pants.stat.attackSpeed;
        stat.focus = helmet.stat.focus + armor.stat.focus + weapon.stat.focus + boots.stat.focus + gloves.stat.focus + pants.stat.focus;
        stat.critical = helmet.stat.critical + armor.stat.critical + weapon.stat.critical + boots.stat.critical + gloves.stat.critical + pants.stat.critical;
        stat.defPower = helmet.stat.defPower + armor.stat.defPower + weapon.stat.defPower + boots.stat.defPower + gloves.stat.defPower + pants.stat.defPower;
        stat.evaRate = helmet.stat.evaRate + armor.stat.evaRate + weapon.stat.evaRate + boots.stat.evaRate + gloves.stat.evaRate + pants.stat.evaRate;
        //stat.collectSpeed = helmet.stat.collectSpeed + armor.stat.collectSpeed + weapon.stat.collectSpeed + boots.stat.collectSpeed;
        //stat.collectAmount = helmet.stat.collectAmount + armor.stat.collectAmount + weapon.stat.collectAmount + boots.stat.collectAmount;

        stat.dps = stat.strPower*(float)stat.attackSpeed*stat.critical + stat.defPower*stat.evaRate;
        return stat;
    }

    //캐릭터 + 장비 합산
    public Stat SumChrEquip(User chr, Stat equipStat)
    {
        Stat stat = new Stat();
        stat.strPower = chr.stat.strPower + equipStat.strPower;
        stat.attackSpeed = chr.stat.attackSpeed + equipStat.attackSpeed;
        stat.focus = chr.stat.focus + equipStat.focus;
        stat.critical = chr.stat.critical + equipStat.critical;
        stat.defPower = chr.stat.defPower + equipStat.defPower;
        stat.evaRate = chr.stat.evaRate + equipStat.evaRate;
        //stat.collectSpeed = chr.stat.collectSpeed + equipStat.collectSpeed;
        //stat.collectAmount = chr.stat.collectAmount + equipStat.collectAmount;

        stat.dps = stat.strPower * (float)stat.attackSpeed * stat.critical + stat.defPower * stat.evaRate;
        return stat;
    }
    public Stat SumChrEquip(Mercenary chr, Stat equipStat)
    {
        Stat stat = new Stat();
        stat.strPower = chr.stat.strPower + equipStat.strPower;
        stat.attackSpeed = chr.stat.attackSpeed + equipStat.attackSpeed;
        stat.focus = chr.stat.focus + equipStat.focus;
        stat.critical = chr.stat.critical + equipStat.critical;
        stat.defPower = chr.stat.defPower + equipStat.defPower;
        stat.evaRate = chr.stat.evaRate + equipStat.evaRate;
        //stat.collectSpeed = chr.stat.collectSpeed + equipStat.collectSpeed;
        //stat.collectAmount = chr.stat.collectAmount + equipStat.collectAmount;

        stat.dps = stat.strPower * (float)stat.attackSpeed * stat.critical + stat.defPower * stat.evaRate;
        return stat;
    }


    //대장장이 + 장비 합산 + 용병 합산
    public Stat SumChrEquipMer(User player, Stat equipStat, Stat mer)
    {
        Stat stat = new Stat();
        stat.strPower = player.stat.strPower + equipStat.strPower + mer.strPower;
        stat.attackSpeed = player.stat.attackSpeed + equipStat.attackSpeed + mer.attackSpeed;
        stat.focus = player.stat.focus + equipStat.focus + mer.focus;
        stat.critical = player.stat.critical + equipStat.critical + mer.critical;
        stat.defPower = player.stat.defPower + equipStat.defPower + mer.defPower;
        stat.evaRate = player.stat.evaRate + equipStat.evaRate + mer.evaRate;
        //stat.collectSpeed = player.stat.collectSpeed + equipStat.collectSpeed + mer.collectSpeed;
        //stat.collectAmount = player.stat.collectAmount + equipStat.collectAmount + mer.collectAmount;

        stat.dps = stat.strPower * (float)stat.attackSpeed * stat.critical + stat.defPower * stat.evaRate;

        return stat;
    }

    //스탯+스탯
    public Stat SumStat(Stat stat1, Stat stat2)
    {
        Stat stat = new Stat();
        stat.strPower = stat1.strPower + stat2.strPower;
        stat.attackSpeed = stat1.attackSpeed  + stat2.attackSpeed;
        stat.focus = stat1.focus+ stat2.focus;
        stat.critical = stat1.critical  + stat2.critical;
        stat.defPower = stat1.defPower + stat2.defPower;
        stat.evaRate = stat1.evaRate + stat2.evaRate;
        //stat.collectSpeed = stat1.collectSpeed+ stat2.collectSpeed;
        //stat.collectAmount = stat1.collectAmount + stat2.collectAmount;

        stat.dps = stat.strPower * (float)stat.attackSpeed * stat.critical + stat.defPower * stat.evaRate;
        return stat;
    }




    public Stat getPlayerRepreStat() { return playerRepreStat; }
    public List<Stat> getPlayerStat() { return playerStat; }
    public Stat getRepreSetStat() { return repreSetStat; }
    public List<Stat> getMercenaryStat(int i)
    {
        if (i==1) return mercenary1Stat;
        else if (i == 2) return mercenary2Stat;
        else if (i == 3) return mercenary3Stat;
        else return null;
    }
}

[Serializable]
public class Stat
{
    public float dps;
    public float strPower;
    public double attackSpeed;
    public float focus;
    public float critical;
    public float defPower;
    public float evaRate;
    public float collectSpeed;
    public float collectAmount;



    public void statClear()
    {
        dps = 0;
        strPower = 0;
        attackSpeed = 0;
        focus = 0;
        critical = 0;
        defPower = 0;
        evaRate = 0;
        collectSpeed = 0;
        collectAmount = 0;
    }
}