using Photon.Pun;
using UnityEngine;

// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class AvatarController : MonoBehaviourPunCallbacks
{
    private void Update() {

        // Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);

        // if (moveVector != Vector3.zero)
        // {
        //     transform.rotation = Quaternion.LookRotation(moveVector);
        //     transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);
        // }   
        // 自身が生成したオブジェクトだけに移動処理を行う
        if (photonView.IsMine && GameState.m_gameState == GameState.e_GameState.Game) {
            var input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            if (input != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(input.normalized);
                transform.Translate(0f, 0f , 4f * Time.deltaTime);
            // transform.rotation = Quaternion.LookRotation(moveVector);
            // transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);
            }    
            
        }
    }
}