using UnityEngine;

public class DetectionText : MonoBehaviour
{
    public float speed = 5f;
    public float delayDuration = 1.5f;

    private bool isWaiting = false;
    private float waitTimer = 0f;

    void Update()
    {
        if (!isWaiting)
        {
            // Move the text from left to right
            MoveText();
        }
        else
        {
            // Increment the timer while waiting
            waitTimer += Time.deltaTime;

            // Check if the delay duration has been reached
            if (waitTimer >= delayDuration)
            {
                // Stop waiting and reset the timer
                isWaiting = false;
                waitTimer = 0f;

                // Reset the text position to the left of the screen
                ResetTextPosition();
            }
        }
    }

    void MoveText()
    {
        // Calculate the new position based on the current position and speed
        Vector3 newPosition = transform.position + Vector3.right * speed * Time.deltaTime;

        // Set the new position
        transform.position = newPosition;

        // Check if the text has reached the middle of the screen
        if (newPosition.x > Screen.width / 2)
        {
            // Start waiting
            isWaiting = true;
        }
    }

    void ResetTextPosition()
    {
        // Reset the text position to the left of the screen
        transform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0f);
    }
}
