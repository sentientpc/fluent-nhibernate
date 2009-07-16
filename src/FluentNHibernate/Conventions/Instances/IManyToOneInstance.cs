using System;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IManyToOneInstance : IManyToOneInspector
    {
        void ColumnName(string columnName);
        void Class<T>();
        void Class(Type type);
        new IAccessInstance Access { get; }
        new ICascadeInstance Cascade { get; }
        new IFetchInstance Fetch { get; }
        IManyToOneInstance Not { get; }
        INotFoundInstance NotFound { get; }
        IOuterJoinInstance OuterJoin { get; }
        void Index(string index);
        void Insert();
        void LazyLoad();
        void Nullable();
        void PropertyRef(string property);
        void ReadOnly();
        void Unique();
        void UniqueKey(string key);
        void Update();
        void ForeignKey(string key);
    }
}