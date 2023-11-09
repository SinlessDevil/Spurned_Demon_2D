using UnityEngine;
using Window;

namespace Services.Factories.UIFactory
{
  public interface IUIFactory
  {
    void CreateUiRoot();
    RectTransform CrateWindow(WindowTypeId windowTypeId);
  }
}