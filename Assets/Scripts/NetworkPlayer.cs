using System;
using System.Linq;
using System.Text.RegularExpressions;
using Mirror;
using TMPro;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private TextMeshProUGUI nameText;
    [SyncVar(hook = nameof(HandleNameUpdate))] private string _name = "Missing Name";
    [SyncVar(hook = nameof(HandleColorUpdate))] private Color _color;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    #region Server

    // Only can be called from the server
    [Server]
    public void SetName(string newName)
    {
        _name = newName;
        RpcDebugLog(_name);
    }
    
    [Server]
    public void SetColor(Color newColor)
    {
        _color = newColor;
    }

    // Client call this method to be run on the server
    [Command]
    private void CmdSetName(string newName)
    {
        if (IsValidName(newName))
        {
            SetName(newName);
        }
        else
        {
            throw new ArgumentException($"Argument {nameof(newName)} on the method {nameof(CmdSetName)}" +
                                        " is invalid.");
        }
    }

    private bool IsValidName(string nameToValidate)
    {
        var list = new[] {" ", "~", "`", "!", "?", "@", "#", "$", "%", "^", "&", "*", "(", ")", "+", "=", "\"", "'"};
        var isValidName = !string.IsNullOrEmpty(nameToValidate) &&
                          !nameToValidate.Any(c => list.Contains(c.ToString())) &&
                          nameToValidate.Length <= 10;
        return isValidName;
    }
    
    #endregion

    #region Client

    private void HandleNameUpdate(string oldName, string newName)
    {
        nameText.SetText(newName);
    }
    
    private void HandleColorUpdate(Color oldColor, Color newColor)
    {
        playerRenderer.material.SetColor(BaseColor, newColor);
    }

    // Show this method on menu inspector to be called if is clicked
    [ContextMenu("Set New Name")]
    private void SetNewName()
    {
        CmdSetName("NewName");
    }

    // Server call this method on all the clients
    [ClientRpc]
    private void RpcDebugLog(string message)
    {
        Debug.Log(message);
    }

    #endregion
}
