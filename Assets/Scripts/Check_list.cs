using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Check_list : MonoBehaviour
{
    public TextMeshProUGUI descriptionText; // 설명 텍스트 UI 참조
    public ScrollRect scrollRect; // Scroll View 참조

    private Dictionary<string, string> characterDescriptions = new Dictionary<string, string>();
    private HashSet<string> foundCharacters = new HashSet<string>(); // 찾은 캐릭터 추적

    void Start()
    {
        // 캐릭터 설명 초기화
        characterDescriptions.Add("clBird", "bird");
        characterDescriptions.Add("clclean", "clean");
        // 추가 캐릭터 설명을 여기에 추가

        // 스크롤 리스트의 모든 버튼에 클릭 이벤트 리스너 추가
        foreach (Transform buttonTransform in scrollRect.content)
        {
            Button button = buttonTransform.GetComponent<Button>();
            if (button != null)
            {
                // 현재 버튼을 캡처
                Button capturedButton = button;
                button.onClick.AddListener(() => OnScrollButtonClick(capturedButton));
            }
        }
    }

    public void ClickBtn()
    {
        print("버튼 클릭");

        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;

        print(clickedObject.name);

        // Scroll 리스트의 해당 버튼도 반투명하게 설정
        GameObject scrollButton = GameObject.Find("cl" + clickedObject.name);
        if (scrollButton != null)
        {
            Button button = scrollButton.GetComponent<Button>();
            Image buttonImage = button.GetComponent<Image>();
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0.5f);

            // 해당 캐릭터를 찾았음을 표시
            foundCharacters.Add(button.gameObject.name);
        }
    }

    void OnScrollButtonClick(Button clickedButton)
    {
        // 클릭된 버튼의 이름으로 설명 또는 체크 상태 업데이트
        string buttonName = clickedButton.gameObject.name;
        if (foundCharacters.Contains(buttonName))
        {
            descriptionText.text = "check";
        }
        else
        {
            if (characterDescriptions.ContainsKey(buttonName))
            {
                descriptionText.text = characterDescriptions[buttonName];
            }
            else
            {
                descriptionText.text = "";
            }
        }
    }
}
