using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameFlowManager : MonoBehaviour
{
    [SerializeField] private string[] miniGameScenes; // 인스펙터에 미니게임 씬 이름들을 등록

    private string currentSceneName;

    private void Start()
    {
        
    }

    private void LoadRandomMiniGame()
    {
        int index = Random.Range(0, miniGameScenes.Length); // 0 이상, Length 미만 (미만이라 마지막 인덱스도 정확히 포함됨)
        currentSceneName = miniGameScenes[index];

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);
        loadOp.completed += OnMiniGameLoaded; // 로드가 끝나면 OnMiniGameLoaded 실행

    }

    private void OnMiniGameLoaded(AsyncOperation op)
    {
        // 방금 로드된 씬을 "활성 씬"으로 지정 (조명 등 씬 단위 설정 적용 목적)
        Scene loadedScene = SceneManager.GetSceneByName(currentSceneName);
        SceneManager.SetActiveScene(loadedScene);

        // 이제 GameManager.Instance가 방금 로드된 미니게임의 것으로 세팅되어 있음
        GameManager.Instance.OnGameClear += HandleMiniGameEnd;
        GameManager.Instance.OnGameOver += HandleMiniGameEnd;
    }

    private void HandleMiniGameEnd()
    {
        // 중복 호출 방지 (Clear/GameOver 둘 다 구독했으니, 이벤트 해제 먼저)
        GameManager.Instance.OnGameClear -= HandleMiniGameEnd;
        GameManager.Instance.OnGameOver -= HandleMiniGameEnd;

        StartCoroutine(SwitchToNextMiniGame());
    }

    private System.Collections.IEnumerator SwitchToNextMiniGame()
    {
        yield return new WaitForSeconds(1.5f); // 결과 확인할 짧은 텀 (클리어/게임오버 표시용)

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentSceneName);
        yield return unloadOp; // 언로드가 끝날 때까지 대기

        LoadRandomMiniGame(); // 다음 미니게임 랜덤 로드
    }
}
