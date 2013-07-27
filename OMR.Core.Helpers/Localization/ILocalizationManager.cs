
namespace OMR.Core.Helpers.Localization
{
    public interface ILocalizationManager
    {
        bool Exists(string region, string name);
        T Get<T>(string region, string name);
        void Set(string region, string name, object value);
    }
}
