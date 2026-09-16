using UnityEngine;
using System.Collections;

public class Pocket : MonoBehaviour
{
    public float sinkDuration = 0.4f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle cue ball
        if (other.CompareTag("CueBall"))
        {
            StartCoroutine(SinkBall(other.gameObject, "CueBall"));
            return;
        }

        // Handle any ball — check fragmentType directly
        Ball ball = other.GetComponent<Ball>();
        if (ball != null && !ball.isPotted)
        {
            ball.isPotted = true;

            if (ball.fragmentType == Ball.FragmentType.Wrong)
                StartCoroutine(SinkBall(other.gameObject, "WrongBall"));
            else
                StartCoroutine(SinkBall(other.gameObject, "Ball"));
        }
    }

    IEnumerator SinkBall(GameObject ball, string ballType)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();

        if (rb == null || sr == null) yield break;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        Vector3 originalScale = ball.transform.localScale;
        Color originalColor = sr.color;
        Vector3 pocketCenter = transform.position;
        float elapsed = 0f;

        while (elapsed < sinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / sinkDuration;

            ball.transform.position = Vector3.Lerp(
                ball.transform.position,
                pocketCenter,
                t * 5f * Time.deltaTime
            );

            ball.transform.localScale = Vector3.Lerp(
                originalScale,
                Vector3.zero,
                t
            );

            sr.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                Mathf.Lerp(1f, 0f, t)
            );

            yield return null;
        }

        if (ballType == "CueBall")
        {
            ball.transform.localScale = originalScale;
            ball.transform.position = new Vector3(-2f, 0f, 0f);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            if (GameManager.Instance != null)
                GameManager.Instance.CueBallPotted();

            Debug.Log("Cue ball scratched!");
        }
        else if (ballType == "WrongBall")
        {
            if (GameManager.Instance != null)
                GameManager.Instance.WrongBallPotted(ball);

            Debug.Log("Wrong ball potted!");
        }
        else if (ballType == "Ball")
        {
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null && GameManager.Instance != null)
            {
                GameManager.Instance.BallPotted(ballScript.fragmentType);
                Debug.Log("Correct ball: " + ballScript.fragmentType);
            }
            ball.SetActive(false);
        }
    }
}