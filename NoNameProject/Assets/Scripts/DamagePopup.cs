using TMPro;
using UnityEngine;
using CodeMonkey.Utils;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    
    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.damagePopup, position, Quaternion.identity);
        
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);
        
        return damagePopup;
    }
    
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Setup(int damageAmount, bool isCriticalHit)
    {
        textMesh.SetText(damageAmount.ToString());
        if (!isCriticalHit)
        {
            textMesh.fontSize = 10;
            textColor = UtilsClass.GetColorFromString("FF7800");
        }
        else
        {
            textMesh.fontSize = 12;
            textColor = UtilsClass.GetColorFromString("F84848");
        }
        
        textMesh.color = textColor;
        disappearTimer = 1f;
    }

    private void Update()
    {
        float moveYSpeed = 6f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
