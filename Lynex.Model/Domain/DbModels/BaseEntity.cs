using System;
using System.Collections.Generic;
using System.Linq;
using Lynex.Model.Domain.DbModels.Interface;

namespace Lynex.Model.Domain.DbModels
{
    public abstract class BaseEntity : IBaseEntity
    {
        public virtual long Id { get; set; }

        protected BaseEntity()
        {
            foreach (var property in GetType().GetProperties())
            {
                if (property.PropertyType.IsGenericType)
                {
                    if (property.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null)
                    {
                        property.SetValue(this, Activator.CreateInstance(property.PropertyType));
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        property.SetValue(this, DateTime.UtcNow);
                    }
                }
            }

            CallInit();
        }

        private void CallInit()
        {
            Init();
        }

        protected virtual void Init()
        {
            
        }
    }
}
