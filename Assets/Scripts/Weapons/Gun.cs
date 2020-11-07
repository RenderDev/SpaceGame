using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Ballistics")]
    [Tooltip("Time in seconds between shots.")]
    [SerializeField] private float fireDelay = 0.2f;

    [Tooltip("When \"Aimed\" property is set to true, this is how far off boresight the gun can gimbal to a target set using the \"TargetPosition\" property.")]
    [SerializeField] [Range(0f, 180f)] private float gimbalRange = 10f;

    [Tooltip("Muzzle velocity in m/s of a fired bullet.")]
    [SerializeField] private float muzzleVelocity = 200f;

    [Header("Firing pattern")]
    [Tooltip("Maximum random aim error in degrees.")]
    [SerializeField] private float deviation = 0.1f;

    [Tooltip("If using multiple barrels, should the barrels fire in sequence or all at once.")]
    [SerializeField] private bool sequentialBarrels = false;

    [Tooltip("Reference transform from which bullets will be spawned. Multiple barrels can be assigned. If no barrels are assigned, bullets will come from the GameObject's center.")]
    [SerializeField] private Transform[] barrels = null;

    [Header("Prefabs")]
    [Tooltip("Bullet prefab to fire from the gun.")]
    [SerializeField] private Bullet bulletPrefab = null;

    [Tooltip("Muzzle flash particle system effect to play from the barrel when the gun is fired.")]
    [SerializeField] private ParticleSystem muzzleFlashPrefab = null;

    [Header("Ammo")]
    [Tooltip("When true, the gun will require ammo to fire. One ammo is consumed per shot, no matter how many barrels the gun has.")]
    [SerializeField] private bool useAmmo = false;

    [Tooltip("Maximum amount of ammo carried by the gun. Ammo can be refilled using the \"ReloadAmmo()\" function.")]
    [SerializeField] private int maxAmmo = 300;

    private Dictionary<Transform, ParticleSystem> barrelToMuzzleFlash = new Dictionary<Transform, ParticleSystem>();
    private Queue<Transform> barrelQueue = null;
    private float cooldown = 0f;
    public bool UseGimballedAiming { get; set; }
    public Vector3 TargetPosition { get; set; }
    public bool HasAmmo { get { return !useAmmo || (useAmmo && AmmoCount > 0); } }
    public int AmmoCount { get; private set; }
    public bool ReadyToFire { get { return (cooldown <= 0f) && (HasAmmo); } }
    public float FireDelay { get { return fireDelay; } }
    public float GimbalRange { get { return gimbalRange; } }
    public float MuzzleVelocity { get { return muzzleVelocity; } }
    public float Deviation { get { return deviation; } }
    public Bullet BulletPrefab { get { return bulletPrefab; } }
    public bool UseAmmo { get { return useAmmo; } set { useAmmo = value; } }
    public int MaxAmmo { get { return maxAmmo; } }

    private void Awake()
    {
        AmmoCount = maxAmmo;
        UseGimballedAiming = false;
        barrelQueue = new Queue<Transform>(barrels);
        if (muzzleFlashPrefab != null)
        {
            foreach (var barrel in barrels)
            {
                var muzzleFlash = Instantiate(muzzleFlashPrefab, barrel, false).GetComponent<ParticleSystem>();
                barrelToMuzzleFlash.Add(barrel, muzzleFlash);
            }
        }
    }

    private void Update()
    {
        cooldown = Mathf.MoveTowards(cooldown, 0f, Time.deltaTime);
    }

    public void ReloadAmmo()
    {
        AmmoCount = maxAmmo;
    }

    public void Fire(Vector3 inheritedVelocity)
    {
        if (ReadyToFire == false)
            return;
        if (barrelQueue.Count == 0)
        {
            SpawnAndFireBulletFromBarrel(transform, inheritedVelocity);
        }
        else if (sequentialBarrels)
        {
            var barrel = barrelQueue.Dequeue();
            SpawnAndFireBulletFromBarrel(barrel, inheritedVelocity);
            barrelQueue.Enqueue(barrel);
        }
        else
        {
            foreach (var barrel in barrelQueue)
            {
                SpawnAndFireBulletFromBarrel(barrel, inheritedVelocity);
            }
        }
        if (useAmmo)
            AmmoCount--;
        cooldown = fireDelay;
    }

    private void SpawnAndFireBulletFromBarrel(Transform barrel, Vector3 inheritedVelocity)
    {
        Vector3 spawnPos = barrel.position;
        Quaternion aimRotation = barrel.rotation;
        if (UseGimballedAiming == true)
        {
            Vector3 gimballedVec = transform.forward;
            gimballedVec = Vector3.RotateTowards(gimballedVec,
                TargetPosition - barrel.position,
                Mathf.Deg2Rad * gimbalRange,
                1f);
            gimballedVec.Normalize();
            aimRotation = Quaternion.LookRotation(gimballedVec);
        }
        if (barrelToMuzzleFlash.ContainsKey(barrel))
            barrelToMuzzleFlash[barrel].Play();
        var bullet = Instantiate(bulletPrefab, spawnPos, aimRotation);
        bullet.Fire(spawnPos, aimRotation, inheritedVelocity, muzzleVelocity, deviation);
    }
}
