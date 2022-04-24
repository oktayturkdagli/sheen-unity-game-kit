using UnityEngine;
using TMPro;

public class CharacterTester : MonoBehaviour
{
	[SerializeField] float speed = 3f;
	[SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI diamondText;


    public void HandleJoystick(Vector2 direction)
	{
		transform.Translate(new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime, Space.World);
        if (direction.magnitude > 0.05)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("diamond"))
        {
            Debug.Log("You earn 1 diamond!");
            diamondText.text = (int.Parse((diamondText.text)) + 1).ToString();
            Destroy(other.gameObject);
        }
    }
}
