using System;
using UnityEngine;
using Window;

namespace Infrastructure.StaticData
{
  [Serializable]
  public class WindowConfig
  {
    public WindowTypeId WindowTypeId;
    public GameObject Prefab;
  }
}