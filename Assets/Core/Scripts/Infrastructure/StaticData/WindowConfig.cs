using System;
using UnityEngine;
using UI.Window;

namespace Infrastructure.StaticData
{
  [Serializable]
  public class WindowConfig
  {
    public WindowTypeId WindowTypeId;
    public GameObject Prefab;
  }
}