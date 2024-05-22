using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Check_list : MonoBehaviour
{
    public TextMeshProUGUI descriptionText; // ���� �ؽ�Ʈ UI ����
    public ScrollRect scrollRect; // Scroll View ����

    private Dictionary<string, string> characterDescriptions = new Dictionary<string, string>();
    private HashSet<string> foundCharacters = new HashSet<string>(); // ã�� ĳ���� ����

    void Start()
    {
        // ĳ���� ���� �ʱ�ȭ
        characterDescriptions.Add("clBird", "bird");
        characterDescriptions.Add("clclean", "clean");
        // �߰� ĳ���� ������ ���⿡ �߰�

        // ��ũ�� ����Ʈ�� ��� ��ư�� Ŭ�� �̺�Ʈ ������ �߰�
        foreach (Transform buttonTransform in scrollRect.content)
        {
            Button button = buttonTransform.GetComponent<Button>();
            if (button != null)
            {
                // ���� ��ư�� ĸó
                Button capturedButton = button;
                button.onClick.AddListener(() => OnScrollButtonClick(capturedButton));
            }
        }
    }

    public void ClickBtn()
    {
        print("��ư Ŭ��");

        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;

        print(clickedObject.name);

        // Scroll ����Ʈ�� �ش� ��ư�� �������ϰ� ����
        GameObject scrollButton = GameObject.Find("cl" + clickedObject.name);
        if (scrollButton != null)
        {
            Button button = scrollButton.GetComponent<Button>();
            Image buttonImage = button.GetComponent<Image>();
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0.5f);

            // �ش� ĳ���͸� ã������ ǥ��
            foundCharacters.Add(button.gameObject.name);
        }
    }

    void OnScrollButtonClick(Button clickedButton)
    {
        // Ŭ���� ��ư�� �̸����� ���� �Ǵ� üũ ���� ������Ʈ
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
