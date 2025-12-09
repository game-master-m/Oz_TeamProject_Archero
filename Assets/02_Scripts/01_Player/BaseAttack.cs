using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    [SerializeField] private Projectile mArrowProjectilePrefab;
    [SerializeField] private Transform mArrowSpawnPoint;

    public void ArrowAttack()
    {
        //모션끝날때 화살생김
        Instantiate(mArrowProjectilePrefab, mArrowSpawnPoint.position, mArrowSpawnPoint.rotation);

        //모션끝날때 적을 처다봐야댐

        //화살이 자동으로 적을 향해 날아가야댐



        //float cur = anim.GetFloat("AttackSpeed");
        //float next = cur + 1f;

        //anim.SetFloat("AttackSpeed", next);
    }
}
