using UnityEngine;
using UnityEditor;

public class TurretScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turrentRotation;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 2f;
    [SerializeField] private float rotationSpeed = 1000f;
    [SerializeField] private float bps = 1f; //Bullers Per Second

    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        if(target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if(!CheckTargetRange())
        {
            target = null;
        } else {
            timeUntilFire += Time.deltaTime;

            if(timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if(hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - target.position.x)*Mathf.Rad2Deg + 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = Quaternion.RotateTowards(turrentRotation.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
