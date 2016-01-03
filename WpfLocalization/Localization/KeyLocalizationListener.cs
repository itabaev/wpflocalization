using System.ComponentModel;

namespace WpfLocalization.Localization
{
    /// <summary>
    /// Слушатель изменения культуры при локализации по ключу
    /// </summary>
    public class KeyLocalizationListener : BaseLocalizationListener, INotifyPropertyChanged
    {
        public KeyLocalizationListener(string key, object[] args)
        {
            Key = key;
            Args = args;
        }

        private string Key { get; }

        private object[] Args { get; }

        public object Value
        {
            get
            {
                var value = LocalizationManager.Instance.Localize(Key);
                if (value is string && Args != null)
                    value = string.Format((string)value, Args);
                return value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnCultureChanged()
        {
            // Уведомляем привязку об изменении строки
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}
