using System;

namespace WpfLocalization.Localization
{
    /// <summary>
    /// Базовый класс для слушателей изменения культуры
    /// </summary>
    public abstract class BaseLocalizationListener
    {
        protected BaseLocalizationListener()
        {
            LocalizationManager.Instance.CultureChanged += CultureChanged;
        }

        private void CultureChanged(object sender, EventArgs eventArgs)
        {
            OnCultureChanged();
        }

        protected abstract void OnCultureChanged();

        ~BaseLocalizationListener()
        {
            LocalizationManager.Instance.CultureChanged -= CultureChanged;
        }
    }
}
