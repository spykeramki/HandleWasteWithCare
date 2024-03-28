using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDetailsCtrl : MonoBehaviour
{
    public TextMeshProUGUI nameInstruction;

    public Button startBtn;

    private string typeInstruction = "Good to go";

    private string nameAlreadyExists = "Hey! You have a twin with same name.\ngive another name";

    private string emptyTextWarning = "You may feel Empty.\nBut you are more than nothing";

    private DataManager.PlayerData playerData;

    private void Start()
    {
        playerData = new DataManager.PlayerData();
        startBtn.onClick.AddListener(OnClickStartBtn);
    }

    public void OnChangeName(string m_name)
    {
        playerData.name = m_name;
        bool doesPlayerNameExist =  DataManager.Instance.PlayerNameAlreadyExists(m_name);
        if(m_name == string.Empty)
        {
            nameInstruction.text = emptyTextWarning;
            errorSetings();
            return;
        }
        if (doesPlayerNameExist)
        {
            nameInstruction.text = nameAlreadyExists;
            errorSetings();
        }
        else
        {
            nameInstruction.text = typeInstruction;
            goodToGoSettings();

        }
    }

    private void goodToGoSettings()
    {
        nameInstruction.color = Color.green;
        startBtn.interactable = true;
    }

    private void errorSetings()
    {
        nameInstruction.color = Color.red;
        startBtn.interactable = false;
    }

    public void OnClickStartBtn()
    {
        DataManager.Instance.SetPlayerData(playerData);
        SceneManager.LoadScene("01Main");
    }

}
