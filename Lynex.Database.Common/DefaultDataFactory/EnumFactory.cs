using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lynex.Model.Domain.DbModels;
using Lynex.Model.Domain.DbModels.Interface;
using Lynex.Model.Enum;
using Lynex.Model.Enum.Mapable;
using NHibernate;

namespace Lynex.Database.Common.DefaultDataFactory
{
    internal class EnumFactory : DefaultDataFactoryBase<IBaseEntity>
    {
        public EnumFactory(ISession session) : base(session)
        {
        }

        protected override IEnumerable<IBaseEntity> GetData()
        {
            foreach (var mapableEnum in Assembly.Load("Lynex.Model").GetTypes().Where(q => q.Namespace == "Lynex.Model.Enum.Mapable"))
            {

                foreach (var enumItem in Enum.GetValues(mapableEnum))
                {
                    var d1 = typeof(EnumTable<>);
                    Type[] typeArgs = { mapableEnum };
                    var makeme = d1.MakeGenericType(typeArgs);
                    var o = (IBaseEntity)Activator.CreateInstance(makeme);
                    var type = o.GetType();
                    var idProperty = type.GetProperty("Id");
                    var nameProperty = type.GetProperty("Name");

                    idProperty.SetValue(o, (int)enumItem, null);
                    nameProperty.SetValue(o, enumItem.ToString(), null);

                    yield return o;
                }

            }

            
        }
    }
}
