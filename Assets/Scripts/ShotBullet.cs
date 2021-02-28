using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ShotBullet : MonoBehaviourPunCallbacks
{
	[SerializeField]
	//private PhotonView photonView;
	public Transform shotPlace;
	public GameObject shellPrefab;
	public float shotSpeed;
	public AudioClip shotSound;
	public AudioClip shotmissSound;
	public static int bulletcount;

	public void ButtonShot()
	{
        // Shot();
        // AudioSource.PlayClipAtPoint(shotSound, transform.position);
		// if (photonView.isMine)
		// {
		// もしも「Fire1」というボタンが押されたら（条件）
		if (bulletcount < 5)
		{
			Shot();
			// ②効果音を再生する。
			AudioSource.PlayClipAtPoint(shotSound, transform.position);
			bulletcount += 1;
		}
		else
		{
			AudioSource.PlayClipAtPoint(shotmissSound, transform.position);
		}
		// }
	}

	public void Shot()
	{
		// プレファブから砲弾(Shell)オブジェクトを作成し、それをshellという名前の箱に入れる。
		GameObject shell = PhotonNetwork.Instantiate("bullet", shotPlace.position, Quaternion.identity, 0);
		//GameObject shell = Instantiate(shellPrefab, shotPlace.position, Quaternion.identity);

		// Rigidbodyの情報を取得し、それをshellRigidbodyという名前の箱に入れる。
		Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();

		// shellRigidbodyにz軸方向の力を加える。
		shellRigidbody.AddForce(transform.forward * shotSpeed);
	}
}