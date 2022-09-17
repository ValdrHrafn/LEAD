using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    #region Variables
    [Header("Bindings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerTransform;
    #endregion

    void LateUpdate()
    {
        var angle = playerTransform.eulerAngles;
        angle.y = cameraTransform.localEulerAngles.y;
        playerTransform.eulerAngles = angle;
    }
}
