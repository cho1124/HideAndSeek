using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObjectShuffler : NetworkBehaviour {
    /*
    맵의 다양성? 혹은 플레이어에게 혼란을 주기 위해...
    맵에 놓인 오브젝트들(움직일만한것들, 커다란 집 이런건 아님...)의
    위치를 배열에 넣고 그들이 게임이 시작될때마다 자리를 바꿀 수 있도록 하는 스크립트

    Fisher-Yates 셔플 알고리즘을 사용하여 무작위성 개선했습니다.
    objects 배열이 null이거나 비어 있을 때의 처리를 추가하였습니다.

    서버가 위치를 셔플한 후 클라이언트들에게 해당 위치를 전송하여 동일한 위치가 적용되도록 하는중입니다...
    */

    public GameObject[] objects; // 인스펙터에서 오브젝트를 넣을 수 있도록 배열을 선언

    public override void OnStartServer() {
        Debug.Log("OnStartServer called");

        if (objects == null || objects.Length == 0) {
            Debug.LogWarning("Objects array is null or empty. Shuffling aborted.");
            return;
        }
        if (isServer) {
            Debug.Log("서버네");
            // 서버에서만 셔플을 실행
            ShufflePositions();
            // 클라이언트들에게 셔플된 위치를 동기화
            RpcUpdatePositions(GetPositions());
        }
    }

    public override void OnStartClient() //클라이언트에서 따로 위치 요청 없이 서버에서 바로 동기화된 위치를 받을 수 있도록 조정 
{
        base.OnStartClient();
    }
  
  //      if (!isServer) {
  //          CmdRequestPositions();
  //      }
  //  }

  //  [Command] //클라이언트에서 서버로 명령을 보내, 현재 오브젝트 위치 요청
  //  void CmdRequestPositions() {
  //      RpcUpdatePositions(GetPositions());
  //  }

    void ShufflePositions() {
        // 배열의 오브젝트 수만큼 위치를 저장할 배열을 생성
        Vector3[] positions = new Vector3[objects.Length];

        // 각 오브젝트의 현재 위치를 저장
        for (int i = 0; i < objects.Length; i++) {
            positions[i] = objects[i].transform.position;
        }


        // Fisher-Yates 알고리즘을 사용하여 위치 배열을 랜덤하게 섞음
        for (int i = positions.Length - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        //  // 섞인 위치를 각 오브젝트에 적용
        //  for (int i = 0; i < objects.Length; i++) {
        //      objects[i].transform.position = positions[i];
        //  }

        // 섞인 위치를 각 오브젝트에 적용
        SetPositions(positions);

    }

    Vector3[] GetPositions() //objects배열에 있는 모든 게임 오브젝트의 위치를 가져와서 Vector3 배열로 반환
        {
        Vector3[] positions = new Vector3[objects.Length];
        for (int i = 0; i < objects.Length; i++) {
            positions[i] = objects[i].transform.position;
        }
        return positions; //든 오브젝트의 위치를 담은 positions 배열을 반환합니다.
    }

    void SetPositions(Vector3[] positions) //전달받은 positions 배열을 사용하여 objects 배열에 있는 각 게임 오브젝트의 위치를 설정합니다.
{
        for (int i = 0; i < objects.Length; i++) {
            // `NetworkTransform` 비활성화 대신 자동으로 위치를 동기화하도록 둡니다.
            objects[i].transform.position = positions[i]; // 위치 설정
        }
    }

    [ClientRpc] //서버가 클라이언트들에게 데이터를 전송
    void RpcUpdatePositions(Vector3[] positions) {
        // 클라이언트가 서버에서 받은 위치 정보를 적용
        SetPositions(positions);
    }

}

