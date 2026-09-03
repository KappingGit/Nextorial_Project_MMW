using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameFlowManager : MonoBehaviour
{
    [SerializeField] private string[] miniGameScenes; // 인스펙터에 미니게임 씬 이름들을 등록
    [SerializeField] private GameObject preparePanel; // 준비 화면UI (목숨 표시 포함된 패널) prepare 뜻 : 준비된
    [SerializeField] private float prepareDuration = 2f; // 준비 화면 대기 시간

    private string currentSceneName;
    private bool isMiniGameEnded;

    private void Start()
    {
        //LoadRandomMiniGame(); // 지금은 사용 안함
        StartCoroutine(GameFlowRoutine());
    }

    // 게임 전체를 관통하는 메인 흐름(계속 반복됨)
    private IEnumerator GameFlowRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(ShowPrepareScreen()); // 게임 준비단계 코루틴
            yield return StartCoroutine(PlayMiniGame()); // 미니 게임 단계 코루틴
        }
    }

    // 1단계 : 준비 화면(목숨 값이 보인다, 잠깐의 텀이 존재)
    private IEnumerator ShowPrepareScreen()
    {
        preparePanel.SetActive(true);
        yield return new WaitForSeconds(prepareDuration);
        preparePanel.SetActive(false);
    }

    private IEnumerator PlayMiniGame()
    {
        int index = Random.Range(0, miniGameScenes.Length); // 미니게임의 갯수만큼 int로 랜덤 값 뽑는다
        currentSceneName = miniGameScenes[index]; // 해당 인덱스의 미니게임 선택

        // AsyncOperation키워드 : 시간이 걸리는 작업(비동기 작업)의 진행 상황을 담아두는 상자라고 기억해두기
        
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);//
        yield return loadOp; // 로드가 끝날 때까지 대기

        Scene loadedScene = SceneManager.GetSceneByName(currentSceneName);
        SceneManager.SetActiveScene(loadedScene);

        isMiniGameEnded = false;
        GameManager.Instance.OnGameClear += HandleMiniGameEnd;
        GameManager.Instance.OnGameOver += HandleMiniGameEnd;

        // => 람다식: 값을 확인하는 방법(함수), "이 매개변수를 받으면, 다음과 같이 동작한다" (읽을 때 "~하면"이라고 읽으면 편해요)
        yield return new WaitUntil(() => isMiniGameEnded); // 클리어/게임오버 될 때까지 대기

        GameManager.Instance.OnGameClear -= HandleMiniGameEnd;
        GameManager.Instance.OnGameOver -= HandleMiniGameEnd;

        yield return new WaitForSeconds(1f); // 결과(클리어/게임 오버 문구 등) 확인할 짧은 텀

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentSceneName);
        yield return unloadOp; // 언로드 끝날 때까지 대기

    }

    private void HandleMiniGameEnd()
    {
        isMiniGameEnded = true;
    }

    #region 초기 코드: 바로 게임이 시작되는 코드 (준비단계 X), 만든 코드가 아까워서 일단 주석처리, 나중에 공부해두기
    /// <summary>
    /// 게임이 시작되는 함수(랜덤)
    /// </summary>
    //private void LoadRandomMiniGame()
    //{
    //    int index = Random.Range(0, miniGameScenes.Length); // 0 이상, Length 미만 (미만이라 마지막 인덱스도 정확히 포함됨)
    //    currentSceneName = miniGameScenes[index];

    //    AsyncOperation loadOp = SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);
    //    loadOp.completed += OnMiniGameLoaded; // 로드가 끝나면 OnMiniGameLoaded 실행

    //}

    //private void OnMiniGameLoaded(AsyncOperation op)
    //{
    //    // 방금 로드된 씬을 "활성 씬"으로 지정 (조명 등 씬 단위 설정 적용 목적)
    //    Scene loadedScene = SceneManager.GetSceneByName(currentSceneName);
    //    SceneManager.SetActiveScene(loadedScene);

    //    // 이제 GameManager.Instance가 방금 로드된 미니게임의 것으로 세팅되어 있음
    //    GameManager.Instance.OnGameClear += HandleMiniGameEnd;
    //    GameManager.Instance.OnGameOver += HandleMiniGameEnd;
    //}

    //private void HandleMiniGameEnd()
    //{
    //    // 중복 호출 방지 (Clear/GameOver 둘 다 구독했으니, 이벤트 해제 먼저)
    //    GameManager.Instance.OnGameClear -= HandleMiniGameEnd;
    //    GameManager.Instance.OnGameOver -= HandleMiniGameEnd;

    //    StartCoroutine(SwitchToNextMiniGame());
    //}

    //private System.Collections.IEnumerator SwitchToNextMiniGame()
    //{
    //    yield return new WaitForSeconds(1.5f); // 결과 확인할 짧은 텀 (클리어/게임오버 표시용)

    //    AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentSceneName);
    //    yield return unloadOp; // 언로드가 끝날 때까지 대기

    //    LoadRandomMiniGame(); // 다음 미니게임 랜덤 로드
    //}
    #endregion

}
