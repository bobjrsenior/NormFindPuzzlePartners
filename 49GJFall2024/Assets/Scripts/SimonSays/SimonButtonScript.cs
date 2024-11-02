using UnityEngine;

public class SimonButtonScript : MonoBehaviour
{
    public float patternCooldown;

    public float curCoolDownTimer;

    public bool pressed = false;

    public Color saveColor;

    public UnityEngine.UI.Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = this.GetComponent<UnityEngine.UI.Image>();
        saveColor = image.color;
        patternCooldown = SimonSaysHandler.instance.patternCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed)
        {
            curCoolDownTimer -= Time.deltaTime;
            if (curCoolDownTimer <= 0.0f)
            {
                pressed = false;
                image.color = saveColor;
            }
        }
    }

    public void Press()
    {
        image.color = Color.white;
        curCoolDownTimer = patternCooldown;
        pressed = true;
    }
}
