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

    */

    public GameObject[] objects; // 인스펙터에서 오브젝트를 넣을 수 있도록 배열을 선언

    void Start() {
        if (objects == null || objects.Length == 0) {
            Debug.LogWarning("Objects array is null or empty. Shuffling aborted.");
            return;
        }
        ShufflePositions();

        // 셔플된 위치를 클라이언트들에게 동기화
        RpcUpdatePositions(GetPositions());
    }

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

        // 섞인 위치를 각 오브젝트에 적용
        for (int i = 0; i < objects.Length; i++) {
            objects[i].transform.position = positions[i];
        }
    }

    Vector3[] GetPositions() {
        Vector3[] positions = new Vector3[objects.Length];
        for (int i = 0; i < objects.Length; i++) {
            positions[i] = objects[i].transform.position;
        }
        return positions;
    }

    [ClientRpc]
    void RpcUpdatePositions(Vector3[] positions) {
        // 클라이언트들이 서버에서 받은 위치 정보를 적용
        for (int i = 0; i < objects.Length; i++) {
            objects[i].transform.position = positions[i];
        }
    }
}

