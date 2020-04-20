using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float speed = 1;
    private Rigidbody2D rb;
    private Vector2 shortestPlant;
    public float health = 1;
    public float damage = 0.01f;
    public Animator anim;
    
    void Start() {
        // rb = GetComponent<Rigidbody2D>();
        speed += Random.Range(-0.2f, 0.05f);
    }

    // Update is called once per frame
    void Update() {
        if(health <= 0)
            Destroy(gameObject);
        
        FindPlant();

        Vector2 myPos = transform.position;
        if (Vector2.Distance(myPos, shortestPlant) < 0.45) {
            anim.SetBool("moving", false);
        } else {
            anim.SetBool("moving", true);
            transform.position = Vector2.MoveTowards(new Vector2(myPos.x, myPos.y),
                shortestPlant, speed * Time.deltaTime);
        }
    }

    public void GetDamage(float amount) {
        health -= amount;
    }

    private void FindPlant() {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
        Vector2 myPos = transform.position;
        if (plants.Length < 1) {
            shortestPlant = myPos;
            return;
        }
        shortestPlant = plants[0].transform.position;
        float shortestDistance = Vector2.Distance(myPos, shortestPlant);
        for (int i = 1; i < plants.Length; i++) {
            if (Vector2.Distance(myPos, plants[i].transform.position) <
                shortestDistance) {
                shortestPlant = plants[i].transform.position;
                shortestDistance = Vector2.Distance(myPos, shortestPlant);
                
            }
        }
        Vector2 dir = shortestPlant - myPos;
        float angle = Mathf.Tan(Mathf.Atan2(dir.y, dir.x));
        bool td = angle > 1;
        if (!td && shortestPlant.x > myPos.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else {
            transform.localScale = new Vector3(1, 1, 1);
        }
        anim.SetBool("td", td);
        
    }
}
