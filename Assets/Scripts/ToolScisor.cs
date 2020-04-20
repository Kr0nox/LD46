using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolScisor : Tool {
    public bool hasPlant = false;
    public Collider2D[] fields;
    public float attackRange = 1;
    public GameObject plantPrefab;
    public Transform plantParent;
    public static bool wasPicked;
    public static bool firstCut = true;
    public Sprite alternateSprite;
    public AudioSource cutSound;

    public override void ActionOnPress() {
        if (!hasPlant) {
            // Cut Plant
            foreach (GameObject plant in GameObject.FindGameObjectsWithTag("Plant")) {
                if (plant.GetComponent<Plant>().size > 10 && 
                    Vector2.Distance(transform.position, plant.transform.position) <= attackRange) {
                    plant.GetComponent<Plant>().size = 1;
                    hasPlant = true;

                    if (firstCut) {
                        ui.SetText("You can plant the sapling on any field you like by pressing the left mouse button.",
                            KeyCode.Mouse0);
                        firstCut = false;
                    }
                    ui.ChangeItem(alternateSprite);
                    cutSound.Play();
                    return;
                }
            }
        } else {
            // Place plant
            foreach (Collider2D field in fields) {
                if (field.bounds.Contains(transform.position)) {
                    hasPlant = false;
                    GameObject plant = Instantiate(plantPrefab, transform.position, Quaternion.identity);
                    Field f = field.gameObject.GetComponent<Field>();
                    plant.GetComponent<Plant>().field = f;
                    f.plantCount++;
                    plant.transform.SetParent(plantParent);
                    ui.ChangeItem(sprite);
                    return;
                }
            }
        }
    }

    protected override void OnPickUp() {
        if(wasPicked)
            return;

        ui.SetText("This tool is used to cut your plant when they grow to big. This is shown by a orange circle around the plant." +
                   "(Press Enter to hide message)", KeyCode.Return);
        wasPicked = true;
        ui.ChangeItem(hasPlant ? alternateSprite:sprite);
    }

    protected override void OnDrop() {
        groundItem.GetComponent<SpriteRenderer>().sprite = hasPlant ? alternateSprite : sprite;
    }
}
