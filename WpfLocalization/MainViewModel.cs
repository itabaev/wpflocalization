using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using WpfLocalization.Localization;

namespace WpfLocalization
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private IEnumerable<CultureInfo> _cultureInfos;
        private CultureInfo _currentCulture;
        private IEnumerable<SomeEnum> _someEnums;
        private string _name = "Bob";
        private string _profession;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<CultureInfo> CultureInfos
        {
            get { return _cultureInfos ?? (_cultureInfos = LocalizationManager.Instance.Cultures); }
            set
            {
                if (Equals(value, _cultureInfos)) return;
                _cultureInfos = value;
                OnPropertyChanged();
            }
        }

        public CultureInfo CurrentCulture
        {
            get { return _currentCulture ?? (_currentCulture = LocalizationManager.Instance.CurrentCulture); }
            set
            {
                if (Equals(value, _currentCulture)) return;
                _currentCulture = value;
                LocalizationManager.Instance.CurrentCulture = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<SomeEnum> SomeEnums
        {
            get { return _someEnums ?? (_someEnums = Enum.GetValues(typeof(SomeEnum)).Cast<SomeEnum>()); }
            set
            {
                if (Equals(value, _someEnums)) return;
                _someEnums = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Profession
        {
            get { return _profession; }
            set
            {
                if (value == _profession) return;
                _profession = value;
                OnPropertyChanged();
            }
        }
    }

    public enum SomeEnum
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
    }
}
