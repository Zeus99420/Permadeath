using UnityEngine;
using UnityEngine.UI;

public class CoolHealthBar : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private float maxHealthPoints = 100;

    [SerializeField]
    private float healthBarStepsLength = 10;

    [SerializeField]
    private float damagesDecreaseRate = 10;

    private float currentHealthPoints;

    private RectTransform imageRectTransform;

    private float damages;

    public Character character;
    public Vector3 offset;

    public bool fixedPosition;
    public float Health
    {
        get { return currentHealthPoints; }
        set
        {
            currentHealthPoints = Mathf.Clamp(value, 0, MaxHealthPoints);
            image.material.SetFloat("_Percent", currentHealthPoints / MaxHealthPoints);

            //if (currentHealthPoints < Mathf.Epsilon)
            //    Damages = 0;
        }
    }

    public float Damages
    {
        get { return damages; }
        set
        {
            damages = Mathf.Clamp(value, 0, MaxHealthPoints);
            image.material.SetFloat("_DamagesPercent", damages / MaxHealthPoints);
        }
    }

    public float MaxHealthPoints
    {
        get { return maxHealthPoints; }
        set
        {
            maxHealthPoints = value;
            image.material.SetFloat("_Steps", MaxHealthPoints / healthBarStepsLength);

        }
    }

    protected void Awake()
    {
        imageRectTransform = image.GetComponent<RectTransform>();
        image.material = Instantiate(image.material); // Clone material
    }

    public void SetSize()
    {
        float width = Mathf.Pow(maxHealthPoints, 0.7f) / 20f;
        float height = 0.05f + Mathf.Sqrt(maxHealthPoints) * 0.015f;
        transform.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        image.material.SetVector("_ImageSize", new Vector4(imageRectTransform.rect.size.x, imageRectTransform.rect.size.y, 0, 0));

        MaxHealthPoints = MaxHealthPoints; // Force the call to the setter in order to update the material
        currentHealthPoints = currentHealthPoints; // Force the call to the setter in order to update the material
    }

    protected void Update()
    {
        if (!fixedPosition) transform.position = character.transform.position + offset; ;

        if (Damages > 0)
        {
            Damages -= damagesDecreaseRate * Time.deltaTime;
        }

    }

    public void Hurt(float damagesPoints)
    {
        Damages += damagesPoints;
        Health -= damagesPoints;
    }
}