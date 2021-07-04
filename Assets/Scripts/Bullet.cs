using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private PhotonView photonview;
	public AudioClip reboundSound;
	public int reboundcount;


	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{

			reboundcount += 1;

			if (reboundcount > 1)
			{
				if (photonView.IsMine)
				{
				ShotBullet.bulletcount -= 1;
				}
				PhotonNetwork.Destroy(this.gameObject);
			}
			else
			{
				AudioSource.PlayClipAtPoint(reboundSound, transform.position);
			}
		}
		if (other.gameObject.CompareTag("Bullet"))
		{

			if (photonView.IsMine)
			{
			ShotBullet.bulletcount -= 1;
			}
			PhotonNetwork.Destroy(this.gameObject);
		}
		if (other.gameObject.CompareTag("Player"))
		{
			if (photonView.IsMine)
			{
			ShotBullet.bulletcount -= 1;
			}
			PhotonNetwork.Destroy(this.gameObject);
		}
			
    }
	
}