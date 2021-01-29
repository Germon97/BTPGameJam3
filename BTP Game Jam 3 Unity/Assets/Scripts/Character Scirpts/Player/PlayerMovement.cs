using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static Vector3 playerPosition = Vector3.zero;

    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private GunScript gun;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private PlayerCollector collector;
    [SerializeField]
    private GameObject hitBox;

    [SerializeField]
    private float dodgeTime = 0.5f;
    [SerializeField]
    private AnimationCurve dodgeSpeed;

    private Rigidbody2D rigi;

    private Vector2 movementInput;

    private Camera mainCam;
    private Vector3 mousePos;

    private bool dodging = false;
    private Vector2 dodgeDirection;
    private float currentDodgeTime = 0;

    private void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (dodging)
            currentDodgeTime += Time.deltaTime;

        playerPosition = transform.position;

        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(dodgeRoutine());
        }

        if (Input.GetMouseButton(0) && !dodging)
        {
            gun.Shoot(mousePos, 16);
        }

        AnimatorLoop();



#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
#endif

    }

    private void FixedUpdate()
    {
        float weightMul = collector.GetWeightSlowMultiplier();

        Vector2 vel = rigi.velocity;

        float currentSpeed = (!dodging) ? speed : dodgeSpeed.Evaluate(currentDodgeTime / dodgeTime) * speed;
        currentSpeed += currentDodgeTime * weightMul;

        if (!dodging)
            vel += movementInput * currentSpeed * Time.deltaTime;
        else
            vel += dodgeDirection * currentSpeed * Time.deltaTime;
        rigi.velocity = vel;
    }

    private void AnimatorLoop()
    {
        anim.SetBool("Walking", movementInput.magnitude > 0.05f);
        anim.SetFloat("YWalkingDirection", Mathf.Sign(mousePos.y - transform.position.y));
    }

    private IEnumerator dodgeRoutine()
    {
        dodging = true;
        currentDodgeTime = 0;

        dodgeDirection = movementInput;
        if (movementInput.magnitude < 0.05f)
            dodgeDirection = (mousePos - transform.position).normalized;

        Debug.Log("Dodge");
        anim.SetTrigger("Dodge");

        hitBox.SetActive(false);
        gun.gameObject.SetActive(false);

        yield return new WaitForSeconds(dodgeTime);

        dodging = false;
        gun.gameObject.SetActive(true);
        hitBox.SetActive(true);
    }
}
