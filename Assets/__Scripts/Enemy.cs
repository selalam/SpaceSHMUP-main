using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    public float powerUpDropChance = 1f;
    public ScoreCounter scoreCounter;

    protected bool calledShipDestroyed = false;
    protected BoundsCheck bndCheck;

    void Awake() {
        bndCheck = GetComponent<BoundsCheck>();
        
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreCounter = scoreGO.GetComponent<ScoreCounter>();
    }

    public Vector3 pos {
        get {
            return this.transform.position;
        }
        set {
            this.transform.position = value;
        }
    }
    
    void Update()
    {
        Move();

        if(bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown)){
            Destroy(gameObject);
        }
    }

    public virtual void Move() {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll) {
        GameObject otherGO = coll.gameObject;

        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if(p != null){
            if(bndCheck.isOnScreen){
                health -= Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;
                if(health <= 0){
                    if(!calledShipDestroyed){
                        calledShipDestroyed = true;
                        Main.SHIP_DESTROYED(this);

                        scoreCounter.score += score;
                    }
                    Destroy(this.gameObject);
                }
            }
            Destroy(otherGO);
        } else {
            print("Enemy hit by non-ProjectileHero: " + otherGO.name);
        }
    }
}
