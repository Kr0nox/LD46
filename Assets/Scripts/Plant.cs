using UnityEngine;
using Random = UnityEngine.Random;

public class Plant : MonoBehaviour {
    public GameUIController ui;

    public float water;
    public float health = 1f;
    public float size = 1;
    public Field field;

    public float waterDecay = .01f;
    public float healthGain = .001f;
    public float sizeFactor = 10;

    public float damage = 0;
    public bool active = true;

    public SpriteRenderer glow;
    public Color normal;

    public Color grown;

    private SpriteRenderer myRenderer;
    private bool growing;
    private bool uiShown = false;

    public void Start() {
        water = Random.Range(.4f, .6f);
        ui = FindObjectOfType<GameUIController>();
        growing = false;
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        water = LimitToRange(water - waterDecay * Time.deltaTime * (growing ? 1.5f : 1), 0f, 1f);
        CalculateHealth();
        if (uiShown) {
            ui.SetWater(water);
            ui.SetHealth(health);
            
        }
    }

    private void CalculateHealth() {
        float substract = 0;
        if (field.nutrient <= 0f) {
            substract += .02f;
        } else if (field.nutrient < .1f) {
            substract += (.1f - field.nutrient);
        } else {
            substract -= healthGain;
        }

        if (water <= 0f) {
            substract += .01f;
        } else if(water <= 0.35f) {
            substract += LimitToRange((0.35f - water)/10, 0, 0.006f);
        } else if (water >= .68f) {
            substract += LimitToRange((water - .68f)/30, 0, 0.003f);
        } else {
            substract -= 1.5f * healthGain;
        }
        if (size > 10) {
            if (substract < 0.01f) {
                substract = 0.01f;
            }

            glow.color = grown;
            ui.SetText("One of your Plants has grown. Cut it and plant the saplings to keep it alive.", KeyCode.Mouse0);
        } else {
            glow.color = normal;
        }
        
        

        substract += damage;
        if (!active) {
            substract = 0.004f;
        }

        substract *= Time.deltaTime * (active ? 1 : 0);
        
        health = LimitToRange(health- substract, 0f, 1f);
        if (health <= 0f) {
            string reason = "death";
            if (water < 0.35f) {
                reason = "dehydration";
            } else if (water > .68f) {
                reason = "over hydration";
            } else if (field.nutrient < .1f) {
                reason = "low nutrient";
            } else if (damage > 0) {
                reason = "weed";
            }
            FindObjectOfType<GameController>().GameOver(reason);
            this.enabled = false;
        }

        if (health >= .8f) {
            if (!growing) {
                field.growCount++;
                // glow.color = normal;
            }
            // myRenderer.color = Color.HSVToRGB(0, 0, 1);
            growing = true;
            size += health / sizeFactor * Time.deltaTime;
        } else {
            if (growing) {
                field.growCount--;
                // glow.color = new Color(0,0,0, 0);
            }
            // myRenderer.color = Color.HSVToRGB(0, 0, health / 0.8f);
            growing = false;
        }
    }

    public void AddWater(float amount) {
        water += amount;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            ui.ShowPlantUI(true);
            uiShown = true;
        } else if (other.CompareTag("Emeny")) {
            damage += other.GetComponent<Enemy>().damage;
        }
    }


    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            ui.ShowPlantUI(false);
            uiShown = false;
        } else if (other.CompareTag("Emeny")) {
            damage -= other.GetComponent<Enemy>().damage;
            if (damage < 0) {
                damage = 0;
            }
        }
    }
    
    public static float LimitToRange(float value, float inclusiveMinimum, float inclusiveMaximum)
    {
        if (value < inclusiveMinimum) { return inclusiveMinimum; }
        if (value > inclusiveMaximum) { return inclusiveMaximum; }
        return value;
    }
}
