using TMPro;
using UnityEngine;
using UnityEngine.UI;

// makes a button for each level using SaveData
// locked levels can't be clicked, and unlocked ones show the best time if there is one
public class LevelSelect : MonoBehaviour
{
    public LevelLoader loader;
    public Button buttonTemplate;   // hidden button that gets copied
    public Transform buttonParent;
    public GameObject backButton;
    public Color labelColour = new Color32(0x0E, 0x25, 0x21, 0xFF);       // dark text on the teal button (5.36 : 1)
    public Color lockedLabelColour = new Color32(0xEA, 0xF5, 0xF3, 0xFF); // light text on the grey locked button (10.06 : 1)

    Button[] buttons;

    void Awake()
    {
        buttonTemplate.gameObject.SetActive(false);
        buttons = new Button[SaveData.LevelCount];

        for (int i = 0; i < SaveData.LevelCount; i++)
        {
            int level = i + 1; // copy for the lambda
            Button button = Instantiate(buttonTemplate, buttonParent);
            button.name = "Level" + level + "Button";
            button.gameObject.SetActive(true);
            button.onClick.AddListener(() => OnLevelClicked(level));
            buttons[i] = button;
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int level = i + 1;
            bool unlocked = SaveData.IsUnlocked(level);
            buttons[i].interactable = unlocked;

            string text = "Level " + level + "\n";
            if (!unlocked) text += "<size=70%>Locked</size>";
            else if (SaveData.HasBest(level)) text += "<size=70%>Best: " + SaveData.GetBest(level).ToString("0.0") + " s</size>";
            else text += "<size=70%>No time yet</size>";

            TMP_Text label = buttons[i].GetComponentInChildren<TMP_Text>();
            label.text = text;
            label.color = unlocked ? labelColour : lockedLabelColour;
        }
    }

    public GameObject FirstButton()
    {
        return buttons.Length > 0 ? buttons[0].gameObject : backButton;
    }

    void OnLevelClicked(int level)
    {
        if (!SaveData.IsUnlocked(level)) return;
        loader.Load(level);
    }
}
