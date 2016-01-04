using System;
using System.Windows;

namespace WpfLocalization.Localization
{
    /// <summary>
    /// Базовый класс для слушателей изменения культуры
    /// </summary>
    public abstract class BaseLocalizationListener : IWeakEventListener, IDisposable
    {
        protected BaseLocalizationListener()
        {
            CultureChangedEventManager.AddListener(LocalizationManager.Instance, this);
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(CultureChangedEventManager))
            {
                OnCultureChanged();
                return true;
            }
            return false;
        }

        protected abstract void OnCultureChanged();

        public void Dispose()
        {
            CultureChangedEventManager.RemoveListener(LocalizationManager.Instance, this);
            GC.SuppressFinalize(this);
        }
    }
}
