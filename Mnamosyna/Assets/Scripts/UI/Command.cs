using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command : MonoBehaviour
{
    public GameObject player;
    private Player playerCon;
    public Text CommandText;

    void Start()
    {
        playerCon = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CommandText.text = "Command: ";
        CommandText.text += playerCon.SkillCommand.Length > 10 ? playerCon.SkillCommand.Substring(playerCon.SkillCommand.Length - 10, 10) : playerCon.SkillCommand;
    }
}
