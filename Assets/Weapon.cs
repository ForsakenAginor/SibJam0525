using FMODUnity;
using NSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] private EventReference _fireSound;
    [SerializeField] private Transform _target;
    [SerializeField] Transform muzzle;

    [SerializeField, Range(0,4)] float dispersion;
    [SerializeField] float range;
    [SerializeField] float speed;
    [SerializeField] UnityEngine.Events.UnityEvent onShot;
    [SerializeField] int damage;
    [SerializeField] Transform visualPrefab;
    List<Projectile> projectiles = new List<Projectile>();
    bool isUpdate;

    void BulletHit(HitData hit)
    {
        Collider target = hit.collider;
        IDamageble dmg = target.GetComponent<IDamageble>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }

        PlayerControll playerHit = target.GetComponentInParent<PlayerControll>();
        if (playerHit) playerHit.RegisterHit();
    }
    
    // Start is called before the first frame update
    public void Fire()
    {
        AudioManager.Instance.PlayOneShot(_fireSound, transform.position);
        Transform visual = null;
        if (visualPrefab)
        {
            visual = Instantiate(visualPrefab);
        }
        onShot?.Invoke();
        muzzle.LookAt(_target.position);
        projectiles.Add(new Projectile(muzzle.position, muzzle.forward, speed, range, BulletHit, visual));
        if (!isUpdate) StartCoroutine(updateProjectiles());
        
    }

    IEnumerator updateProjectiles()
    {
        isUpdate = true;
        while (projectiles.Count > 0)
        {
            projectiles.RemoveAll(P => !P.alive);
            foreach (Projectile p in projectiles) { p.Update(Time.deltaTime); }
            yield return null;
        }
        isUpdate = false;
    }

    class Projectile
    {
        Vector3 velocity;
        Vector3 position;
        Vector3 initialPos;
        Transform visual;
        float dist;
        float maxDist;
        public bool alive { get; private set; }

        System.Action<HitData> onHit;
        public Projectile(Vector3 position, Vector3 direction, float speed, float maxDist, System.Action<HitData> onHit, Transform visual)
        {
            this.position = position;
            this.velocity = direction * speed;
            this.onHit = onHit;
            initialPos = position;
            this.visual = visual;
            this.maxDist = maxDist;
            alive = true;
        }

        void Kill()
        {
            if(visual) Destroy(visual.gameObject);
            alive = false; 
        }

        public void Update(float dt)
        {
            Vector3 nextPos = position + velocity * dt;
            dist += velocity.magnitude * dt;
            if (dist > maxDist) Kill();

            if (Physics.Raycast(position, nextPos-position, out RaycastHit hit, (nextPos - position).magnitude))
            {
                HitData data = new HitData();
                data.point = hit.point;
                data.distance = dist;
                data.normal = hit.normal;
                data.velocity = velocity;
                data.collider = hit.collider;
                Debug.DrawLine(position, hit.point);
                position = hit.point;
                onHit?.Invoke(data);
                Kill();
            }
            else
            {
                Debug.DrawLine(position, nextPos);
                position = nextPos;
            }
            if (visual != null)
            {
                visual.position = position;
                visual.rotation = Quaternion.LookRotation(velocity);
            }
        }
    }


}
