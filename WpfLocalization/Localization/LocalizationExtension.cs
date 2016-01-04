using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfLocalization.Localization
{
    /// <summary>
    /// Расширение разметки, которое возвращает локализованную строку по ключу или привязке
    /// </summary>
    [ContentProperty(nameof(ArgumentBindings))]
    public class LocalizationExtension : MarkupExtension
    {
        private Collection<BindingBase> _arguments;

        public LocalizationExtension()
        {
        }

        public LocalizationExtension(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Ключ локализованной строки
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Привязка для ключа локализованной строки
        /// </summary>
        public Binding KeyBinding { get; set; }

        /// <summary>
        /// Аргументы форматируемой локализованный строки
        /// </summary>
        public IEnumerable<object> Arguments { get; set; }

        /// <summary>
        /// Привязки аргументов форматируемой локализованный строки
        /// </summary>
        public Collection<BindingBase> ArgumentBindings
        {
            get { return _arguments ?? (_arguments = new Collection<BindingBase>()); }
            set { _arguments = value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Key != null && KeyBinding != null)
                throw new ArgumentException($"Нельзя одновременно задать {nameof(Key)} и {nameof(KeyBinding)}");
            if (Key == null && KeyBinding == null)
                throw new ArgumentException($"Необходимо задать {nameof(Key)} или {nameof(KeyBinding)}");
            if (Arguments != null && ArgumentBindings.Any())
                throw new ArgumentException($"Нельзя одновременно задать {nameof(Arguments)} и {nameof(ArgumentBindings)}");

            var target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (target.TargetObject.GetType().FullName == "System.Windows.SharedDp")
                return this;

            // Если заданы привязка ключа или список привязок аргументов,
            // то используем BindingLocalizationListener
            if (KeyBinding != null || ArgumentBindings.Any())
            {
                var listener = new BindingLocalizationListener();

                // Создаем привязку для слушателя
                var listenerBinding = new Binding { Source = listener };

                var keyBinding = KeyBinding ?? new Binding { Source = Key };

                var multiBinding = new MultiBinding
                {
                    Converter = new BindingLocalizationConverter(),
                    ConverterParameter = Arguments,
                    Bindings = { listenerBinding, keyBinding }
                };

                // Добавляем все переданные привязки аргументов
                foreach (var binding in ArgumentBindings)
                    multiBinding.Bindings.Add(binding);

                var value = multiBinding.ProvideValue(serviceProvider);
                // Сохраняем выражение привязки в слушателе
                listener.SetBinding(value as BindingExpressionBase);
                return value;
            }

            // Если задан ключ, то используем KeyLocalizationListener
            if (!string.IsNullOrEmpty(Key))
            {
                var listener = new KeyLocalizationListener(Key, Arguments?.ToArray());

                // Если локализация навешана на DependencyProperty объекта DependencyObject
                if ((target.TargetObject is DependencyObject && target.TargetProperty is DependencyProperty) ||
                    target.TargetObject is Setter)
                {
                    var binding = new Binding(nameof(KeyLocalizationListener.Value))
                    {
                        Source = listener,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    return binding.ProvideValue(serviceProvider);
                }

                // Если локализация навешана на Binding, то возвращаем слушателя
                var targetBinding = target.TargetObject as Binding;
                if (targetBinding != null && target.TargetProperty != null &&
                    target.TargetProperty.GetType().FullName == "System.Reflection.RuntimePropertyInfo")
                {
                    targetBinding.Path = new PropertyPath(nameof(KeyLocalizationListener.Value));
                    targetBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    return listener;
                }

                // Иначе возвращаем локализованную строку
                return listener.Value;
            }

            return null;
        }
    }
}
