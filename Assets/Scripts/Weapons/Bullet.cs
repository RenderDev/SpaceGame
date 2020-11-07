using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("Time until the projectile expires.")]
    [SerializeField] private float timeToLive = 5f;

    [Tooltip("Layers to check for collisions.")]
    [SerializeField] private LayerMask hitMask = -1;

    [Tooltip("Effect prefab to spawn when the bullet hits something.")]
    [SerializeField] private ParticleSystem impactFxPrefab = null;

    [Tooltip("How much gravity is applied to the bullet.")]
    [SerializeField] private float gravityModifier = 0f;

    [Tooltip("Bullet will rotate so that it is always aligned with its velocity vector. Recommended if using gravity.")]
    [SerializeField] private bool alignToVelocity = false;

    [Tooltip("Move the bullet only during the fixed update instead of regular update.")]
    [SerializeField] private bool useFixedUpdate = true;

    private Vector3 velocity = Vector3.forward;
    private float destructionTime = 0f;
    private bool isFired = false;
    private const float kVelocityMult = 1f;

    private void Update()
    {
        if (isFired == false)
            return;
        if (Time.time > destructionTime)
            DestroyBullet(transform.position, false);
        else if (useFixedUpdate == false)
            MoveBullet();
    }

    private void FixedUpdate()
    {
        if (isFired == false)
            return;
        if (useFixedUpdate == true)
            MoveBullet();
    }

    public void Fire(Vector3 position, Quaternion rotation, Vector3 inheritedVelocity, float muzzleVelocity, float deviation)
    {
        transform.position = position;
        Vector3 deviationAngle = Vector3.zero;
        deviationAngle.x = Random.Range(-deviation, deviation);
        deviationAngle.y = Random.Range(-deviation, deviation);
        Quaternion deviationRotation = Quaternion.Euler(deviationAngle);
        transform.rotation = rotation * deviationRotation;
        velocity = (transform.forward * muzzleVelocity) + inheritedVelocity;
        destructionTime = Time.time + timeToLive;
        isFired = true;
    }

    public void DestroyBullet(Vector3 position, bool fromImpact)
    {
        if (fromImpact == true && impactFxPrefab != null)
        {
            var impactFx = Instantiate(impactFxPrefab, position, transform.rotation);
            impactFx.Play();
        }
        Destroy(gameObject);
    }

    private void MoveBullet()
    {
        RaycastHit rayHit;
        Ray velocityRay = new Ray(transform.position, velocity.normalized);
        bool rayHasHit = Physics.Raycast(velocityRay, out rayHit, velocity.magnitude * kVelocityMult * Time.deltaTime, hitMask);
        if (rayHasHit == true)
        {
            DestroyBullet(rayHit.point, true);
        }
        else
        {
            transform.Translate(velocity * Time.deltaTime, Space.World);
            velocity += Physics.gravity * gravityModifier * Time.deltaTime;
            if (alignToVelocity == true)
                transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}
