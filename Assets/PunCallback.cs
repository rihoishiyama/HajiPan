using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PunCallback : MonoBehaviour, IPunOwnershipCallbacks
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable() {
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable() {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    // 所有権のリクエストが行われた時に呼ばれるコールバック
    void IPunOwnershipCallbacks.OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        // 自身が所有権を持つインスタンスで所有権のリクエストが行われたら、常に許可して所有権を移譲する
        if (targetView.IsMine)
        {
            bool acceptsRequest = true;
            if (acceptsRequest)
            {
                targetView.TransferOwnership(requestingPlayer);
            }
            else
            {
                // リクエストを拒否する場合は、何もしない
            }
        }
    }

    // 所有権の移譲が行われた時に呼ばれるコールバック
    void IPunOwnershipCallbacks.OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        string id = targetView.ViewID.ToString();
        string p1 = previousOwner.NickName;
        string p2 = targetView.Owner.NickName;
        Debug.Log($"ViewID {id} の所有権が {p1} から {p2} に移譲されました");
        //PhotonNetwork.Destroy(this.gameObject);
    }
}