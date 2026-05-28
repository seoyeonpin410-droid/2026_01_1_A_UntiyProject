using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class DialogueManager : MonoBehaviour
{

    [Header("UI 요소 - 인스펙터 창에서 연결")]
    public GameObject DialoguePanel;                  //대화창 전체 패널
    public Image characterImage;                      //캐릭터 이미지 표시하는 UI
    public TextMeshProUGUI characterNameText;         //캐릭터 이름 표시하는 텍스트
    public TextMeshProUGUI dialogueText;              //대화 내용을 표시하는 텍스트
    public Button nextButton;                         //다음 대화 버튼

    [Header("기본 설정")]
    public Sprite defaultCharacterImage;              //캐릭터 이미지가 없을때 사용 할 기본 이미지

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f;                 //글자 하나당 출력 속도
    public bool skipTypingOnClick = true;             //클릭시 타이핑 즉시 완료 여부

    //내부 변수들
    private DialogueDataSO currentDialogue;           //현재 진행 중인 대화 데이터
    private int currenLineIndex = 0;                 //현재 몇 번째 대화 중인지 
    private bool isDialogueActive = false;            //대화가 진행 중인지 확인하는 플래그
    private bool isTyping = false;                    //현재 타이핑 효과가 진행 중인지 확인
    private Coroutine typingCoroutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialoguePanel.SetActive(true);                //대화창 숨기기
        nextButton.onClick.AddListener(HandleNextInput); // 버튼에 새로운 입력 처리 연결
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();  //다음 입력 처리

        }
    }

    IEnumerator TypeText(string textToType)            //타이핑 할 전체 텍스트
    {
        isTyping = true;
        dialogueText.text = "";

        //텍스트를 한 글자씩 추가
        for (int i = 0; i < textToType.Length; i++)
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void CompleteTyping()                       //타이핑 효과를 즉시 완료 하는 함수 
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);             //코루틴 중지
        }

        isTyping = false;                               //타이핑 상태 해제 

        //현재 줄의 전체 텍스트를 즉시 표시 
        if(currentDialogue != null && currenLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currenLineIndex];
        }
    }

    void ShowCurrentLine() //현재 대화 줄의 내용을 타이핑 효과와 함께 화면에 표시하는 함수
    {
        if (currentDialogue != null && currenLineIndex < currentDialogue.dialogueLines.Count)
        {
            if (typingCoroutine != null) //이전 타이핑 효과가 있다면
            {
                StopCoroutine(typingCoroutine);
            }
        }

        //현재 줄의 대화 내용으로 타이핑 효과 시작
        string currentText = currentDialogue.dialogueLines[currenLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }

    public void ShowNextLine()
    {
        currenLineIndex++;

        if (currenLineIndex >= currentDialogue.dialogueLines.Count)
        {

        }
        else
        {
            ShowCurrentLine();
        }
    }

    void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine );
            typingCoroutine = null;
        }

        isDialogueActive = false;
        isTyping = false;
        DialoguePanel.SetActive(false);  
        currenLineIndex = 0;
        
    }

    public void HandleNextInput()
    {
        if(isTyping && skipTypingOnClick)
        {
            CompleteTyping();
        }
        else if (!isTyping)
        {
            ShowNextLine();
        }

        
    }

    public void SkipDialogue()
    {
        EndDialogue();
    }

    public bool IsDiaigyeActive()
    {
        return isDialogueActive;
    }

    public void StartDialogue(DialogueDataSO dialogue)    //새로운 대화를 시작 하는 함수
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return; //대화 데이터 없거나 내용이 비어 있으면 실행 하지 않음

        //대화 시작 준비
        currentDialogue = dialogue;                       //현재 대화 데이터 설정
        currenLineIndex = 0;                              //첫 번째 대화 부터 시작
        isDialogueActive = true;                          //대화 활성화 플래그 On

        //UI 업데이트
        DialoguePanel.SetActive(true);                    //대화창 보이기
        characterNameText.text = dialogue.characterName;  //캐릭터 이름 표시

        if (characterImage != null)
        {
            if (dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;      
            }
            else
            {
                characterImage.sprite = defaultCharacterImage;        
            }
        }

        ShowCurrentLine();                               
    }

}


