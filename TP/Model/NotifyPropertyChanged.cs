using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TP.Model
{
    /// <summary>
    /// Уведомление об изменении свойства
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения свойства
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Действие при изменении свойства
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
