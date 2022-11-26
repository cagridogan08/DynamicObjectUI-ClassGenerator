using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DynamicViewWithDynamicObject.Models
{
    public class CustomObject : DynamicObject, INotifyPropertyChanged, ICustomTypeDescriptor
    {
        #region PropertyChanged Event
        public event PropertyChangedEventHandler? PropertyChanged;

        private  void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if(PropertyChanged != null)
            {
                if (dispatcher is not null)
                {
                    if (propertyName is not null && int.TryParse(this[propertyName].ToString(), out int val))
                    {
                        this[propertyName] = val;
                    }
                    else if(propertyName is not null && bool.TryParse(this[propertyName].ToString(),out bool value))
                    {
                        this[propertyName] = value;
                    }
                    dispatcher.Invoke( PropertyChanged, new object[] { this, new PropertyChangedEventArgs(propertyName) });
                }
                else
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Dictionary for propertName and Property for DynamicObject
        /// </summary>
        internal Dictionary<string, object> dictionary = new();
        Dispatcher? dispatcher;
        /// <summary>
        /// Lock CustomModule
        /// </summary>
        private object LockObject = new();

        #endregion
        #region Constructor
        public CustomObject(Dispatcher value)
        {
            this.dispatcher = value;
        }
        public CustomObject()
        {

        }
        public CustomObject(Dictionary<string, object> dictionary)
        {
            this.dictionary = dictionary;
        }

        #endregion
        #region Methods
        public object this[string name]
        {
            get
            {
                lock (LockObject)
                {
                    return dictionary[name];
                }
            }
            set
            {
                object? oldValue = null;
                lock (LockObject)
                {
                    if (dictionary.ContainsKey(name))
                    {
                        oldValue = dictionary[name];
                    }
                    dictionary[name] = value;
                }
                if (oldValue is not null && !oldValue.Equals(value))
                {
                    OnPropertyChanged(name);
                }
            }
        }
        public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this, true);

        public string? GetClassName()=>TypeDescriptor.GetClassName(this,true);

        public string? GetComponentName() => TypeDescriptor.GetComponentName(this, true);

        public TypeConverter GetConverter() => TypeDescriptor.GetConverter(this, true);

        public EventDescriptor? GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);

        public PropertyDescriptor? GetDefaultProperty() => TypeDescriptor.GetDefaultProperty(this, true);

        public object? GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(this, editorBaseType, true);

        public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(this, true);

        public EventDescriptorCollection GetEvents(Attribute[]? attributes)=>TypeDescriptor.GetEvents(this, attributes,true);

        public PropertyDescriptorCollection GetProperties() => this.GetProperties(Array.Empty<Attribute>());

        public PropertyDescriptorCollection GetProperties(Attribute[]? attributes)
        {
            var props = new List<CustomPropertyDescriptor>();
            foreach (var item in dictionary)
            {
                props.Add(new CustomPropertyDescriptor(item.Key, (item.Value?.GetType()) ?? typeof(object)));
            }
            return new PropertyDescriptorCollection(props.ToArray());
        }

        public object? GetPropertyOwner(PropertyDescriptor? pd) => dictionary;
        #endregion
    }

    internal class CustomPropertyDescriptor : PropertyDescriptor
    {
        Type type;
        string key;
        public CustomPropertyDescriptor(string key, Type type)
            : base(key, null)
        {
            this.type = type;
            this.key = key;
        }
        void PropertyChanged(object? sender, EventArgs e)
        {
            OnValueChanged(sender, e);
        }
        public override void AddValueChanged(object component, EventHandler handler)
        {
            base.AddValueChanged(component, handler);
            ((INotifyPropertyChanged)component).PropertyChanged += PropertyChanged;
        }
        public override void RemoveValueChanged(object component, EventHandler handler)
        {
            base.RemoveValueChanged(component, handler);
            ((INotifyPropertyChanged)component).PropertyChanged -= PropertyChanged;
        }
        public override Type PropertyType
        {
            get { return type; }
        }
        public override void SetValue(object component, object value)
        {
            ((CustomObject)component)[key] = value;
        }
        public override object GetValue(object component)
        {
            return ((CustomObject)component)[key];
        }
        public override bool IsReadOnly
        {
            get { return false; }
        }
        public override Type ComponentType
        {
            get { return null; }
        }
        public override bool CanResetValue(object component)
        {
            return false;
        }
        public override void ResetValue(object component)
        {
        }
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
