using System.Collections.Generic;
using System.Globalization;

namespace WpfLocalization.Localization
{
    /// <summary>
    /// Интерфейс для реализации поставщика локализованных строк
    /// </summary>
    public interface ILocalizationProvider
    {
        /// <summary>
        /// Возвращает локализованный объект по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        object Localize(string key);

        /// <summary>
        /// Доступные культуры
        /// </summary>
        IEnumerable<CultureInfo> Cultures { get; }
    }
}
