using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] private RectTransform leftDoor;
    [SerializeField] private RectTransform rightDoor;
    [SerializeField] private float doorMoveSpeed = 500f;
    private static string loadingScene = "Loading";
    private static string subwayScene = "subway_door";
    private bool doorsOpened = false;

    void Start()
    {
        if (leftDoor == null || rightDoor == null)
        {
            Debug.LogError("LeftDoor or RightDoor is not assigned in the Inspector.");
            return;
        }

        leftDoor.anchoredPosition = new Vector2(-283.7274f, -1f);
        rightDoor.anchoredPosition = new Vector2(258.58f, -1f);

        // 로딩 씬을 먼저 로드
        SceneManager.LoadScene(loadingScene, LoadSceneMode.Additive);

        // 문이 열리는 코루틴 시작
        StartCoroutine(OpenDoors());
    }

    private IEnumerator OpenDoors()
    {
        float timer = 0f;

        Vector2 leftDoorStartPos = new Vector2(-283.7274f, -1f);
        Vector2 rightDoorStartPos = new Vector2(258.58f, -1f);

        Vector2 leftDoorEndPos = new Vector2(-774f, -1f);
        Vector2 rightDoorEndPos = new Vector2(749.8526f, -1f);

        // 문이 열리는 애니메이션
        while (!doorsOpened)
        {
            yield return null;

            timer += Time.unscaledDeltaTime * doorMoveSpeed;
            float progress = Mathf.Clamp01(timer / Mathf.Abs(leftDoorEndPos.x - leftDoorStartPos.x));

            leftDoor.anchoredPosition = Vector2.Lerp(leftDoorStartPos, leftDoorEndPos, progress);
            rightDoor.anchoredPosition = Vector2.Lerp(rightDoorStartPos, rightDoorEndPos, progress);

            Debug.Log($"Progress: {progress}, LeftDoor Pos: {leftDoor.anchoredPosition}, RightDoor Pos: {rightDoor.anchoredPosition}");

            if (progress >= 1f)
            {
                doorsOpened = true;
                UnloadDoorScene();
            }
        }
    }

    private void UnloadDoorScene()
    {
        // subway_door 씬을 Unload
        SceneManager.UnloadSceneAsync(subwayScene);
    }
}
