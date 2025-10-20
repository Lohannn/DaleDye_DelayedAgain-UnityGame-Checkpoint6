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
