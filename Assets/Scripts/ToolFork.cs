using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolFork : Tool {
    private float damage = .5f;
    public float baseDamage = .5f;
    public float attackRange = .5f;

    public bool hasPlant;
    private GameObject plant = null;
    public Collider2D[] fields;
    public static bool pickedUp = false;
    public Sprite alternateSprite;
    public static bool firstFlower = true;
    public AudioSource hitSound;

    private void Awake() {
        damage = baseDamage;
    }

    public override void ActionOnPress() {
        if (!equipted)
            return;
        
        if (!hasPlant) {
            // Attack
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Emeny")) {
                if (Vector2.Distance(transform.position, enemy.transform.position) <= attackRange) {
                    enemy.GetComponent<Enemy>().GetDamage(damage);
                    hitSound.Play();
                    return;
                }
            }

            // Pick up plant
            foreach (GameObject planty in GameObject.FindGameObjectsWithTag("Plant")) {
                if (Vector2.Distance(transform.position, planty.transform.position) <= attackRange) {
                    plant = planty;
                    hasPlant = true;
                    plant.GetComponent<Plant>().active = false;
                    plant.GetComponent<Plant>().field.plantCount--;
                    plant.SetActive(false);
                    ui.ChangeItem(alternateSprite);
                    if (firstFlower) {
                        ui.SetText("You picked up your flower. Place it in a bed before it dies using the left mouse button.",
                            KeyCode.Mouse0);
                        firstFlower = false;
                    }
                    return;
                }
            }
        } else {
            //Place plant
            foreach (Collider2D field in fields) {
                if (field.bounds.Contains(transform.position)) {
                    hasPlant = false;
                    plant.transform.position = transform.position;
                    plant.SetActive(true);
                    Field f = field.gameObject.GetComponent<Field>();
                    plant.GetComponent<Plant>().field = f;
                    plant.GetComponent<Plant>().active = true;
                    f.plantCount++;
                    ui.ChangeItem(sprite);
                    return;
                }
            }
        }
    }

    protected override void OnPickUp() {
        if(pickedUp)
            return;
        
        ui.SetText("This can hurt really bad. It's also useful for moving your plants." +
                   "(Press Enter to hide message)", KeyCode.Return);
        pickedUp = true;
        ui.ChangeItem(hasPlant ? alternateSprite:sprite);
    }
    
    protected override void OnDrop() {
        groundItem.GetComponent<SpriteRenderer>().sprite = hasPlant ? alternateSprite : sprite;
    }

    public override void Upgrade(int number) {
        damage = (number + 1) * baseDamage;
        ui.SetText("Your fork received an upgrade. It deals more damage now. (Press Enter to hide message)", KeyCode.Return);
    }
}
