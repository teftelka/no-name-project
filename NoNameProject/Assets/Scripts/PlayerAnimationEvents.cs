using UnityEngine;
using System.Collections;

public class PlayerAnimationEvents : MonoBehaviour {

    private Animator anim;

	void Start () {
        anim = gameObject.GetComponent<Animator>();
	}
	
	void Update () {
	
	}

    public void AttackAnimationFinish()
    {
        anim.SetBool("Attack", false);
    }
}
