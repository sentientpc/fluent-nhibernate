using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ManyToOneInspector : IManyToOneInspector
    {
        private readonly ManyToOneMapping mapping;

        public ManyToOneInspector(ManyToOneMapping mapping)
        {
            this.mapping = mapping;
        }

        public Access Access
        {
            get { throw new NotImplementedException(); }
        }

        public string OuterJoin
        {
            get { return mapping.OuterJoin; }
        }

        public Type EntityType
        {
            get { throw new NotImplementedException(); }
        }
        public string StringIdentifierForModel
        {
            get { throw new NotImplementedException(); }
        }
        public bool IsSet(PropertyInfo property)
        {
            throw new NotImplementedException();
        }

        public PropertyInfo Property
        {
            get { return mapping.PropertyInfo; }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public TypeReference ClassValue
        {
            get { return mapping.Class ?? TypeReference.Empty; }
        }

        public IEnumerable<IColumnInspector> Columns
        {
            get
            {
                foreach (var column in mapping.Columns.UserDefined)
                {
                    yield return new ColumnInspector(mapping.ContainingEntityType, column);
                }
            }
        }

        public string Cascade
        {
            get { return mapping.Cascade; }
        }
    }
}