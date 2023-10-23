using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonShield : AreaShield
{
    public int sides;
    public float radius;

    public GameObject segmentPrefab;
    public Transform shieldTransform;

    public List<EdgeCollider2D> segmentColliders;
    public List<LineRenderer> segmentRenderers;
    public float[] segmentHealth;
    public float[] lastDamageTime;

    public float segmentMaxHealth;
    public float segmentInitialHealth;
    public float distributionRate;
    public float restoreAmount;



    [Header("Appearance")]
    public float alphaMin;
    public float alphaMax;
    public float widthMin;
    public float widthMax;
    public Color normalColor;
    public Color damagedColor;
    public Color brokenColor;

    void Start()
    {
        CreateSegments();
        PolygonPosition();
    }

    private void Update()
    {
        shieldDistribution();
        SegmentApperance();
        SetSegmentWidth();
    }

    public override void Damage(Collider2D collider, int damageAmount)
    {
        EdgeCollider2D edgeCollider = (EdgeCollider2D)collider;
        int c = segmentColliders.IndexOf(edgeCollider);
        segmentHealth[c] -= damageAmount;
        lastDamageTime[c] = Time.time;
    }

    public override void Collision(Collider2D collider, GameObject other)
    {
        EdgeCollider2D edgeCollider = (EdgeCollider2D)collider;
        int c = segmentColliders.IndexOf(edgeCollider);
        Debug.Log("Collision with segment " + c);

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (segmentHealth[c] >= collisionSelfDamage)
        {
            player.Damage(collisionDamageMax);
        }

        else
        {
            int collisionDamage = (int)(collisionDamageMax * (segmentHealth[c] / collisionSelfDamage));
            player.Damage(collisionDamage);
        }

        Damage(collider, collisionSelfDamage);
    }

    public override float GetHealth(Collider2D collider)
    {
        EdgeCollider2D edgeCollider = (EdgeCollider2D)collider;
        int s = segmentColliders.IndexOf(edgeCollider);
        return segmentHealth[s];
    }

    public void shieldDistribution()
    {
        //Each shield segment shares a portion of it's health to neighboring segments
        for(int i=0;i < sides; i++)
        {
            if (segmentHealth[i] > 0)
            {
                segmentRenderers[i].enabled = true;
                segmentColliders[i].enabled = true;

                float shareAmount = segmentHealth[i] * distributionRate * Time.deltaTime;
                segmentHealth[(sides + i - 1) % sides] += shareAmount;
                segmentHealth[(i + 1) % sides] += shareAmount;
                segmentHealth[i] -= shareAmount * 2;
            }


            else
            {
                //segmentRenderers[i].enabled = false;
                segmentColliders[i].enabled = false;
            }
        }
    }

    public void Restore()
    {
        for (int i = 0; i < sides; i++)
        {
            segmentHealth[i] += restoreAmount;
            if (segmentHealth[i] > segmentMaxHealth) segmentHealth[i] = segmentMaxHealth;
        }
    }

    public void CreateSegments()
    {
        segmentHealth = new float[sides];
        lastDamageTime = new float[sides];
        for (int n=0; n < sides; n++)
        {
            GameObject segment = Instantiate(segmentPrefab, shieldTransform);
            segmentColliders.Add(segment.GetComponent<EdgeCollider2D>());
            segmentRenderers.Add(segment.GetComponent<LineRenderer>());

            segmentHealth[n] = segmentInitialHealth;
            lastDamageTime[n] = 0;
        }

    }

    private void PolygonPosition()
    {
        Vector3[] points = new Vector3[sides];
        float TAU = 2 * Mathf.PI;

        for (int currentPoint = 0; currentPoint < sides; currentPoint++)
        {
            float currentRadian = ((float)currentPoint / sides) * TAU;
            points[currentPoint].x = Mathf.Cos(currentRadian) * radius;
            points[currentPoint].y = Mathf.Sin(currentRadian) * radius;
        }

        for (int i = 0; i < sides; i++)
        {
            Vector2[] edgePoints = new Vector2[2];
            edgePoints[0] = points[i];
            edgePoints[1] = points[(i + 1) % sides];
            segmentColliders[i].points = edgePoints;

            segmentRenderers[i].SetPosition(0, edgePoints[0]);
            segmentRenderers[i].SetPosition(1, edgePoints[1]);
        }

    }



    public void SegmentApperance()
    {
        float alpha, healthFraction; Color color;

        for (int a = 0; a < sides; a++)
        {
            healthFraction = segmentHealth[a] / segmentMaxHealth;

            if (healthFraction < 0)
            {
                color = brokenColor;
                alpha = Mathf.Lerp(0.4f, 0, (Time.time - lastDamageTime[a]) / 0.15f);
            }

            else
            {
                if (Time.time < lastDamageTime[a] + 0.05f)
                {
                    color = damagedColor;
                    alpha = 0.9f;
                }

                else
                {
                    color = normalColor;
                    alpha = Mathf.Lerp(alphaMin, alphaMax, healthFraction);
                }
            }

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                );
            segmentRenderers[a].colorGradient = gradient;

        }
    }


    public void SetSegnmentAlphas()
    {
        for (int a = 0; a < sides; a++)
        {
            float alpha = Mathf.Lerp(alphaMin, alphaMax, segmentHealth[a] / segmentMaxHealth);

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                segmentRenderers[a].colorGradient.colorKeys,
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                );
            segmentRenderers[a].colorGradient = gradient;
        }
    }

    public void SetSegmentWidth()
    {
        for (int w=0; w < sides; w++)
        {
            float width = Mathf.Lerp(widthMin, widthMax, segmentHealth[w] / segmentMaxHealth);
            segmentRenderers[w].startWidth = width;
            segmentRenderers[w].endWidth = width;
        }
    }

    public void Rotate(float rotationSpeed)
    {
        shieldTransform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);
    }
}
