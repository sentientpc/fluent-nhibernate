using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapManyToMany : IAutoMapper
    {
        private readonly AutoMappingExpressions conventions;

        public AutoMapManyToMany(AutoMappingExpressions conventions)
        {
            this.conventions = conventions;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            var type = property.PropertyType;
            if (type.Namespace != "Iesi.Collections.Generic" &&
                type.Namespace != "System.Collections.Generic")
                return false;

            var hasInverse = GetInverseProperty(property) != null;
            return hasInverse;
        }

        private static PropertyInfo GetInverseProperty(PropertyInfo property)
        {
            Type type = property.PropertyType;
            var inverseSide = type.GetGenericTypeDefinition()
                .MakeGenericType(property.DeclaringType);

            var argument = type.GetGenericArguments()[0];
            return argument.GetProperties()
                .Where(x => x.PropertyType == inverseSide)
                .FirstOrDefault();
        }

        private ICollectionMapping GetCollection(PropertyInfo property)
        {
            if (property.PropertyType.FullName.Contains("ISet"))
                return new SetMapping();

            return new BagMapping();
        }

        private void ConfigureModel(PropertyInfo property, ICollectionMapping mapping, ClassMappingBase classMap, Type parentSide)
        {
            // TODO: Make the child type safer
            mapping.SetDefaultValue(x => x.Name, property.Name);
            mapping.Relationship = CreateManyToMany(property, property.PropertyType.GetGenericArguments()[0]);
            mapping.ContainingEntityType = classMap.Type;
            mapping.Key = new KeyMapping();
            mapping.ChildType = property.PropertyType.GetGenericArguments()[0];
            mapping.MemberInfo = property;

            if (parentSide != property.DeclaringType)
                mapping.Inverse = true;
        }

        private ICollectionRelationshipMapping CreateManyToMany(PropertyInfo property, Type child)
        {
            var mapping = new ManyToManyMapping
            {
                Class = new TypeReference(property.PropertyType.GetGenericArguments()[0])
            };

            mapping.AddDefaultColumn(new ColumnMapping { Name = child.Name + "_id" });

            return mapping;
        }

        public void Map(ClassMapping classMap, PropertyInfo property)
        {
            var inverseProperty = GetInverseProperty(property);
            var parentSide = conventions.GetParentSideForManyToMany(property.DeclaringType, inverseProperty.DeclaringType);
            var mapping = GetCollection(property);

            ConfigureModel(property, mapping, classMap, parentSide);

            classMap.AddCollection(mapping);
        }

        public void Map(JoinedSubclassMapping classMap, PropertyInfo property)
        {
            var inverseProperty = GetInverseProperty(property);
            var parentSide = conventions.GetParentSideForManyToMany(property.DeclaringType, inverseProperty.DeclaringType);
            var mapping = GetCollection(property);

            ConfigureModel(property, mapping, classMap, parentSide);

            classMap.AddCollection(mapping);
        }

        public void Map(SubclassMapping classMap, PropertyInfo property)
        {
            var inverseProperty = GetInverseProperty(property);
            var parentSide = conventions.GetParentSideForManyToMany(property.DeclaringType, inverseProperty.DeclaringType);
            var mapping = GetCollection(property);

            ConfigureModel(property, mapping, classMap, parentSide);

            classMap.AddCollection(mapping);
        }
    }
}
