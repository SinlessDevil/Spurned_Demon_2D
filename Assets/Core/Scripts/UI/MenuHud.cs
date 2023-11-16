using Infrastructure.Services.LocalizationService;
using UnityEngine;

namespace UI
{
    public class MenuHud : MonoBehaviour
    {
        [field: SerializeField] public GameObject Menu;
        [field: SerializeField] public LocalizeMenu localizeMenu;
    }
}