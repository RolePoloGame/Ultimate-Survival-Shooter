using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private bool AllowRotation = true;
	[SerializeField]
    private float speed = 50f;

    // Update is called once per frame
    void Update()
    {
        if (!AllowRotation)
            return;
        Rotate();
    }

    private void Rotate() => transform.Rotate(0f, speed * Time.deltaTime, 0f);
}
