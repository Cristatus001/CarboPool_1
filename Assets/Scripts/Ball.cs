using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    public enum FragmentType { CH3, CH2, CH, C, OH, COOH, NH2, Wrong }
    public FragmentType fragmentType;
    public bool isPotted = false;

    private TextMeshProUGUI formulaText;

    void Start()
    {
        formulaText = GetComponentInChildren<TextMeshProUGUI>();
        if (formulaText != null)
            formulaText.text = "";
    }

    public void ResetBall()
    {
        isPotted = false;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        transform.localScale = Vector3.one * 0.9f;
    }
}