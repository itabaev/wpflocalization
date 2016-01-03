using System.Collections.Generic;
using System.Globalization;
using WpfLocalization.Resources;

namespace WpfLocalization.Localization
{
    /// <summary>
    /// Реализация поставщика локализованных строк через ресурсы приложения
    /// </summary>
    public class ResxLocalizationProvider : ILocalizationProvider
    {
        private IEnumerable<CultureInfo> _cultures;

        public object Localize(string key)
        {
            return Strings.ResourceManager.GetObject(key);
        }

        public IEnumerable<CultureInfo> Cultures => _cultures ?? (_cultures = new List<CultureInfo>
        {
            new CultureInfo("ru-RU"),
            new CultureInfo("en-US"),
        });
    }
}
