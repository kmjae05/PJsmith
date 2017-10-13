using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatData : MonoBehaviour {
    //능력치 계산


    public static StatData instance = null;

    public static Stat stat = new Stat();

    //장비 4개 합산
    public Stat SumEquip(Equipment helmet, Equipment armor, Equipment weapon, Equipment boots)
    {
        stat.strPower = helmet.stat.strPower + armor.stat.strPower + weapon.stat.strPower + boots.stat.strPower;
        stat.attackSpeed = helmet.stat.attackSpeed + armor.stat.attackSpeed + weapon.stat.attackSpeed + boots.stat.attackSpeed;
        stat.focus = helmet.stat.focus + armor.stat.focus + weapon.stat.focus + boots.stat.focus;
        stat.critical = helmet.stat.critical + armor.stat.critical + weapon.stat.critical + boots.stat.critical;
        stat.defPower = helmet.stat.defPower + armor.stat.defPower + weapon.stat.defPower + boots.stat.defPower;
        stat.evaRate = helmet.stat.evaRate + armor.stat.evaRate + weapon.stat.evaRate + boots.stat.evaRate;
        stat.collectSpeed = helmet.stat.collectSpeed + armor.stat.collectSpeed + weapon.stat.collectSpeed + boots.stat.collectSpeed;
        stat.collectAmount = helmet.stat.collectAmount + armor.stat.collectAmount + weapon.stat.collectAmount + boots.stat.collectAmount;

        stat.dps = helmet.stat.dps + armor.stat.dps + weapon.stat.dps + boots.stat.dps;
        return stat;
    }
    //ex) Stat s = SumChrEquip(Player.Play, SumEquip(helmet, armor, weapon, boots));
    //캐릭터 + 장비 합산
    public Stat SumChrEquip(Player.User chr, Stat equipStat)
    {
        stat.strPower = chr.stat.strPower + equipStat.strPower;
        stat.attackSpeed = chr.stat.attackSpeed + equipStat.attackSpeed;
        stat.focus = chr.stat.focus + equipStat.focus;
        stat.critical = chr.stat.critical + equipStat.critical;
        stat.defPower = chr.stat.defPower + equipStat.defPower;
        stat.evaRate = chr.stat.evaRate + equipStat.evaRate;
        stat.collectSpeed = chr.stat.collectSpeed + equipStat.collectSpeed;
        stat.collectAmount = chr.stat.collectAmount + equipStat.collectAmount;

        stat.dps = chr.stat.dps + equipStat.dps;
        return stat;
    }
    public Stat SumChrEquip(Mercenary chr, Stat equipStat)
    {
        stat.strPower = chr.stat.strPower + equipStat.strPower;
        stat.attackSpeed = chr.stat.attackSpeed + equipStat.attackSpeed;
        stat.focus = chr.stat.focus + equipStat.focus;
        stat.critical = chr.stat.critical + equipStat.critical;
        stat.defPower = chr.stat.defPower + equipStat.defPower;
        stat.evaRate = chr.stat.evaRate + equipStat.evaRate;
        stat.collectSpeed = chr.stat.collectSpeed + equipStat.collectSpeed;
        stat.collectAmount = chr.stat.collectAmount + equipStat.collectAmount;

        stat.dps = chr.stat.dps + equipStat.dps;

        return stat;
    }


    //대장장이 + 장비 합산 + 용병 합산
    public Stat SumChrEquipMer(Player.User player, Stat equipStat, Stat mer)
    {
        stat.strPower = player.stat.strPower + equipStat.strPower + mer.strPower;
        stat.attackSpeed = player.stat.attackSpeed + equipStat.attackSpeed + mer.attackSpeed;
        stat.focus = player.stat.focus + equipStat.focus + mer.focus;
        stat.critical = player.stat.critical + equipStat.critical + mer.critical;
        stat.defPower = player.stat.defPower + equipStat.defPower + mer.defPower;
        stat.evaRate = player.stat.evaRate + equipStat.evaRate + mer.evaRate;
        stat.collectSpeed = player.stat.collectSpeed + equipStat.collectSpeed + mer.collectSpeed;
        stat.collectAmount = player.stat.collectAmount + equipStat.collectAmount + mer.collectAmount;

        stat.dps = player.stat.dps + equipStat.dps + mer.dps;

        return stat;
    }



    //
}


public class Stat
{
    public float dps;
    public float strPower;
    public float attackSpeed;
    public float focus;
    public float critical;
    public float defPower;
    public float evaRate;
    public float collectSpeed;
    public float collectAmount;
}