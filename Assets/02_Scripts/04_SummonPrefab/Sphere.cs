using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    [SerializeField] private Vector3 mPositionOffset = new Vector3(0, 1.0f, 0);

    // 이번 발사체에서 무시할 충돌체 ID 목록
    private HashSet<int> mIgnoreColliderIDs = new HashSet<int>();

    private PlayerAttack mPlayer;
    private float mRotateSpeed = 100.0f;

    private float mAttackDamage;
    private float mAttackSpeed;

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
        SetUp(mPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up * mRotateSpeed * Time.deltaTime);
    }

    public void SetUp(PlayerAttack attack) 
    {
        this.gameObject.transform.SetParent(attack.gameObject.transform, false);
        this.gameObject.transform.position = attack.gameObject.transform.position + mPositionOffset;

        mAttackSpeed = attack.gameObject.GetComponent<PlayerStat>().AttackSpeed;
        mAttackDamage = attack.gameObject.GetComponent<PlayerStat>().AttackDamage * 0.4f;

        ElementCarrirer[] elementCarriers = GetComponentsInChildren<ElementCarrirer>();
        foreach (ElementCarrirer elementCarrier in elementCarriers) 
        {
           // elementCarrier.SetOwner(this)
        }
    }
}
