using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PublicDamageConstans
{
    //속성 데미지 관련 상수

    //화염 데미지 > IDamageable 의 TakeDotDamage에 사용
    public const float FireEffectTime = 3f;                 //화염 데미지 지속시간
    public const float FireDamageTick = 0.2f;               //데미지 부여 주기
    public const float FireDamageDuplicater = 0.2f;         //배율 > 플레이어 공격력 * 데미지 배율

    //독 데미지 > IDamageable 의 TakeDotDamage에 사용
    public const float VenomEffectTime = 9999f;             //독 데미지 지속시간 > 실제로는 대상이 죽을때까지
    public const float VenomDamageTick = 1.0f;              //데미지 부여 주기
    public const float VenomDamageDuplicater = 0.5f;        //배율

    //번개 데미지 > IDamageble 의 TakeDamage에 사용
    public const float LightningDamageDuplicater = 0.3f;    //배율


    //메테오 데미지 배율
    public const float MeteorDamageDuplicater = 1.25f;      //메테오 데미지 배율
}
