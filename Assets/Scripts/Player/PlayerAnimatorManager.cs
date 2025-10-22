using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    private Player player;
    private Animator anim;

    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetCurrentSpeed() > player.GetBaseSpeed())
        {
            anim.speed = 1.0f * player.GetSugarMultiplier();
        }
        else
        {
            anim.speed = 1.0f;
        }

        anim.SetInteger("pMove", (int)player.GetMovement());
        anim.SetBool("pGround", player.OnGround());
        anim.SetFloat("pAirSpeed", player.GetAirSpeed());
        anim.SetBool("pSlow", player.GetSlowed());
    }

    public void Jump()
    {
        anim.SetTrigger("pJump");
    }
}
