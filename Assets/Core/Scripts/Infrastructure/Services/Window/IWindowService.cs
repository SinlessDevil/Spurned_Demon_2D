using Window;

namespace Infrastructure.Services.Window
{
  public interface IWindowService
  {
    void Open(WindowTypeId windowTypeId);
  }
}