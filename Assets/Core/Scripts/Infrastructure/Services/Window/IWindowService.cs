using UnityEngine;
using UI.Window;

namespace Infrastructure.Services.Window
{
  public interface IWindowService
  {
    RectTransform Open(WindowTypeId windowTypeId);
  }
}